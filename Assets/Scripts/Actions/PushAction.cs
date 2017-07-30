using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PushAction : BaseAction
{

	// Use this for initialization
    public PushAction(UnitScript unit) : base(unit)
    {
    }

    public override int PowerCost()
    {
        return 3;
    }

    public override int ActionCost()
    {
        return 3;
    }

    protected override void PerformAction(ActionParameters parameters)
    {
        GameManager.UISystem().Log(Unit.Name + " Pushed " + parameters.Target.Name);
        var distanceToPush = 3; //todo stats based
        var directionToPush = DirectionToPosition(parameters.Target.CurrentPosition);
        var restingPlace = parameters.Target.CurrentPosition;
        for (int i = 0; i <= distanceToPush; i++)
        {
            var newPos = GetNewPosition(directionToPush, restingPlace);
            if (GameManager.GridSystem().IsPositionAccessible(newPos))
            {
                restingPlace = newPos;
            }
            else
            {
                parameters.Target.TakeDamage(3);
                GameManager.UISystem().Log(parameters.Target.Name + " Took 3 damage from collision!");
                break;
            }

        }
        parameters.Target.MoveAnimation(restingPlace.ToVector2());//Hacky AF
        Unit.transform.up = parameters.Target.transform.position - Unit.transform.position;
        parameters.Target.SetPosition(restingPlace);
    }

    protected override void ActionAnimation(ActionParameters parameters)
    {  }

    public override bool CanTakeAction(ActionParameters parameters)
    {
        if (parameters.Target == null)
            return false;

        if (Vector2.Distance(Unit.CurrentPosition.ToVector2(), parameters.Target.CurrentPosition.ToVector2()) > 1.9f)
            return false;

        return base.CanTakeAction(parameters);
    }

    private Direction DirectionToPosition(GridPosition position)
    {
        var xDif = Unit.CurrentPosition.x - position.x;
        var yDif = Unit.CurrentPosition.y - position.y;

        if (Mathf.Abs(xDif) > Mathf.Abs(yDif))
        {
            return xDif > 0 ? Direction.Left : Direction.Right;
        }
        return yDif > 0 ? Direction.Down : Direction.Up;
    }

    private GridPosition GetNewPosition(Direction direction, GridPosition position)
    {
        switch (direction)
        {
            case Direction.Up:
                return (new GridPosition(position.ToVector2() + Vector2.up));
            case Direction.Down:
                return (new GridPosition(position.ToVector2() + Vector2.down));
            case Direction.Left:
                return (new GridPosition(position.ToVector2() + Vector2.left));
            case Direction.Right:
                return (new GridPosition(position.ToVector2() + Vector2.right));
            default:
                throw new ArgumentOutOfRangeException("direction", direction, null);
        }
    }
}
