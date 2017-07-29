using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class UnitScript : MonoBehaviour
{
    //###STATS###
    public int Health;
    public int Speed;

    public int TotalActionTokens;
    public int CurrentActionTokens;
    public int CurrentPower;


    public Vector2 StartingPosition;
    public GridPosition CurrentPosition;

    public Dictionary<string, BaseAction> Actions = new Dictionary<string, BaseAction>();

    // Use this for initialization
    public void StartBase ()
    {
        GameManager.TurnSystem().RegisterUnit(this);
        CurrentPosition = new GridPosition(StartingPosition);
        SetPosition(CurrentPosition);
    }

    public void StartTurn()
    {
        CurrentActionTokens = TotalActionTokens;

    }

    public bool IsTurn()
    {
        return GameManager.TurnSystem().IsTurn(this);
    }

    public void SetPosition(GridPosition target)
    {
        GameManager.GridSystem().FreePosition(CurrentPosition);
        GameManager.GridSystem().FillPosition(target);
        CurrentPosition = target;
        gameObject.transform.position = CurrentPosition.ToVector2();
    }

    public void LoadBaseActions()
    {
        Actions.Add("Move", new MovementAction(this));
        Actions.Add("Pass", new PassTurnAction(this));
        Actions.Add("BasicAttack", new BasicAttackAction(this));
    }

    public void TakeDamage(int damage)
    {
        Health -= damage;
        if (Health <= 0)
        {
            Kill();
        }
    }

    public void Kill()
    {
        GameManager.TurnSystem().UnRegisterUnit(this);
        Destroy(gameObject);
    }
}
