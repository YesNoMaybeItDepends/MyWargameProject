using Godot;
using System;

public class DefaultState : State
{
    public DefaultState(StateManager Owner) : base(Owner)
    {
        Name = "State DefaultState";
    }

    public override void handleInput(Godot.Collections.Array ColliderDicts)
    {
        // If Left Clicking
        if (Input.IsMouseButtonPressed((int)ButtonList.Left))
        {
            // Select Unit
            foreach (Godot.Collections.Dictionary collider in ColliderDicts)
            {
                if (collider["collider"] is Unit unit)
                {
                    
                    //col.handleInput();
                    unit.sprite.Modulate = Colors.GreenYellow;
                    UnitSelected unitSelected = new UnitSelected(owner,unit);
                    owner.state = unitSelected;
                    return;
                }
            }
        }
    }

    public override void handleEvent(object o)
    {
        switch (o)
        {
            case Terrain terrain:
                GD.Print("terrain");
                break;
            case Unit unit:
                GD.Print("unit");
                break;
            case Hex hex:
                GD.Print("hex");
                break;
            case null:
                GD.Print("UH OH, NULL, STINKY!!");
                break;
            default:
                GD.Print("deafult");
                break;
        }
    }
}