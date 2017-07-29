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
        return Unit.CurrentActionTokens >= ActionCost() && Unit.CurrentPower >= PowerCost();
    }

    public void Action(ActionParameters parameters)
    {
        if (!CanTakeAction(parameters))
            return;

        PerformAction(parameters);
        PostPerformAction();
    }

    protected abstract void PerformAction(ActionParameters parameters);

    public virtual void PostPerformAction()
    {
        Unit.CurrentActionTokens -= ActionCost();
        Unit.CurrentPower -= PowerCost();

        if(Unit.CurrentActionTokens <= 0)
            GameManager.TurnSystem().NextTurn();
    }
}
