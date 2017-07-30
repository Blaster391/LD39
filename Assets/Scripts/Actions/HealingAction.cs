using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealingAction : BaseAction {

	// Use this for initialization
    public HealingAction(UnitScript unit) : base(unit)
    {
    }

    public override int PowerCost()
    {
        return 1;
    }

    public override int ActionCost()
    {
        return 2;
    }

    protected override void PerformAction(ActionParameters parameters)
    {
        Unit.Health += Unit.Efficiency;
        if (Unit.Health > Unit.MaxHealth)
        {
            Unit.Health = Unit.MaxHealth;
        }
        GameManager.UISystem().Log(Unit.Name + " Healed for " + Unit.Efficiency + " HP!");
    }

    public override bool CanTakeAction(ActionParameters parameters)
    {
        if (Unit.Health == Unit.MaxHealth) return false;
        return base.CanTakeAction(parameters);
    }
    protected override void ActionAnimation(ActionParameters parameters)
    {
        Unit.HealAnimation();
    }
}
