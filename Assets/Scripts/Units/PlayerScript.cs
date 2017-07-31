using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Analytics;
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
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            ConsumePower();

        }
        if (Input.GetKeyDown(KeyCode.Alpha5))
        {
            PassTurn();

        }
        if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            Heal();

        }
        if (Input.GetKeyDown(KeyCode.Alpha4))
        {
            Push();

        }

        if (Input.GetKeyDown(KeyCode.Alpha3))
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
        GameManager.AnimationSystem().AudioMaster.clip = GameManager.AnimationSystem().PlayerDeathSound;
        GameManager.AnimationSystem().AudioMaster.Play();

        GameManager.GameActive = false;
        GameManager.UISystem().ShowDeathScreen();
        base.Kill();

        try
        {
            Analytics.CustomEvent("gameOver", new Dictionary<string, object>
            {
                {"Level", Score},
                {"Turn", GameManager.TurnSystem().CurrentRotation },
                {"MaxHealth", MaxHealth},
                {"Strength", Strength},
                {"Speed", Speed},
                {"Efficiency", Efficiency},
                {"Capacity", Capacity},
                {"PlayTime", Time.realtimeSinceStartup }
            });
        }
        catch (Exception ex)
        {
            
        }

    }

    public void LevelUpHealth()
    {
        MaxHealth += 5;
        Health += 3;
        GameManager.UISystem().PlayerHealthBar.SetBar(Health, MaxHealth);
        LevelUp();
    }

    public void LevelUpStrength()
    {
        Strength++;
        LevelUp();
    }

    public void LevelUpCapacity()
    {
        Capacity+=2;
        CurrentPower++;
        GameManager.UISystem().PlayerPowerBar.SetBar(CurrentPower, Capacity);
        LevelUp();
    }

    public void LevelUpSpeed()
    {
        Speed++;
        GameManager.UISystem().PlayerActionBar.SetBar(CurrentActionTokens, Speed);
        LevelUp();
    }

    public void LevelUpEfficieny()
    {
        Efficiency+=2;
        LevelUp();
    }

    public void LevelUp()
    {
        GameManager.GameActive = true;
        GameManager.UISystem().LevelUpScreen.SetActive(false);
        GameManager.UISystem().LevelingUp = false;
        AudioSource.clip = GameManager.AnimationSystem().LevelUpSound;
        AudioSource.Play();
    }
}
