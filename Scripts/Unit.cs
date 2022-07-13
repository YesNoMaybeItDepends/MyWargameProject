using Godot;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

public class Unit : InteractiveEntity
{
    public Hex tile;
    public StateManager unitStateManager;

    bool canMove;
    bool canAttack;
    public bool selected = false;

    public int maxMovementPoints = 3;
    public int movementPoints = 3;
    
    internal Unit(String name, String texturePath)
    {
        sprite.Texture = GD.Load(texturePath) as Texture;

        Name = name;

        ZIndex = 1;

        unitStateManager = new StateManager();
        AddChild(unitStateManager);

        DefaultUnitState defaultUnitState = new DefaultUnitState(unitStateManager);
        unitStateManager.state = defaultUnitState;
    }

    public bool Move(Hex hex)
    {
        if (hex.unit is null)
        {
            int distanceTo = tile.axialPos.DistanceTo(hex.axialPos);

            if (movementPoints >= distanceTo) // is valid goal
            {
                List<Hex> path;
                UnitMovingState s;
                // if unit is already moving
                if (unitStateManager.state is UnitMovingState unitMovingState)
                {
                    GD.Print("sneed");
                    s = unitMovingState;
                    path = Pathfinding.GetPath(unitMovingState.targetPos, hex);
                    path.RemoveAt(0);
                    GD.Print(s.pathsToAdd.Count);
                    s.pathsToAdd.AddRange(path);
                    GD.Print(s.pathsToAdd.Count);
                }
                else
                {
                    path = Pathfinding.GetPath(tile, hex);
                    s = new UnitMovingState(unitStateManager, this, path);
                    unitStateManager.state = s;
                    s.Move();
                }
                
                return true;
            }
        }
        return false;
    }

    public override void handleOnMouseEntered()
    {
        sprite.Modulate = new Color(0.9f,0.9f,0.9f);
        
        TargetPanel t = ServiceProvider.GetService<GuiManager>().targetPanel;
        SelectedPanel s = ServiceProvider.GetService<GuiManager>().selectedPanel;
        if (s.unit == null || s.unit.selected == false)
        {
            s.Set(this);
        }
        else if (s.unit != this)
        {
            t.Set(this);
        }

        ServiceProvider.GetService<Inputcontroller>().handleMouseEnter(this);
    }
    
    public override void handleOnMouseExited()
    {
        sprite.Modulate = new Color(1f,1f,1f);
        TargetPanel t = ServiceProvider.GetService<GuiManager>().targetPanel;
        SelectedPanel s = ServiceProvider.GetService<GuiManager>().selectedPanel;

        // when we exit a unit

        // if theres a unit in s
        if (s.unit != null)
        {
            // if unit is selected
            if (s.unit.selected)
            {
                // hide target
                if (t.unit != null && t.unit == this)
                {
                    t.unit = null;
                    t.Visible = false;
                }
            }
            // unit is not selected, we can hide
            else
            {
                // if it's this unit
                if (s.unit == this)
                {
                    s.unit = null;
                    s.Visible = false;
                }
            }
        }
        // theres NO unit in s
        // do nothing


        // Selected Panel
        // If panel is not null and unit is not selected, hide
        // if (s.unit != null && s.unit.selected == false && s.unit == this)
        // {
        //     s.unit = null;
        //     s.Visible = false;
        // }
        // // Target Panel
        // // If panel is not null and is not unit, hide
        // if (t.unit != null && t.unit == this)
        // {
        //     t.unit = null;
        //     t.Visible = false;
        // }

        ServiceProvider.GetService<Inputcontroller>().handleMouseExit(this);

    }

    public override void onInputEvent(Godot.Object viewport, InputEvent @event, int shape_idx)
    {
        ServiceProvider.GetService<Inputcontroller>().handleEvent(this);
    }

    public void handleInput()
    {
        GD.Print("NOOOOO");
    }
}



