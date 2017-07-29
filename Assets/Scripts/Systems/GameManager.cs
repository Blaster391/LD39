using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class GameManager
{

    private static TurnSystem _turnSystem;
    private static GridSystem _gridSystem;
    private static PowerSystem _powerSystem;
    private static AnimationSystem _animationSystem;

    public static GridSystem GridSystem()
    {
        return _gridSystem ?? (_gridSystem = GameObject.Find("Master").GetComponent<GridSystem>());
    }

    public static TurnSystem TurnSystem()
    {
        return _turnSystem ?? (_turnSystem = GameObject.Find("Master").GetComponent<TurnSystem>());
    }

    public static PowerSystem PowerSystem()
    {
        return _powerSystem ?? (_powerSystem = GameObject.Find("Master").GetComponent<PowerSystem>());
    }

    public static AnimationSystem AnimationSystem()
    {
        return _animationSystem ?? (_animationSystem = GameObject.Find("Master").GetComponent<AnimationSystem>());
    }

    public static PlayerScript PlayerUnit;
    public static bool GameActive = true;
}
