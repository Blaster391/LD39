using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerScript : UnitScript
{

    public int Score;

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
            ConsumePower();

        }
        if (Input.GetKeyDown(KeyCode.Q))
        {
            PassTurn();

        }
        if (Input.GetKeyDown(KeyCode.F))
        {
            Heal();

        }
        if (Input.GetKeyDown(KeyCode.Z))
        {
            Push();

        }

        if (Input.GetKeyDown(KeyCode.Space))
        {
            Attack();
        }

        if (Input.GetKeyDown(KeyCode.Return))
        {
            SceneManager.LoadScene(0);
        }

        if (Input.GetKeyDown(KeyCode.Escape))
        {
            Application.Quit();
        }
    }

    public void PassTurn()
    {
        Actions["Pass"].Action(null);
    }

    public void ConsumePower()
    {
        Actions["Power"].Action(null);
    }

    public void Attack()
    {
        var parameters = new ActionParameters { Target = this.Target };
        Actions["BasicAttack"].Action(parameters);
    }

    public void Push()
    {
        var parameters = new ActionParameters { Target = this.Target };
        Actions["Push"].Action(parameters);
    }

    public void Heal()
    {
        Actions["Heal"].Action(null);
    }

    private void CallMovementAction(Direction direction)
    {
        var parameters = new ActionParameters {Direction = direction};
        Actions["Move"].Action(parameters);
    }

    public override void Kill()
    {
        GameManager.GameActive = false;
        GameManager.UISystem().ShowDeathScreen();
        base.Kill();
    }
}
