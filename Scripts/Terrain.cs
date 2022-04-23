using Godot;
using System;

//public class Terrain : Area2D
public class Terrain : Area2D, Iinput
{
    public Sprite sprite;
    public String name; 
    //public Control control;

    public CollisionPolygon2D HexShape;
    
    // Wesnoth polygon
    // public Vector2[] HexPolygon = new Vector2[8]
    // {
    //     new Vector2(72,35),
    //     new Vector2(72,37),
    //     new Vector2(54,72),
    //     new Vector2(18,72),
    //     new Vector2(0,37),
    //     new Vector2(0,35),
    //     new Vector2(18,0),
    //     new Vector2(54,0)
    // }; 

    // Zeshio Polygon with edges
    // public Vector2[] HexPolygon = new Vector2[8]
    // {
    //     new Vector2(84,39),
    //     new Vector2(84,45),
    //     new Vector2(60,84),
    //     new Vector2(12,84),
    //     new Vector2(-12,45),
    //     new Vector2(12,39),
    //     new Vector2(12,0),
    //     new Vector2(60,0)
    // }; 

    // Zeshio Polygon without edges -1,-1
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
        sprite = new Sprite();
        sprite.Texture = GD.Load(texturePath) as Texture;
        AddChild(sprite);

        CollisionPolygon2D collPolygon = new CollisionPolygon2D();
        collPolygon.Polygon = HexPolygon;
        collPolygon.Position = new Vector2(-36, -36);
        AddChild(collPolygon);

        //InputPickable = true;
        CollisionLayer = 1;
        //control = new Control();
        //AddChild(control);
        //control.MouseFilter = (Control.MouseFilterEnum.Pass);
        
        Name = "Plains";

        Connect("mouse_entered", this, "onMouseEntered"); 
        Connect("mouse_exited", this, "onMouseExited");
        Connect("input_event", this, "onInputEvent");
    }

    void onMouseEntered()
    {
        Modulate = new Color(0.9f,0.9f,0.9f);
    }
    
    void onMouseExited()
    {
        Modulate = new Color(1,1,1);
    }

    void onInputEvent(Godot.Object viewport, InputEvent @event, int shape_idx)
    {
        Helpers.GetStateManager().state.handleEvent(this);
    }

    // TODO do we use this?
    public void handleInput()
    {
                // if left click
        if (Input.IsMouseButtonPressed((int)ButtonList.Left))
        {
            // Select Unit if is not selected
                //sprite.Modulate = new Color(0.9f,0.9f,0.9f);
                sprite.Modulate = Colors.Green;
            
        }
    }


}