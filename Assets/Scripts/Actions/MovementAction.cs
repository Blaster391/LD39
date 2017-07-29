using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovementAction : BaseAction {
    public MovementAction(UnitScript unit) : base(unit)
    {
    }

    protected override void PerformAction(ActionParameters parameters)
    {
        Move(GetNewPosition(parameters.Direction));
    }

    private GridPosition GetNewPosition(Direction direction)
    {
        switch (direction)
        {
            case Direction.Up:
                return (new GridPosition(Unit.CurrentPosition.ToVector2() + Vector2.up));
            case Direction.Down:
                return (new GridPosition(Unit.CurrentPosition.ToVector2() + Vector2.down));
            case Direction.Left:
                return (new GridPosition(Unit.CurrentPosition.ToVector2() + Vector2.left));
            case Direction.Right:
                return (new GridPosition(Unit.CurrentPosition.ToVector2() + Vector2.right));
            default:
                throw new ArgumentOutOfRangeException("direction", direction, null);
        }
    }

    private void Move(GridPosition target)
    {
        if (!GameManager.GridSystem().IsPositionAccessible(target)) return;

        Unit.SetPosition(target);
    }

    public bool CanTakeAction(ActionParameters parameters)
    {
        var newPosition = GetNewPosition(parameters.Direction);
        return base.CanTakeAction(parameters) && (GameManager.GridSystem().IsPositionAccessible(newPosition));
    }

    public override int PowerCost()
    {
        return 0;
    }

    public override int ActionCost()
    {
        return 1;
    }
}
