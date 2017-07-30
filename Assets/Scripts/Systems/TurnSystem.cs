using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using NUnit.Framework.Constraints;
using UnityEngine;

public class TurnSystem : MonoBehaviour
{

    public List<UnitScript> Units = new List<UnitScript>();
    public int CurrentActiveUnit;

    public void NextTurn()
    {
        CurrentActiveUnit++;
        if (CurrentActiveUnit >= Units.Count)
        {
            CurrentActiveUnit = 0;
        }
        GameManager.PowerSystem().SpawnPowerChance();
        Units[CurrentActiveUnit].StartTurn();
    }

    public void StartGame()
    {
        Units = Units.OrderByDescending(x => x.Speed).ToList();
        CurrentActiveUnit = 0;
        Units[CurrentActiveUnit].StartTurn();
        GameManager.PowerSystem().SpawnPower();
        GameManager.GameActive = true;
        GameManager.UISystem().Log("Match Started");
    }

    public void RegisterUnit(UnitScript unit)
    {
        Units.Add(unit);
    }

    public void UnRegisterUnit(UnitScript unit)
    {
        if(Units.Contains(unit))
            Units.Remove(unit);

        //TODO Make this less hacky
        NextTurn();
    }

    public bool IsTurn(UnitScript unit)
    {
        return Units[CurrentActiveUnit] == unit;
    }

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
