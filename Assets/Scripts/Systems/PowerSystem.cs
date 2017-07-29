using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class PowerSystem : MonoBehaviour
{

    public GameObject PowerCellPrefab;
    public float BaseSpawnChance;
    public float SpawnChanceIncrease;
    public int MaxPowerCells;
    public void SpawnPower()
    {
        var position = GetRandomFreePosition();
        GameManager.GridSystem().Grid[position].ContainsPower = true;

        var powerCellObject = Instantiate(PowerCellPrefab);
        powerCellObject.transform.position = position.ToVector2();

        GameManager.GridSystem().PowerCells.Add(position, powerCellObject.GetComponent<PowerCell>());
    }

    private int _fails;
    public void SpawnPowerChance()
    {
        if (GameManager.GridSystem().PowerCells.Count > MaxPowerCells)
            return;

        var random = Random.value;
        if  (BaseSpawnChance + (SpawnChanceIncrease * _fails) > random)
        {
            SpawnPower();
            _fails = 0;
        }
        else
        {
            _fails++;
        }
    }

    private GridPosition GetRandomFreePosition()
    {
        var possiblePositions =  GameManager.GridSystem().Grid.Where(x => x.Value.IsFree && x.Value.ContainsPower == false).Select(x => x.Key).ToList();
        var randomPosition = Mathf.CeilToInt(Random.value * possiblePositions.Count);

        return possiblePositions[randomPosition];
    }

    public void ConsumePower(GridPosition position)
    {
        var cell = GameManager.GridSystem().PowerCells[position];
        GameManager.GridSystem().PowerCells.Remove(position);
        Destroy(cell.gameObject);
    }
}
