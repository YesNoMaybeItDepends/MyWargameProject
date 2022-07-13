using System;
using System.Collections.Generic;
using Godot;

public class UnitMovingState : State
{
    bool moving = false;
    Unit unit;
    public Queue<Hex> path = new Queue<Hex>();
    public List<Hex> realPath = new List<Hex>();
    int i;
    public List<Hex> pathsToAdd = new List<Hex>();
    Tween tween = new Tween();

    Hex oldPos;
    Hex newPos;

    public Hex targetPos;

    public UnitMovingState(StateManager Owner, Unit Unit, List<Hex> Path) : base(Owner)
    {
        unit = Unit;
        path = new Queue<Hex>(Path);
        targetPos = Path[Path.Count-1];
        if (targetPos != null)
        {
            GD.Print("np");
        }
        path.Dequeue();
        AddChild(tween);

        tween.Connect("tween_completed", this, "OnMoveCompleted");
    }

    public bool Move()
    {
        bool success = false;
        if (path.Count == 0 && pathsToAdd.Count != 0)
        {
            // for (int y = 1; y != pathsToAdd.Count; y++)
            // {
                // path.Enqueue(pathsToAdd[y]);
            // }
            foreach(Hex hex in pathsToAdd)
            {
                GD.Print("adding new memes");
                path.Enqueue(hex);
            }

            targetPos = pathsToAdd[pathsToAdd.Count-1];
            pathsToAdd.Clear();
            GD.Print("Path Count: "+path.Count);
        }

        if (path.Count != 0)
        {
            newPos = path.Dequeue();
            oldPos = unit.tile;

            if (newPos != null)
            {
                if (newPos.unit is null)
                {
                    success = true;

                    int distanceToHex = newPos.axialPos.DistanceTo(unit.tile.axialPos);

                    // We add a delay of 0.01f because otherwise there's a weird effect where, after the tween finishes, the unit is teleported back to its previous tile for a milisecond before teleporting back to its new position. It only happens when the unit has moved more than 1 tile.

                    tween.InterpolateProperty(unit, "global_transform:origin", unit.GlobalPosition, newPos.GlobalPosition, 0.5f, Tween.TransitionType.Quint, Tween.EaseType.Out, 0.01f);

                    
                    tween.Start();
                }
            }
        }

        return success;
    }

    void OnMoveCompleted(Godot.Object obj, NodePath key)
    {
        unit.tile.unit = null;
        newPos.unit = unit;
        unit.Position = Vector2.Zero;

        if (!Move())
        {
            GD.Print("finished");
            owner.state = new DefaultState(owner);
        }
    }
}