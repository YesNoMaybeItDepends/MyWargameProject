using Godot;
using System;

public class CubeCoordinates
{
    public int q;
    public int r;
    public int s;

    #region Constructors
    public CubeCoordinates(int Q, int R, int S)
    {
        q = Q;
        r = R;
        s = S;
    }

    public CubeCoordinates(float Q, float R, int S)
    {
        q = (int)Q;
        r = (int)R;
        s = (int)S;
    }

    public CubeCoordinates(Vector3 v3)
    {
        q = (int)v3.x;
        r = (int)v3.y;
        s = (int)v3.z;
    }
    #endregion

    #region Conversions
    
    public static AxialCoordinates ToAxial(CubeCoordinates c)
    {
        int q = c.q;
        int r = c.r;
        return new AxialCoordinates(q,r);
    }

    #endregion

    #region Arithmetic
    
    public static CubeCoordinates Substract(CubeCoordinates a, CubeCoordinates b)
    {
        int q = a.q - b.q;
        int r = a.r - b.r;
        int s = a.s - b.s;
        return new CubeCoordinates(q,r,s);
    }

    #endregion

    #region Etc
    
    public static int Distance(CubeCoordinates a, CubeCoordinates b)
    {
        CubeCoordinates v = Substract(a,b);
        return (Math.Abs(v.q) + Math.Abs(v.r) + Math.Abs(v.s))/2;
    }

    #endregion
}