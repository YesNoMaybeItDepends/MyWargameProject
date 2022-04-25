using Godot;
using System;

public class Unit : InteractiveEntity
{
    public Hex tile;

    bool canMove;
    bool canAttack;
    bool selected = false;

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
        //sprite.Modulate = Colors.Green;
        Helpers.GetStateManager().state.handleMouseEnter(this);
    }
    
    public override void handleOnMouseExited()
    {
        sprite.Modulate = new Color(1f,1f,1f);

        Helpers.GetStateManager().state.handleMouseExit(this);
    }

    public override void onInputEvent(Godot.Object viewport, InputEvent @event, int shape_idx)
    {

        Helpers.GetStateManager().state.handleEvent(this);
    }

    public void handleInput()
    {
        GD.Print("NOOOOO");
    }
}



