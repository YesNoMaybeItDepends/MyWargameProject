using Godot;
using System;

public class DefaultState : State
{
    public DefaultState(StateManager Owner) : base(Owner)
    {
        Name = "State DefaultState";
    }

    public override void handleEvent(object o)
    {
        // Handle common input
        switch (o)
        {
            case Terrain terrain:
                // GD.Print("terrain");
                break;
            case Unit unit:
                if (Input.IsActionJustPressed("mouse_click_left"))
                {
                    UnitSelectedState unitSelected = new UnitSelectedState(owner, unit);
                    owner.state = unitSelected;
                }
                break;
            case Hex hex:
                // GD.Print("hex");
                break;
            case null:
                GD.Print("UH OH, NULL, STINKY!!");
                break;
            default:
                GD.Print("default");
                break;
        }
    }
}