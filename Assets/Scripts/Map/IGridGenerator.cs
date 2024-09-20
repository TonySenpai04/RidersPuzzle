using UnityEngine;

public interface IGridGenerator
{
  void GenerateGrid(int rows, int cols, float cellSize, float spacing, Vector2 centerOffset);
   GameObject[,] Grid() ;
}