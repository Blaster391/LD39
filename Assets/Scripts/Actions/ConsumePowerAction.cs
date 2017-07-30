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
        Unit.CurrentPower += GameManager.GridSystem().PowerCells[Unit.CurrentPosition].Power * Unit.Efficiency;
        if (Unit.CurrentPower > Unit.Capacity)
            Unit.CurrentPower = Unit.Capacity;

        GameManager.PowerSystem().ConsumePower(Unit.CurrentPosition);
        GameManager.UISystem().Log(Unit.Name + " Consumed Power");
    }

    public override bool CanTakeAction(ActionParameters parameters)
    {
        if (Unit.CurrentPower == Unit.Capacity) return false;
        if (!GameManager.GridSystem().PowerCells.ContainsKey(Unit.CurrentPosition)) return false;
        return base.CanTakeAction(parameters);
    }

    protected override void ActionAnimation(ActionParameters parameters)
    {
        Unit.ConsumePowerAnimation(Unit.transform.position);
    }
}
