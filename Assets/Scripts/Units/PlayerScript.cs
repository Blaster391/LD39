using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerScript : UnitScript
{

    // Use this for initialization
    void Start ()
	{
	    StartBase();
	    LoadBaseActions();
	    GameManager.PlayerUnit = this;


    }

    public UnitScript Target;

	// Update is called once per frame
	void Update () {


        if (!IsTurn() && GameManager.GameActive) return;

	    if (Input.GetKeyDown(KeyCode.W))
	    {
            CallMovementAction(Direction.Up);
        }
	    if (Input.GetKeyDown(KeyCode.S))
        {
            CallMovementAction(Direction.Down);
        }
	    if (Input.GetKeyDown(KeyCode.A))
	    {
            CallMovementAction(Direction.Left);
        }
	    if (Input.GetKeyDown(KeyCode.D))
	    {
            CallMovementAction(Direction.Right);
        }
        if (Input.GetKeyDown(KeyCode.E))
        {
            Actions["Power"].Action(null);
        }
        if (Input.GetKeyDown(KeyCode.Q))
        {
            Actions["Pass"].Action(null);
        }

        if (Input.GetKeyDown(KeyCode.Space))
	    {
            var parameters = new ActionParameters { Target = this.Target};
            Actions["BasicAttack"].Action(parameters);
        }
	}

    private void CallMovementAction(Direction direction)
    {
        var parameters = new ActionParameters {Direction = direction};
        Actions["Move"].Action(parameters);
    }


}
