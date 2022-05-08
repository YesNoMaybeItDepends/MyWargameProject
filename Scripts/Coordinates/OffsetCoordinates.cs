using Godot;
using System;
using System.Collections.Generic;

public class OffsetCoordinates
{
    /// <summary> Columns </summary>
    public int x;
    /// <summary> Rows </summary>
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

    public OffsetCoordinates(AxialCoordinates a)
    {
        x = a.q;
        y = a.r + (a.q - (a.q & 1)) / 2;
    }

    public OffsetCoordinates(CubeCoordinates c)
    {
        x = c.q;
        y = c.r + (c.q - (c.q & 1)) / 2;
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
    
    public CubeCoordinates ToCube()
    {
        int q = x;
        int r = y - (x - (x & 1)) / 2;
        int s = -q-r;

        return new CubeCoordinates(q,r,s);
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

    public OffsetCoordinates rotateVectorLeft(OffsetCoordinates Center)
    {
        CubeCoordinates hex = this.ToCube();
        CubeCoordinates center = Center.ToCube();
        CubeCoordinates cubeRotated = hex.rotateVectorLeft(center);

        return new OffsetCoordinates(cubeRotated);
    }

    public OffsetCoordinates rotateVectorRight(OffsetCoordinates Center)
    {
        CubeCoordinates hex = this.ToCube();
        CubeCoordinates center = Center.ToCube();
        CubeCoordinates cubeRotated = hex.rotateVectorRight(center);
        
        return new OffsetCoordinates(cubeRotated);
    }

    #endregion

}