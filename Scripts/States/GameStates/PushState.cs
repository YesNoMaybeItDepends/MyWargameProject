using Godot;
using System;
using System.Collections.Generic;

public class PushState : State
{
    Map board;
    Unit pusher;
    Unit target;
    BlockDie.Faces dieResult;
    List<Hex> pushbackHexes = new List<Hex>();

    bool followingUp = false;
    List<Hex> followUpHexes = new List<Hex>();

    public PushState(StateManager Owner, BlockDie.Faces dieResult_, Unit pusher_, Unit pushed) : base(Owner)
    {
        board = ServiceProvider.GetService<GameManager>().board;

        dieResult = dieResult_;
        pusher = pusher_;
        target = pushed;

        // Find vector from pusher to target
        AxialCoordinates directionVector = target.tile.axialPos - this.pusher.tile.axialPos;
        
        // Get position behind target
        AxialCoordinates posBehindPushed = pushed.tile.axialPos + (directionVector);
        Hex centerHex = board.getHexAt(posBehindPushed.ToOffset());
        
        if (centerHex != null)
        {   
            pushbackHexes.Add(centerHex);

            var leftHex = board.getHexAt(pushed.tile.offsetPos.rotateVectorLeft(centerHex.offsetPos));
            var rightHex = board.getHexAt(pushed.tile.offsetPos.rotateVectorRight(centerHex.offsetPos));

            if (leftHex != null && rightHex != null)
            {
                pushbackHexes.Add(leftHex);
                pushbackHexes.Add(rightHex);
            }
        }

        foreach(Hex hex in pushbackHexes)
        {
            hex.terrain.sprite.Modulate = Colors.Yellow;
        }
    }

    public async override void handleEvent(object o)
    {
        if (Input.IsActionJustPressed("mouse_click_right"))
        {
            if (o is Hex hex)
            {
                if (!followingUp)
                {
                    if (pushbackHexes.Contains(hex))
                    {
                        // push tween
                        Tween tween = new Tween();
                        target.AddChild(tween);
                        tween.InterpolateProperty(target, "global_transform:origin", target.GlobalPosition, hex.GlobalPosition, 0.5f, Tween.TransitionType.Quint , Tween.EaseType.Out);

                        // start push tween
                        tween.Start();
                        await ToSignal(tween, "tween_completed");
                        
                        // Add hexes to follow up later
                        followUpHexes.Add(target.tile);
                        followUpHexes.Add(pusher.tile);

                        // push complete
                        target.tile.unit = null;
                        hex.unit = target;
                        target.Position = new Vector2(0,0);

                        // defender down / stumbled
                        if (dieResult == BlockDie.Faces.DefenderDown || dieResult == BlockDie.Faces.DefenderStumbles)
                        {
                            target.tile.unit = null;
                            target.QueueFree();
                        }

                        // follow up?
                        followingUp = true;
                        foreach (Hex h in followUpHexes)
                        {
                            h.terrain.sprite.Modulate = Colors.Blue;
                        }
                    }
                }
                else
                {
                    if (followUpHexes.Contains(hex))
                    {
                        if (hex != pusher.tile)
                        {
                            // move tween
                            Tween tween = new Tween();
                            pusher.AddChild(tween);
                            tween.InterpolateProperty(pusher, "global_transform:origin", pusher.GlobalPosition, hex.GlobalPosition, 0.5f, Tween.TransitionType.Quint , Tween.EaseType.Out);

                            // start move tween
                            tween.Start();
                            await ToSignal(tween, "tween_completed");

                            pusher.tile.unit = null;
                            hex.unit = pusher;
                            pusher.Position = new Vector2(0,0);
                        }

                        foreach (Hex h in followUpHexes)
                        {
                            h.terrain.sprite.Modulate = new Color(1,1,1);
                        }

                        // end state
                        UnitSelectedState newState = new UnitSelectedState(owner, pusher);
                        owner.state = newState;
                    }
                }
            }
        }
    }

    void onTweenCompleted(Godot.Object o, NodePath p)
    {

    }

    public override void stateExit()
    {
        foreach (Hex hex in pushbackHexes)
        {
            hex.terrain.sprite.Modulate = new Color(1,1,1);
        }
    }
}