using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ConsumePowerAction : BaseAction {

	// Use this for initialization
    public ConsumePowerAction(UnitScript unit) : base(unit)
    {
    }

    public override int PowerCost()
    {
        return 0;
    }

    public override int ActionCost()
    {
        return 3;
    }

    protected override void PerformAction(ActionParameters parameters)
    {
        Unit.CurrentPower += GameManager.GridSystem().PowerCells[Unit.CurrentPosition].Power;
        if (Unit.CurrentPower > Unit.MaxPower)
            Unit.CurrentPower = Unit.MaxPower;

        GameManager.PowerSystem().ConsumePower(Unit.CurrentPosition);
        GameManager.UISystem().Log(Unit.Name + " Consumed Power");
    }

    public override bool CanTakeAction(ActionParameters parameters)
    {
        if (Unit.CurrentPower == Unit.MaxPower) return false;
        if (!GameManager.GridSystem().PowerCells.ContainsKey(Unit.CurrentPosition)) return false;
        return base.CanTakeAction(parameters);
    }
}
