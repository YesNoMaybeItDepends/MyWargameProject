using Godot;
using System;

public class Unit : Area2D, Iinput
{
    public Sprite sprite;
    public String name;
    public Hex tile;

    bool canMove;
    bool canAttack;
    bool selected = false;

    public int maxMovementPoints = 3;
    public int movementPoints = 3;
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

    public Unit(String name, String texturePath)
    {
        sprite = new Sprite();
        sprite.Texture = GD.Load(texturePath) as Texture;
        AddChild(sprite);

        CollisionPolygon2D collPolygon = new CollisionPolygon2D();
        collPolygon.Polygon = HexPolygon;
        collPolygon.Position = new Vector2(-36, -36);
        AddChild(collPolygon);

        //InputPickable = true;
        CollisionLayer = 1;

        Name = name;
    }

    // public void handleInput(InputEvent e)
    // {
    //     GD.Print("SNEED");
    //     if (e is InputEventMouseButton button)
    //     {
    //         // if left click
    //         if (button.Pressed && button.ButtonIndex == (int)ButtonList.Left)
    //         {
    //             // Select Unit if is not selected
    //             if (!selected)
    //             {
    //                 GD.Print("sneed");
    //                 selected = true;
    //                 //sprite.Modulate = new Color(0.9f,0.9f,0.9f);
    //                 sprite.Modulate = Colors.Green;
    //             }
    //         }
    //     }
    // }

    public void Move(Hex tile)
    {
        
    }

    public void handleInput()
    {
        GD.Print("NOOOOO");
        // // if left click
        // if (Input.IsMouseButtonPressed((int)ButtonList.Left))
        // {
        //     // Select Unit if is not selected
        //     if (!selected)
        //     {
        //         GD.Print("sneed");
        //         selected = true;
        //         //sprite.Modulate = new Color(0.9f,0.9f,0.9f);
        //         sprite.Modulate = Colors.Green;
        //     }
        // }
        
        // // if right click
        // if (Input.IsMouseButtonPressed((int)ButtonList.Left))
        // {
        //     if (selected)
        //     {

        //     }
        // }
    }
}



