
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridGenerator : IGridGenerator
{
    private GameObject blockPrefab; 
    private GameObject owner;
    private GameObject[,] grid; 

    public GridGenerator(GameObject owner, GameObject cellPrefab)
    {
        this.owner = owner;
        this.blockPrefab = cellPrefab;
    }



    public void GenerateGrid(int rows, int cols, float cellSize, float spacing, Vector2 centerOffset)
    {
        grid = new GameObject[rows, cols]; 
        float gridWidth = cols * (cellSize + spacing);
        float gridHeight = rows * (cellSize + spacing);
        Vector3 startPosition = new Vector3(
            -gridWidth / 2f + centerOffset.x + (cellSize + spacing) / 2f,
            gridHeight / 2f + centerOffset.y - (cellSize + spacing) / 2f + 0.5f,
            0
        );

        for (int row = 0; row < rows; row++)
        {
            for (int col = 0; col < cols; col++)
            {
                Vector3 cellPosition = new Vector3(
                    startPosition.x + col * (cellSize + spacing),
                    startPosition.y - row * (cellSize + spacing),
                    0
                );

 
                GameObject cell = GameObject.Instantiate(blockPrefab, cellPosition, Quaternion.identity, owner.transform);
                cell.transform.localScale = new Vector3(cellSize, cellSize, 1);
                grid[row, col] = cell; 
                cell.name = "block:" + row + "-" + col;
            }
        }
    }
    public GameObject[,] Grid()
    {
        return this.grid;
    }

}

