using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BasicAttackAction : BaseAction
{

	// Use this for initialization
    public BasicAttackAction(UnitScript unit) : base(unit)
    {
    }
    //TODO stats
    public override int PowerCost()
    {
        return 1;
    }

    public override int ActionCost()
    {
        return 1;
    }

    public override bool CanTakeAction(ActionParameters parameters)
    {
        if (parameters.Target == null)
            return false;

        if (Vector2.Distance(Unit.CurrentPosition.ToVector2(), parameters.Target.CurrentPosition.ToVector2()) > 1.9f)
            return false;

        return base.CanTakeAction(parameters);
    }

    protected override void PerformAction(ActionParameters parameters)
    {
        parameters.Target.TakeDamage(1);
    }
}
