using Godot;
using System;
using System.Collections.Generic;

public class AxialCoordinates
{
    public int q;
    public int r;
    public int s { get { return -q - r; } }

    public static AxialCoordinates[] DirectionVectors = new AxialCoordinates[6]
    {
        new AxialCoordinates(1,0),
        // NE
        new AxialCoordinates(1,-1),
        // N
        new AxialCoordinates(0,-1),
        // NW
        new AxialCoordinates(-1,0),
        // SW
        new AxialCoordinates(-1,1),
        // S
        new AxialCoordinates(0,1)
    };

    #region Constructors
    
    // From Int
    public AxialCoordinates(int Q, int R)
    {
        q = Q;
        r = R;
    }

    // From Float
    public AxialCoordinates(float Q, float R)
    {
        q = (int)Q;
        r = (int)R;
    }

    // From Vector2
    public AxialCoordinates(Vector2 v2)
    {
        q = (int)v2.x;
        r = (int)v2.y;
    }

    // From Offset
    public AxialCoordinates(OffsetCoordinates c)
    {
        AxialCoordinates v = c.ToAxial();
        q = v.q;
        r = v.r;
    }

    // From Cube
    public AxialCoordinates(CubeCoordinates c)
    {
        q = CubeCoordinates.ToAxial(c).q;
        r = CubeCoordinates.ToAxial(c).r;
    } 
    
    #endregion

    #region Conversions

    // Static To Offset
    public static OffsetCoordinates ToOffset(AxialCoordinates c)
    {
        int x = c.q;
        int y = c.r + (c.q - (c.q & 1)) / 2;

        return new OffsetCoordinates(x,y);
    }

    // To Offset
    public OffsetCoordinates ToOffset()
    {
        int x = this.q;
        int y = this.r + (this.q - (this.q & 1)) / 2;

        return new OffsetCoordinates(x,y);
    }

    // To Cube
    public static CubeCoordinates ToCube(AxialCoordinates c)
    {
        int q = c.q;
        int r = c.r;
        int s = -c.q - c.r;
        return new CubeCoordinates(q,r,s);
    }

    // To Vector2
    public static Vector2 ToVector2(AxialCoordinates c)
    {
        float x = (float)c.q;
        float y = (float)c.r;
        return new Vector2(x, y);
    }

    #endregion

    #region Arithmetic

    public AxialCoordinates AxialAdd(AxialCoordinates a1, AxialCoordinates a2)
    {
        //return new Vector2(hex.x + vec.x, hex.y + vec.x);
        return new AxialCoordinates(a1.q + a2.q, a1.r + a2.r);
    }

    #endregion

    #region Strings
    
    public override string ToString()
    {
        return $"q: {q}, r: {r}, s: {s}";
    }

    public string ToStringOnSeparateLines()
    {
        return $"q: {q}\nr: {r}\ns: {s}";
    }

    #endregion

    #region Etc
    
    public AxialCoordinates GetDirectionVector(HexDirections direction)
    {
        return DirectionVectors[(int)direction];
    }

    public AxialCoordinates GetNeighbour(AxialCoordinates pos, HexDirections direction)
    {
        return AxialAdd(pos, GetDirectionVector(direction));
    }

    public int Distance(AxialCoordinates a, AxialCoordinates b)
    {
        CubeCoordinates ac = ToCube(a);
        CubeCoordinates bc = ToCube(b);
        return CubeCoordinates.Distance(ac,bc);
    }

    // ACTUALLY RETURNS OFFSET COORDS?
    // public List<OffsetCoordinates> CoordinatesInRange(AxialCoordinates h, int N)
    // {
    //     List<OffsetCoordinates> coordinates = new List<OffsetCoordinates>();
    //     for (int q = -N; q <= N; q++)
    //     {
    //         for (int r = Math.Max(-N, -q - N); r <= Math.Min(N, -q + N); r++)
    //         {
    //             OffsetCoordinates pos = new OffsetCoordinates(h.q + q, h.r + r);
    //             coordinates.Add(pos);
    //         }
    //     }

    //     return coordinates;
    // }
    
    public List<AxialCoordinates> CoordinatesInRange(AxialCoordinates h, int N)
    {
        List<AxialCoordinates> coordinates = new List<AxialCoordinates>();
        for (int q = -N; q <= N; q++)
        {
            for (int r = Math.Max(-N, -q - N); r <= Math.Min(N, -q + N); r++)
            {
                AxialCoordinates pos = new AxialCoordinates(h.q + q, h.r + r);
                coordinates.Add(pos);
            }
        }

        return coordinates;
    }

    #endregion
}