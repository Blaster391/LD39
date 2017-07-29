using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BasicEnemyScript : UnitScript {

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
        ActionParameters tryMoveUpParameters = new ActionParameters {Direction = Direction.Up};
        if ((Actions["Move"]).CanTakeAction(tryMoveUpParameters))
        {
            Actions["Move"].Action(tryMoveUpParameters);
        }
        else
        {
            Actions["Pass"].Action(null);
        }
    }
}
