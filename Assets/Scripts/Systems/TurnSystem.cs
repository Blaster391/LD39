﻿using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;
using Random = UnityEngine.Random;

public class TurnSystem : MonoBehaviour
{

    public List<UnitScript> Units = new List<UnitScript>();
    public int CurrentActiveUnit;
    public GameObject EnemyPrefab;

    public int CurrentRotation = 0;
    private int EnemiesSpawned = 1;
    public void NextTurn()
    {
        CurrentActiveUnit++;
        if (CurrentActiveUnit >= Units.Count)
        {
            CurrentActiveUnit = 0;
            SpawnNewEnemyChance();
            CurrentRotation++;
        }
        GameManager.PowerSystem().SpawnPowerChance();
        Units[CurrentActiveUnit].StartTurn();
    }

    public void SpawnNewEnemyChance()
    {
        if(Units.Count == 1)
            SpawnNewEnemy();

        var randomValue = Random.value;
        var chance =( 0.1f * ((float)CurrentRotation/3) )/ Units.Count;
        if(randomValue < chance)
            SpawnNewEnemy();
    }

    public void SpawnNewEnemy()
    {
        var position = GetRandomFreePosition();
        if (position == null)
            return;

        EnemiesSpawned++;
        var enemy = EnemyPrefab.GetComponent<BasicEnemyScript>();
        enemy.StartingPosition = position.ToVector2();
        enemy.CurrentPosition = position;
        enemy.Name = "Bad Guy " + EnemiesSpawned;

        //Stat generator
        enemy.HealDesire = Random.value / 2;
        enemy.PowerDesire = Random.value * 0.2f;

        enemy.Strength = 1 + Mathf.RoundToInt(Random.value * EnemiesSpawned * 0.5f);
        enemy.Capacity = 5 + Mathf.RoundToInt(Random.value * EnemiesSpawned / 2);
        enemy.Speed = 3 + Mathf.RoundToInt(Random.value * EnemiesSpawned * 0.4f);
        enemy.MaxHealth = 5 + Mathf.RoundToInt(Random.value * EnemiesSpawned * 2);
        enemy.Efficiency = 2 + Mathf.RoundToInt(Random.value * EnemiesSpawned);

        //
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
        GameManager.UISystem()._isDead = false;
        GameManager.UISystem().Log("Match Started");
    }

    public void RegisterUnit(UnitScript unit)
    {
        Units.Add(unit);
    }

    public void UnRegisterUnit(UnitScript unit)
    {
        if (Units.Contains(unit))
        {
            if (Units.IndexOf(unit) < CurrentActiveUnit)
            {
                CurrentActiveUnit -= 1;
            }
            Units.Remove(unit);

        }
    }

    public bool IsTurn(UnitScript unit)
    {
        return Units[CurrentActiveUnit] == unit;
    }
}
