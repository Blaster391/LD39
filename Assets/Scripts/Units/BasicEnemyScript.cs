using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class BasicEnemyScript : UnitScript
{

    public float PowerDesire = 0;

    public override void StartTurn()
    {
        base.StartTurn();
        GameManager.PlayerUnit.Target = this;
    }


    void Start()
    {
        StartBase();
        LoadBaseActions();
    }

	// Update is called once per frame
	void Update () {
	    if (IsTurn() && GameManager.GameActive)
	    {
	        DoAnAction();
	    }
	}

    void OnMouseDown()
    {
        GameManager.PlayerUnit.Target = this;
    }

    private GridPosition _targetPower;
    private void DoAnAction()
    {
        if (WantsPower())
        {
            if (TryGetPower())
            {
                return;
            }

            var directionToPower = DirectionToPosition(_targetPower);

            if (TryMoveAroundObstacle(directionToPower))
            {
                return;
            }

        }
        _targetPower = null;

        if (CurrentPower == 0)
        {
            if (TryFlee())
            {
                return;
            }
        }

        if (DistanceFromPlayer() <= 1.9f)
        {
            if (TryBasicAttack())
            {
                return;
            }
        }

        if (TryHeal())
        {
            return;
        }

        if (TryMoveAction())
        {
            return;
        }

        Actions["Pass"].Action(null);
    }

    private bool TryHeal()
    {
        if ((Actions["Heal"]).CanTakeAction(null))
        {
            Actions["Heal"].Action(null);
            return true;
        }
        return false;
    }

    private bool WantsPower()
    {
        if (_targetPower != null && _targetPower.Equals(CurrentPosition))
            return true;

        if (_targetPower != null)
        {
            _targetPower = SelectClosestPower();
            return _targetPower != null;
        }
           

        //if (_targetPower != null && GameManager.GridSystem().PowerCells.ContainsKey(_targetPower))
        //    return true;

        //_targetPower = null;

        if (GameManager.GridSystem().PowerCells.Count == 0)
            return false;

        if (CurrentPower == MaxPower)
            return false;

        var power = (float)CurrentPower/MaxPower;
        var uncertainty = (Random.value - 0.5f) * 0.1f;

        if ((CurrentPower <= 0) ||PowerDesire - power + uncertainty > 0)
        {
            _targetPower = SelectClosestPower();
        }

        return _targetPower != null;
    }

    public override void Kill()
    {
        //LEVEL UP

        throw new NotImplementedException();
        base.Kill();
    }


    private GridPosition SelectClosestPower()
    {
        GridPosition selectedCell = null;
        foreach (var cell in GameManager.GridSystem().PowerCells.Keys)
        {
            if (selectedCell == null)
            {
                if (GameManager.GridSystem().IsPositionAccessible(cell))
                    selectedCell = cell;
            }
            else
            {
                if(Vector2.Distance(CurrentPosition.ToVector2(),cell.ToVector2()) < Vector2.Distance(CurrentPosition.ToVector2(), selectedCell.ToVector2()))
                {
                    if (GameManager.GridSystem().IsPositionAccessible(cell))
                        selectedCell = cell;
                }
            }
        }

        return selectedCell;
    }

    private bool TryMoveAction()
    {
        ActionParameters tryMoveUpParameters = new ActionParameters { Direction = DirectionToPlayer() };
        if ((Actions["Move"]).CanTakeAction(tryMoveUpParameters))
        {
            Actions["Move"].Action(tryMoveUpParameters);
            return true;
        }
        return false;
    }

    private bool TryMoveAroundObstacle(Direction direction)
    {
        var directionToPlayer = direction;

        if (directionToPlayer != Direction.Up && directionToPlayer != Direction.Down)
        {
            ActionParameters tryMoveParameters = new ActionParameters { Direction = Direction.Up };
            if ((Actions["Move"]).CanTakeAction(tryMoveParameters))
            {
                Actions["Move"].Action(tryMoveParameters);
                return true;
            }
            tryMoveParameters = new ActionParameters { Direction = Direction.Down };
            if ((Actions["Move"]).CanTakeAction(tryMoveParameters))
            {
                Actions["Move"].Action(tryMoveParameters);
                return true;
            }
        }
        else
        {
            ActionParameters tryMoveParameters = new ActionParameters { Direction = Direction.Left };
            if ((Actions["Move"]).CanTakeAction(tryMoveParameters))
            {
                Actions["Move"].Action(tryMoveParameters);
                return true;
            }
            tryMoveParameters = new ActionParameters { Direction = Direction.Right };
            if ((Actions["Move"]).CanTakeAction(tryMoveParameters))
            {
                Actions["Move"].Action(tryMoveParameters);
                return true;
            }
        }

        return false;
    }

    private bool TryBasicAttack()
    {
        ActionParameters tryMoveUpParameters = new ActionParameters { Target = GameManager.PlayerUnit };
        if ((Actions["BasicAttack"]).CanTakeAction(tryMoveUpParameters))
        {
            Actions["BasicAttack"].Action(tryMoveUpParameters);
            return true;
        }
        return false;
    }

    private bool TryGetPower()
    {
        if (CurrentPosition.Equals(_targetPower))
        {
            if ((Actions["Power"]).CanTakeAction(null))
            {
                Actions["Power"].Action(null);
                _targetPower = null;
                return true;
            }
            else
            {
                //TODO something else?
                Actions["Pass"].Action(null);
                return true;
            }
        }

        ActionParameters tryMoveUpParameters = new ActionParameters { Direction = DirectionToPower() };
        if ((Actions["Move"]).CanTakeAction(tryMoveUpParameters))
        {
            Actions["Move"].Action(tryMoveUpParameters);
            return true;
        }
        return false;
    }

    private Direction DirectionToPlayer()
    {
        return DirectionToPosition(GameManager.PlayerUnit.CurrentPosition);
    }

    private Direction DirectionToPower()
    {
        return DirectionToPosition(_targetPower);
    }

    private Direction DirectionToPosition(GridPosition position)
    {
        if(position == null) //TODO Debug this
            return Direction.Up;

        var xDif = CurrentPosition.x - position.x;
        var yDif = CurrentPosition.y - position.y;

        if (Mathf.Abs(xDif) > Mathf.Abs(yDif))
        {
            return xDif > 0 ? Direction.Left : Direction.Right;
        }
        return yDif > 0 ? Direction.Down : Direction.Up;
    }

    private float DistanceFromPlayer()
    {
        var player = GameManager.PlayerUnit;
        return Vector2.Distance(player.CurrentPosition.ToVector2(), CurrentPosition.ToVector2());
    }

    private bool TryFlee()
    {
        ActionParameters tryMoveUpParameters = new ActionParameters { Direction = DirectionToPlayer().Reverse() };
        if ((Actions["Move"]).CanTakeAction(tryMoveUpParameters))
        {
            Actions["Move"].Action(tryMoveUpParameters);
            return true;
        }
        return false;
    }
}
