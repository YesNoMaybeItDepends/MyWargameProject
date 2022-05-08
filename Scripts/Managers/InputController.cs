using Godot;
using System;

public class Inputcontroller : Node2D, IService
{
    public GameManager Level;
    public StateManager stateManager;

    public Unit hoveredUnit;

    public Inputcontroller()
    {
        ServiceProvider.SetService<Inputcontroller>(this);
    }

    public void Initialize(StateManager StateManager)
    {
        stateManager = StateManager;
    }

    public void handleEvent(object o)
    {
        // // Handle common input
        // switch (o)
        // {
        //     case Terrain terrain:
        //         // GD.Print("terrain");
        //         break;
        //     case Unit unit:
        //         if (Input.IsActionJustPressed("mouse_click_left"))
        //         {
        //             UnitSelectedState unitSelected = new UnitSelectedState(stateManager, unit);
        //             stateManager.state = unitSelected;
        //         }
        //         break;
        //     case Hex hex:
        //         // GD.Print("hex");
        //         break;
        //     case null:
        //         GD.Print("UH OH, NULL, STINKY!!");
        //         break;
        //     default:
        //         GD.Print("default");
        //         break;
        // }

        // Handle state input
        stateManager.handleEvent(o);
    }

    public void handleMouseEnter(object o)
    {
        stateManager.handleMouseEnter(o);
    }

    public void handleMouseExit(object o)
    {
        stateManager.handleMouseExit(o);
    }
}