using Godot;
using System;
using System.Collections.Generic;

public class PushState : State
{
    Unit pusher;
    Unit target;
    BlockDie.Faces dieResult;
    List<Hex> pushbackHexes = new List<Hex>();

    public PushState(StateManager Owner, BlockDie.Faces dieResult_, Unit pusher_, Unit pushed) : base(Owner)
    {
        dieResult = dieResult_;
        pusher = pusher_;
        target = pushed;

        // Find vector from pusher to target
        AxialCoordinates directionVector = target.tile.axialPos - this.pusher.tile.axialPos;
        
        // Get position behind target
        AxialCoordinates posBehindPushed = pushed.tile.axialPos + (directionVector);
        Hex centerHex = owner.board.getHexAt(posBehindPushed.ToOffset());
        
        if (centerHex != null)
        {   
            pushbackHexes.Add(centerHex);

            var leftHex = owner.board.getHexAt(pushed.tile.offsetPos.rotateVectorLeft(centerHex.offsetPos));
            var rightHex = owner.board.getHexAt(pushed.tile.offsetPos.rotateVectorRight(centerHex.offsetPos));

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

    public override void handleEvent(object o)
    {
        if (Input.IsActionJustPressed("mouse_click_right"))
        {
            if (o is Hex hex)
            {
                if (pushbackHexes.Contains(hex))
                {
                    target.tile.unit = null;
                    hex.unit = target;
                    target.Position = new Vector2(0,0);

                    if (dieResult == BlockDie.Faces.DefenderDown || dieResult == BlockDie.Faces.DefenderStumbles)
                    {
                        target.tile.unit = null;
                        target.QueueFree();
                    }

                    DefaultState newState = new DefaultState(owner);
                    owner.state = newState;
                }
            }
        }
    }

    public override void stateExit()
    {
        foreach (Hex hex in pushbackHexes)
        {
            hex.terrain.sprite.Modulate = new Color(1,1,1);
        }
    }
}