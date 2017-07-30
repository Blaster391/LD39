using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PassTurnAction : BaseAction {
    public PassTurnAction(UnitScript unit) : base(unit)
    {
    }

    public override int PowerCost()
    {
        return 0;
    }

    public override int ActionCost()
    {
        return Unit.CurrentActionTokens;
    }

    protected override void PerformAction(ActionParameters parameters)
    {
        GameManager.UISystem().Log(Unit.Name + " Passed Turn");
    }
}
