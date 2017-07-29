using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class BaseAction
{
    public ActionType ActionType;
    public UnitScript Unit;
    public abstract int PowerCost();
    public abstract int ActionCost();

    protected BaseAction(UnitScript unit)
    {
        Unit = unit;
    }

    public virtual bool CanTakeAction(ActionParameters parameters)
    {
        if (!GameManager.GameActive)
            return false;

        return Unit.CurrentActionTokens >= ActionCost() && Unit.CurrentPower >= PowerCost();
    }

    public void Action(ActionParameters parameters)
    {
        if (!CanTakeAction(parameters))
            return;

        ActionAnimation(parameters);
        PerformAction(parameters);
        PostPerformAction();
    }

    protected abstract void PerformAction(ActionParameters parameters);

    protected virtual void ActionAnimation(ActionParameters parameters)
    {
        Unit.PassTurnAnimation();
    }
    public virtual void PostPerformAction()
    {
        Unit.CurrentActionTokens -= ActionCost();
        Unit.CurrentPower -= PowerCost();

        if(Unit.CurrentActionTokens <= 0)
            GameManager.TurnSystem().NextTurn();
    }
}
