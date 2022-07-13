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

    public CubeCoordinates(AxialCoordinates a)
    {
        
        q = a.q;
        r = a.r;
        s = a.s;
    }
    #endregion

    #region Conversions
    
    public AxialCoordinates ToAxial()
    {
        return new AxialCoordinates(q,r);
    }

    public OffsetCoordinates ToOffset()
    {
        int x = q;
        int y = r + (q - (q & 1)) / 2;

        return new OffsetCoordinates(x,y);
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

    public CubeCoordinates Add(CubeCoordinates b)
    {
        return new CubeCoordinates(q+b.q, r+b.r, s+b.s);
    }

    #endregion

    #region Etc
    
    public static int DistanceTo(CubeCoordinates a, CubeCoordinates b)
    {
        CubeCoordinates v = Substract(a,b);
        return (Math.Abs(v.q) + Math.Abs(v.r) + Math.Abs(v.s))/2;
    }

    public CubeCoordinates rotateVectorLeft(CubeCoordinates center)
    {
        var vec = CubeCoordinates.Substract(this,center);
        var rotatedVec = vec.rotateLeft();
        var position = rotatedVec.Add(center);
        return position;
    }

    public CubeCoordinates rotateVectorRight(CubeCoordinates center)
    {
        var vec = CubeCoordinates.Substract(this,center);
        var rotatedVec = vec.rotateRight();
        var position = rotatedVec.Add(center);
        return position;
    }

    private CubeCoordinates rotateRight()
    {
        return new CubeCoordinates(-s,-q,-r);
    }

    private CubeCoordinates rotateLeft()
    {
        return new CubeCoordinates(-r,-s,-q);
    }



    #endregion
}