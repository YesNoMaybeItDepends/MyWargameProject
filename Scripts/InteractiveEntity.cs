using Godot;
using System;

public class InteractiveEntity : Area2D
{
    public Sprite sprite;
    public CollisionPolygon2D collPolygon;
    
    // TODO move polygons to their own class
    #region polygons
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
    #endregion

    private bool _inputEnabled;
    public bool inputEnabled 
    {
        get
        {
            return _inputEnabled;
        }
        set
        {
            // Enable Input
            if (value is true)
            {
                InputPickable = true;
                Connect("mouse_entered", this, "onMouseEntered"); 
                Connect("mouse_exited", this, "onMouseExited");
                Connect("input_event", this, "onInputEvent");                
            }
            // Disable Input
            else
            {
                InputPickable = false;
                Disconnect("mouse_entered", this, "onMouseEntered");
                Disconnect("mouse_exited", this, "onMouseExited");
                Disconnect("input_event", this, "onInputEvent");
            }
            
            _inputEnabled = value;
        }
    }

    public bool mouseOver = false;

    public InteractiveEntity()
    {
        sprite = new Sprite();
        AddChild(sprite);
        
        collPolygon = new CollisionPolygon2D();
        collPolygon.Polygon = HexPolygon;
        collPolygon.Position = new Vector2(-36, -36);
        AddChild(collPolygon);

        inputEnabled = true;
    }



    public void onMouseEntered()
    {
        mouseOver = true;
        handleOnMouseEntered();
    }

    public virtual void handleOnMouseEntered()
    {

    }

    public void onMouseExited()
    {
        mouseOver = false;
        handleOnMouseExited();
    }

    public virtual void handleOnMouseExited()
    {

    }

    public virtual void onInputEvent(Godot.Object viewport, InputEvent @event, int shape_idx)
    {
        
    }
}