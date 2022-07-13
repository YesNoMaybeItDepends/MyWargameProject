using Godot;
using System;
using System.Collections.Generic;

public class Hex : InteractiveEntity
{
    public Map board;
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

    public Hex(Vector2 Position, Map Board)
    {
        board = Board;

        offsetPos = new OffsetCoordinates(Position);
        axialPos = new AxialCoordinates(offsetPos);
        
        Name = "Tile "+offsetPos.x+", "+offsetPos.y;
    }

    public List<Hex> GetNeighbours()
    {
        List<Hex> neighbours = new List<Hex>();

        var neighbouringCoordinates = offsetPos.GetNeighbours();
        foreach (OffsetCoordinates coords in neighbouringCoordinates)
        {
            Hex hex = board.getHexAt(coords);
            if (hex != null)
            {
                neighbours.Add(hex);
            }
        }

        return neighbours;
    }

    // public static Vector2 NW = new Vector2(-1,0);
    // public static Vector2 N = new Vector2(0, -1);
    // public static Vector2 NE = new Vector2(1,-1);
    // public static Vector2 SW = new Vector2(-1, 1);
    // public static Vector2 S = new Vector2(0,1);
    // public static Vector2 SE = new Vector2(1,0);

    public override void onInputEvent(Godot.Object viewport, InputEvent @event, int shape_idx)
    {
        ServiceProvider.GetService<Inputcontroller>().handleEvent(this);
    }
}