using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridSystem : MonoBehaviour
{

    public Dictionary<GridPosition, Tile> Grid;
    public Dictionary<GridPosition, PowerCell> PowerCells;
    public Vector2 Size;
    public List<GameObject> TilePrefabs;
	// Use this for initialization
	void Start () {

        Grid = new Dictionary<GridPosition, Tile>();
        PowerCells = new Dictionary<GridPosition, PowerCell>();
        for (int i = 0; i < Size.x; i++)
	    {
            for (int j = 0; j < Size.y; j++)
            {
                AddTile(new GridPosition(i,j));
            }
        }
	}

    void AddTile(GridPosition position)
    {
        Tile tile = new Tile { IsFree = true };
        Grid.Add(position, tile);

        var selectedTile = Mathf.FloorToInt(Random.value * TilePrefabs.Count);
        var tileObject = GameObject.Instantiate(TilePrefabs[selectedTile]);
        tileObject.transform.position = position.ToVector2();
    }
	
	// Update is called once per frame
	void Update () {
		
	}

    public bool IsPositionAccessible(GridPosition position)
    {
        if (!Grid.ContainsKey(position))
            return false;

        return Grid[position].IsFree;
    }

    public void FreePosition(GridPosition position)
    {
        Grid[position].IsFree = true;
    }

    public void FillPosition(GridPosition position)
    {
        Grid[position].IsFree = false;
    }
}
