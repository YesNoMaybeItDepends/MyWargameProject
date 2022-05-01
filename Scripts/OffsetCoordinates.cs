using Godot;
using System;
using System.Collections.Generic;

public class OffsetCoordinates
{
    public int x;
    public int y;

    #region Constructors
    
    public OffsetCoordinates(int X, int Y)
    {
        x = X;
        y = Y;
    }

    public OffsetCoordinates(Vector2 v2)
    {
        x = (int)v2.x;
        y = (int)v2.y;
    }



    #endregion

    #region Strings
    
    public override string ToString()
    {
        return $"x: {x}, y: {y}";
    }

    public string ToStringOnSeparateLines()
    {
        return $"x: {x}\ny: {y}";
    }

    #endregion

    #region Conversions
    
    public AxialCoordinates ToAxial()
    {
        int q = this.x;
        int r = this.y - (this.x - (this.x & 1)) / 2;
        
        return new AxialCoordinates(q,r);
    }
    
    #endregion

    #region Etc
    
    public List<OffsetCoordinates> GetNeighbours()
    {
        List<OffsetCoordinates> neighbours = new List<OffsetCoordinates>();
        AxialCoordinates axial = this.ToAxial(); 
        
        foreach (AxialCoordinates direction in AxialCoordinates.DirectionVectors)
        {
            var neighbour = axial.AxialAdd(axial, direction);
            neighbours.Add(neighbour.ToOffset());
        }

        return neighbours;
    }

    public List<OffsetCoordinates> GetCoordinatesInRange(int N)
    {
        List<OffsetCoordinates> coordinates = new List<OffsetCoordinates>();
        AxialCoordinates h = new AxialCoordinates(this);
        for (int q = -N; q <= N; q++)
        {
            for (int r = Math.Max(-N, -q - N); r <= Math.Min(N, -q + N); r++)
            {
                AxialCoordinates pos = new AxialCoordinates(h.q + q, h.r + r);
                coordinates.Add(pos.ToOffset());
            }
        }

        return coordinates;
    }

    #endregion

}