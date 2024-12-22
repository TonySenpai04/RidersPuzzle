using System;
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
    [SerializeField] private MovementController character;
    public bool isActiveObject=true;
    public int currentObjectRow;
    public int currentObjectColumn;
    [SerializeField] private List<BoxCollider2D> cellColliders = new List<BoxCollider2D>();
    private List<SpriteRenderer> spriteRenderers = new List<SpriteRenderer>();
    private List<Sprite> originalSprites = new List<Sprite>();
    [SerializeField] private Sprite highlightSprites;

    private void Awake()
    {
        gridGenerator=new GridGenerator(this.gameObject,blockPrefab);
        gridGenerator.GenerateGrid( rows,  cols,  cellSize,  spacing, centerOffset);
        grid= gridGenerator.Grid();
        GetCollider();

    }
    public void GetSprite()
    {
        spriteRenderers.Clear();
        originalSprites.Clear();
        foreach (var cell in grid)
        {
            SpriteRenderer spriteRenderer = cell.GetComponent<SpriteRenderer>();
            if (spriteRenderer != null)
            {
                spriteRenderers.Add(spriteRenderer);
                originalSprites.Add(spriteRenderer.sprite);
            }
        }
    }
    private void Update()
    {
        HighlightMovableCells();
        ActiveHiddenObject();

    }
    public void GetCollider()
    {
        cellColliders.Clear();
        foreach (var cell in grid)
        {
            var collider = cell.GetComponent<BoxCollider2D>();
            if (collider != null)
            {
                cellColliders.Add(collider);
            }
        }
    }
    public void ClearCollider()
    {
        foreach(var collider in cellColliders)
        {
            collider.enabled = true;
        }
    }
    void HighlightMovableCells()
    {
        int currentRow = character.GetPos().Item1;
        int currentCol = character.GetPos().Item2;
        for (int i = 0; i < spriteRenderers.Count; i++)
        {
            spriteRenderers[i].sprite = originalSprites[i]; 
        }

        HighlightCell(currentRow - 1, currentCol); // Ô trên
        HighlightCell(currentRow + 1, currentCol); // Ô dưới
        HighlightCell(currentRow, currentCol - 1); // Ô trái
        HighlightCell(currentRow, currentCol + 1); // Ô phải
       
      
    }
    public void ActiveHiddenObject()
    {
        int currentRow = character.GetPos().Item1;
        int currentCol = character.GetPos().Item2;

        GameObject hiddenObject = LevelManager.instance.CheckForHiddenObject(currentRow, currentCol);
        if (hiddenObject != null  && isActiveObject)
        {
            hiddenObject.GetComponent<HiddenObject>().ActiveSkill();
            currentObjectRow = character.GetPos().Item1;
            currentObjectColumn = character.GetPos().Item2;
            isActiveObject = false;
        }

        if (currentObjectRow != currentRow || currentObjectColumn!= currentCol)
        {
            isActiveObject = true;
        }
        

    }

    void HighlightCell(int row, int col)
    {
        if (row >= 0 && row < rows && col >= 0 && col < cols)
        {
            SpriteRenderer spriteRenderer = spriteRenderers[row * cols + col]; 
            if (spriteRenderer != null)
            {
                
                spriteRenderer.sprite = highlightSprites;
            }
        }
    }
}
