using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum ActionType {
    Movement,
    Targetted
}

public enum Direction
{
    Up,
    Down,
    Left,
    Right
}

public static class DirectionExtensions
{
    public static Direction Reverse(this Direction direction)
    {
        switch (direction)
        {
            case Direction.Up:
                return Direction.Down;
            case Direction.Down:
                return Direction.Up;
            case Direction.Left:
                return Direction.Right;
            case Direction.Right:
                return Direction.Left;
            default:
                throw new ArgumentOutOfRangeException("direction", direction, null);
        }
    }
}

