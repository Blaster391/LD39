using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BasicEnemyScript : UnitScript
{

    public float PowerDesire = 0;

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

    private void DoAnAction()
    {
        if (WantsPower())
        {
            
        }

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

        if (TryMoveAction())
        {
            return;
        }

        Actions["Pass"].Action(null);
    }

    private bool WantsPower()
    {
        return false;
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

    private Direction DirectionToPlayer()
    {
        var player = GameManager.PlayerUnit;

        var xDif = CurrentPosition.x - player.CurrentPosition.x;
        var yDif = CurrentPosition.y - player.CurrentPosition.y;

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
