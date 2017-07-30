using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using NUnit.Framework.Constraints;
using UnityEngine;
using Random = UnityEngine.Random;

public class TurnSystem : MonoBehaviour
{

    public List<UnitScript> Units = new List<UnitScript>();
    public int CurrentActiveUnit;
    public GameObject EnemyPrefab;

    private int _currentRotation = 0;
    public void NextTurn()
    {
        CurrentActiveUnit++;
        if (CurrentActiveUnit >= Units.Count)
        {
            CurrentActiveUnit = 0;
            SpawnNewEnemyChance();
            _currentRotation++;
        }
        GameManager.PowerSystem().SpawnPowerChance();
        Units[CurrentActiveUnit].StartTurn();
    }

    public void SpawnNewEnemyChance()
    {
        if(Units.Count == 1)
            SpawnNewEnemy();

        var randomValue = Random.value;
        var chance =( 0.1f * ((float)_currentRotation/3) )/ Units.Count;
        if(randomValue < chance)
            SpawnNewEnemy();
    }

    public void SpawnNewEnemy()
    {
        var position = GetRandomFreePosition();
        if (position == null)
            return;

        var enemy = EnemyPrefab.GetComponent<BasicEnemyScript>();
        enemy.StartingPosition = position.ToVector2();
        enemy.CurrentPosition = position;
        Instantiate(EnemyPrefab);
    }

    private GridPosition GetRandomFreePosition()
    {
        var possiblePositions = GameManager.GridSystem().Grid.Where(x => x.Value.IsFree).Select(x => x.Key).ToList();
        if (possiblePositions.Count == 0)
            return null;

        var randomPosition = Mathf.FloorToInt(Random.value * possiblePositions.Count);

        return possiblePositions[randomPosition];
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
