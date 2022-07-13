using Godot;
using System;

//public class Terrain : Area2D
public class Terrain : InteractiveEntity
{
    public CollisionPolygon2D HexShape;
    public int MovementCost = 1;
 
    public Vector2[] HexPolygon = new Vector2[8]
    {
        new Vector2(80,40),
        new Vector2(80,44),
        new Vector2(59,80),
        new Vector2(13,80),
        new Vector2(-8,44),
        new Vector2(-8,40),
        new Vector2(13,4),
        new Vector2(59,4)
    }; 

    public Terrain(String name, String texturePath)
    {
        sprite.Texture = GD.Load(texturePath) as Texture;

        Name = "Plains";
    }

    public Terrain(String name, int movementCost, Texture texture)
    {
        sprite.Texture = texture;
        Name = name;
        MovementCost = movementCost;
    }

    public override void handleOnMouseEntered()
    {
        Modulate = new Color(0.9f,0.9f,0.9f);
    }
    
    public override void handleOnMouseExited()
    {
        Modulate = new Color(1,1,1);
    }

    public override void onInputEvent(Godot.Object viewport, InputEvent @event, int shape_idx)
    {
        ServiceProvider.GetService<Inputcontroller>().handleEvent(this);
    }
}