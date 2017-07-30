using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class UISystem : MonoBehaviour {
    public UIBarScript PlayerHealthBar;
    public UIBarScript PlayerPowerBar;
    public UIBarScript PlayerActionBar;

    public UIBarScript TargetHealthBar;
    public UIBarScript TargetPowerBar;
    public UIBarScript TargetActionBar;

    public PlayerScript Player;

    public Button StartButton;

    public Text ConsoleText;
    public ScrollRect Console;

    public GameObject ActiveIndicator;
    public GameObject TargetIndicator;

    public GameObject GameOverScreen;

    public Text ScoreText;
    public Text TurnText;


    public Text ScoreTextLabel;
    public Text TurnTextLabel;

    public Text ScoreTextDeath;
    public Text TurnTextDeath;

    public Button AttackButton;
    public Button ConsumeButton;
    public Button PushButton;
    public Button HealingButton;

    public GameObject ActionPanel;

    void Start()
    {
        GameOverScreen.SetActive(false);

        Player = GameManager.PlayerUnit;

        PlayerHealthBar.MaximumValue = Player.Health;
        PlayerPowerBar.MaximumValue = Player.MaxPower;
        PlayerActionBar.MaximumValue = Player.TotalActionTokens;

        TargetHealthBar.MaximumValue = Player.Target.Health;
        TargetPowerBar.MaximumValue = Player.Target.MaxPower;
        TargetActionBar.MaximumValue = Player.Target.TotalActionTokens;
        ActionPanel.SetActive(false);
    }

    void Update()
    {
        if (_isDead)
            return;

        ScoreText.text = Player.Score.ToString();
        TurnText.text = GameManager.TurnSystem().CurrentRotation.ToString();

        PlayerHealthBar.SetBar(Player.Health);
        PlayerPowerBar.SetBar(Player.CurrentPower);
        PlayerActionBar.SetBar(Player.CurrentActionTokens);
            

        ActiveIndicator.transform.position =
            GameManager.TurnSystem().Units[GameManager.TurnSystem().CurrentActiveUnit].transform.position;

        if (GameManager.TurnSystem().IsTurn(Player) && !_isDead)
        {
            ActionPanel.SetActive(true);
            TargetActionBar.gameObject.SetActive(false);
            PlayerActionBar.gameObject.SetActive(true);
            TargetIndicator.SetActive(true);

            var actionParams = new ActionParameters {Target = Player.Target};
            AttackButton.gameObject.SetActive(GameManager.PlayerUnit.Actions["BasicAttack"].CanTakeAction(actionParams));
            HealingButton.gameObject.SetActive(GameManager.PlayerUnit.Actions["Heal"].CanTakeAction(actionParams));
            PushButton.gameObject.SetActive(GameManager.PlayerUnit.Actions["Push"].CanTakeAction(actionParams));
            ConsumeButton.gameObject.SetActive(GameManager.PlayerUnit.Actions["Power"].CanTakeAction(actionParams));
        }
        else
        {
            ActionPanel.SetActive(false);
            PlayerActionBar.gameObject.SetActive(false);
            TargetActionBar.gameObject.SetActive(true);
            TargetIndicator.SetActive(false);
        }
        if (Player.Target != null)
        {
            //TargetIndicator.SetActive(true);
            TargetIndicator.transform.position = Player.Target.gameObject.transform.position;
            TargetHealthBar.SetBar(Player.Target.Health);
            TargetPowerBar.SetBar(Player.Target.CurrentPower);
            TargetActionBar.SetBar(Player.Target.CurrentActionTokens);
        }
        else
        {
            TargetIndicator.SetActive(false);
            TargetHealthBar.gameObject.SetActive(false);
            TargetPowerBar.gameObject.SetActive(false);
            TargetActionBar.gameObject.SetActive(false);
        }
    }

    public void TriggerStart()
    {
        GameManager.TurnSystem().StartGame();
        Destroy(StartButton.gameObject);
    }

    public void Log(string text)
    {
        ConsoleText.text += text;
        ConsoleText.text += "\n";

        StartCoroutine(ResetScroll());
    }

    IEnumerator ResetScroll()
    {
        Canvas.ForceUpdateCanvases();

        // Wait.
        yield return null;

        Console.verticalScrollbar.value = 0f;
        Canvas.ForceUpdateCanvases();
    }

    public bool _isDead = true;
    public void ShowDeathScreen()
    {
        ScoreTextDeath.text = Player.Score.ToString();
        TurnTextDeath.text = GameManager.TurnSystem().CurrentRotation.ToString();
        GameOverScreen.SetActive(true);
        _isDead = true;

        TargetIndicator.SetActive(false);
        TargetHealthBar.gameObject.SetActive(false);
        TargetPowerBar.gameObject.SetActive(false);
        TargetActionBar.gameObject.SetActive(false);

        ScoreText.gameObject.SetActive(false);
        TurnText.gameObject.SetActive(false);
        ScoreTextLabel.gameObject.SetActive(false);
        TurnTextLabel.gameObject.SetActive(false);


        ActiveIndicator.SetActive(false);
        PlayerHealthBar.gameObject.SetActive(false);
        PlayerActionBar.gameObject.SetActive(false);
        PlayerPowerBar.gameObject.SetActive(false);
    }

    public void RestartGame()
    {
        GameManager.TurnSystem().Units = new List<UnitScript>();
        GameManager.TurnSystem().CurrentActiveUnit = 0;

        GameManager.GridSystem().Grid = new Dictionary<GridPosition, Tile>();
        GameManager.GridSystem().PowerCells = new Dictionary<GridPosition, PowerCell>();

        Destroy(GameManager.TurnSystem());
        Destroy(GameManager.GridSystem());
        Destroy(GameManager.AnimationSystem());
        Destroy(GameManager.PowerSystem());
        GameManager.Reset();
        Destroy(GameObject.Find("Master"));
        SceneManager.LoadScene(0);
        Destroy(this);
    }

    public void Quit()
    {
        Application.Quit();
    }
}
