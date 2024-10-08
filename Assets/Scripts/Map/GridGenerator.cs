
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridGenerator : IGridGenerator
{
    private GameObject blockPrefab; // Prefab cho mỗi ô
    private GameObject owner;
    //public int rows = 5; // Số hàng
    //public int cols = 5; // Số cột
    //public float cellSize = 1.0f; // Kích thước mỗi ô (độ rộng và cao)
    //public float spacing = 0.1f; // Khoảng cách giữa các ô
   // public Vector2 centerOffset = new Vector2(0, 0); // Điểm trung tâm của lưới

     private GameObject[,] grid; // Mảng lưu trữ các ô

    public GridGenerator (GameObject owner, GameObject cellPrefab)
    {
        this.owner = owner;
        this.blockPrefab = cellPrefab;
    }

    //void Awake()
    //{
    //    GenerateGrid();
    //}

    public void GenerateGrid(int rows, int cols,float cellSize, float spacing, Vector2 centerOffset)
    {
        grid = new GameObject[rows, cols]; // Khởi tạo lưới

        // Tính toán tổng kích thước của lưới (chiều rộng và chiều cao)
        float gridWidth = cols * (cellSize + spacing);
        float gridHeight = rows * (cellSize + spacing);

        // Tính toán điểm bắt đầu để tạo ô xung quanh tâm
        Vector3 startPosition = new Vector3(
            -gridWidth / 2f + centerOffset.x + (cellSize + spacing) / 2f,
            gridHeight / 2f + centerOffset.y - (cellSize + spacing) / 2f+0.5f,
            0
        );

        for (int row = 0; row < rows; row++)
        {
            for (int col = 0; col < cols; col++)
            {
                // Tính toán vị trí của từng ô xung quanh tâm
                Vector3 cellPosition = new Vector3(
                    startPosition.x + col * (cellSize + spacing),
                    startPosition.y - row * (cellSize + spacing),
                    0
                );

                // Tạo ô vuông từ Prefab tại vị trí đã tính toán
                GameObject cell = GameObject.Instantiate(blockPrefab, cellPosition, Quaternion.identity, owner.transform);
                
                grid[row, col] = cell; // Lưu trữ ô vào mảng grid
                cell.name = "block:" + row + "-" + col;
            }
        }
    }
    public GameObject[,] Grid()
    {
        return this.grid;
    }
    
}
