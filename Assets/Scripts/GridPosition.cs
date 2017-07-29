using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridPosition
{

    public readonly int x;
    public readonly int y;

    public GridPosition(int x, int y)
    {
        this.x = x;
        this.y = y;

    }
    public GridPosition(Vector2 vector)
    {
        x = Mathf.RoundToInt(vector.x);
        y = Mathf.RoundToInt(vector.y);
    }

    public override bool Equals(object other)
    {
        if (other == null) return false;

        if (other.GetType() != typeof(GridPosition)) return false;

        return Equals((GridPosition)other);
    }

    protected bool Equals(GridPosition other)
    {
        return x == other.x && y == other.y;
    }

    public override int GetHashCode()
    {
        unchecked
        {
            return (x * 397) ^ y;
        }
    }

    public override string ToString()
    {
        return ("x:" + x + " y:" + y);
    }

    public Vector2 ToVector2()
    {
        return new Vector2(x,y);
    }
}
