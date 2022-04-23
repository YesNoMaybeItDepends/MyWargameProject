using Godot;
using System;


public interface Iinput
{
    void handleInput();
}

public class Hex : Area2D, Iinput
{
    public OffsetCoordinates offsetPos;
    public AxialCoordinates axialPos;
    public int x {get{return offsetPos.x;}}
    public int y {get{return offsetPos.y;}}

    private Unit _unit;
    public Unit unit
    {
        get
        {
            return _unit;
        }
        set
        {
            // If there was no Unit
            if (_unit == null)
            {
                // Add Unit
                if (value != null)
                {
                    AddChild(value);
                    
                    _unit = value;
                    _unit.tile = this;
                }
            }
            // If there was a Unit
            else
            {
                // If it's the same Unit
                if (_unit == value)
                {
                    return;
                }
                // If it's a different Unit
                else
                {
                    // Remove current Unit
                    RemoveChild(_unit);
                    _unit.tile = null;

                    // Add new Unit
                    if (value != null)
                    {
                        AddChild(value);
                        _unit = value;
                        _unit.tile = this;
                    }
                    // Set Null
                    else
                    {
                        _unit = value;
                    }
                }
            }
        }
    }
    
    private Terrain _terrain;
    public Terrain terrain 
    {
        get
        {
            return _terrain;
        }
        set
        {
            _terrain = value;

            AddChild(value);
        }
    }

    public CollisionPolygon2D HexCollisionPolygon;
    public Vector2[] HexPolygonVector = new Vector2[8]
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

    public Hex(Vector2 Position)
    {
        offsetPos = new OffsetCoordinates(Position);
        axialPos = new AxialCoordinates(offsetPos);
        Name = "Tile";

        // Add Collision Polygon
        HexCollisionPolygon = new CollisionPolygon2D();
        HexCollisionPolygon.Polygon = HexPolygonVector;
        HexCollisionPolygon.Position = new Vector2(-36, -36); // TODO these should be 
        AddChild(HexCollisionPolygon);

        // Enable mouse interaction
        InputPickable = true;
        CollisionLayer = 1;
    }

    public void handleInput()
    {
        
    }

    public static Vector2 NW = new Vector2(-1,0);
    public static Vector2 N = new Vector2(0, -1);
    public static Vector2 NE = new Vector2(1,-1);
    public static Vector2 SW = new Vector2(-1, 1);
    public static Vector2 S = new Vector2(0,1);
    public static Vector2 SE = new Vector2(1,0);
}