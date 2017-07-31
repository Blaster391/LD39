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

        Unit.AudioSource.clip = GameManager.AnimationSystem().MoveSoundSound;
        Unit.AudioSource.Play();

        Unit.SetPosition(target);
    }

    public override bool CanTakeAction(ActionParameters parameters)
    {
        var newPosition = GetNewPosition(parameters.Direction);
        return base.CanTakeAction(parameters) && (GameManager.GridSystem().IsPositionAccessible(newPosition));
    }

    protected override void ActionAnimation(ActionParameters parameters)
    {
        switch (parameters.Direction)
        {
            case Direction.Up:
                Unit.transform.rotation = Quaternion.Euler(new Vector3(0,0,0));
                break;
            case Direction.Down:
                Unit.transform.rotation = Quaternion.Euler(new Vector3(0, 0, 180));
                break;
            case Direction.Left:
                Unit.transform.rotation = Quaternion.Euler(new Vector3(0, 0, 90));
                break;
            case Direction.Right:
                Unit.transform.rotation = Quaternion.Euler(new Vector3(0, 0, 270));
                break;
            default:
                throw new ArgumentOutOfRangeException();
        }

        Unit.MoveAnimation(GetNewPosition(parameters.Direction).ToVector2());
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
