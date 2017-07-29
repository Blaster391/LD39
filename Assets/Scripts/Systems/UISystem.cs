using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UISystem : MonoBehaviour {
    public UIBarScript PlayerHealthBar;
    public UIBarScript PlayerPowerBar;
    public UIBarScript PlayerActionBar;

    public UIBarScript TargetHealthBar;
    public UIBarScript TargetPowerBar;
    public UIBarScript TargetActionBar;

    public PlayerScript Player;

    void Start()
    {
        PlayerHealthBar.MaximumValue = Player.Health;
        PlayerPowerBar.MaximumValue = Player.MaxPower;
        PlayerActionBar.MaximumValue = Player.TotalActionTokens;

        TargetHealthBar.MaximumValue = Player.Target.Health;
        TargetPowerBar.MaximumValue = Player.Target.MaxPower;
        TargetActionBar.MaximumValue = Player.Target.TotalActionTokens;
    }

    void Update()
    {
        PlayerHealthBar.SetBar(Player.Health);
        PlayerPowerBar.SetBar(Player.CurrentPower);
        PlayerActionBar.SetBar(Player.CurrentActionTokens);

        TargetHealthBar.SetBar(Player.Target.Health);
        TargetPowerBar.SetBar(Player.Target.CurrentPower);
        TargetActionBar.SetBar(Player.Target.CurrentActionTokens);

        if (GameManager.TurnSystem().IsTurn(Player))
        {
            TargetActionBar.gameObject.SetActive(false);
            PlayerActionBar.gameObject.SetActive(true);
        }
        else
        {
            PlayerActionBar.gameObject.SetActive(false);
            TargetActionBar.gameObject.SetActive(true);
        }
    }
}
