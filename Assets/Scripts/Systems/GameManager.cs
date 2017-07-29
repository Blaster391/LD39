using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class GameManager
{

    private static TurnSystem _turnSystem;
    private static GridSystem _gridSystem;

    public static GridSystem GridSystem()
    {
        return _gridSystem ?? (_gridSystem = GameObject.Find("Master").GetComponent<GridSystem>());
    }

    public static TurnSystem TurnSystem()
    {
        return _turnSystem ?? (_turnSystem = GameObject.Find("Master").GetComponent<TurnSystem>());
    }

    public static bool GameActive = true;
}
