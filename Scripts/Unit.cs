using Godot;
using System;

public class Unit : InteractiveEntity
{
    public Hex tile;

    bool canMove;
    bool canAttack;
    public bool selected = false;

    public int maxMovementPoints = 3;
    public int movementPoints = 3;

    internal Unit(String name, String texturePath)
    {
        sprite.Texture = GD.Load(texturePath) as Texture;

        Name = name;
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
                GD.Print("np");
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



