using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridController : MonoBehaviour
{
    private IGridGenerator gridGenerator;
    public GameObject blockPrefab;
    public int rows = 5; // Số hàng
    public int cols = 5; // Số cột
    public float cellSize = 1.0f; // Kích thước mỗi ô (độ rộng và cao)
    public float spacing = 0.1f; // Khoảng cách giữa các ô
    public Vector2 centerOffset = new Vector2(0, 0); // Điểm trung tâm của lưới
    public GameObject[,] grid;
    private void Awake()
    {
        gridGenerator=new GridGenerator(this.gameObject,blockPrefab);
        gridGenerator.GenerateGrid( rows,  cols,  cellSize,  spacing, centerOffset);
        grid= gridGenerator.Grid();
    }
   
}
