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
    public float cellSize ; 
    public float spacing = 0.1f; 
    public Vector2 centerOffset = new Vector2(0, 0); 
    public GameObject[,] grid;
    [SerializeField] private MovementController character;
    public bool isActiveObject=true;
    public int currentObjectRow;
    public int currentObjectColumn;
    [SerializeField] private List<BoxCollider2D> cellColliders = new List<BoxCollider2D>();
    private List<SpriteRenderer> spriteRenderers = new List<SpriteRenderer>();
    private List<Sprite> originalSprites = new List<Sprite>();
    [SerializeField] private Sprite highlightSprites;

    [SerializeField] private Sprite arrowUpSprite;     // Mũi tên hướng lên
    [SerializeField] private Sprite arrowDownSprite;   // Mũi tên hướng xuống
    [SerializeField] private Sprite arrowLeftSprite;   // Mũi tên hướng trái
    [SerializeField] private Sprite arrowRightSprite;  // Mũi tên hướng phải

    private GameObject arrowUp, arrowDown, arrowLeft, arrowRight;
    private void Awake()
    {
       
        gridGenerator =new GridGenerator(this.gameObject,blockPrefab);
        float targetAspect = 9f / 16f; 
        float screenWidth = Camera.main.orthographicSize * 2 *targetAspect;
        cellSize = (screenWidth - spacing * (cols - 1)) / cols-0.1f;
        gridGenerator.GenerateGrid( rows,  cols,  cellSize,  spacing, centerOffset);
        grid= gridGenerator.Grid();
        GetCollider();
        CreateArrows();

    }
    private void CreateArrows()
    {
        arrowUp = CreateArrow(arrowUpSprite);
        arrowDown = CreateArrow(arrowDownSprite);
        arrowLeft = CreateArrow(arrowLeftSprite);
        arrowRight = CreateArrow(arrowRightSprite);
    }
    private GameObject CreateArrow(Sprite arrowSprite)
    {
        GameObject arrow = new GameObject("Arrow");
        arrow.transform.SetParent(this.transform);
        arrow.AddComponent<SpriteRenderer>().sprite = arrowSprite;
        arrow.GetComponent<SpriteRenderer>().sortingOrder = 3;
        arrow.SetActive(false); 
        return arrow;
    }

    private void UpdateArrowPositions()
    {
        int currentRow = character.GetPos().Item1;
        int currentCol = character.GetPos().Item2;
        float offset = cellSize / 2-0.1f;
        UpdateArrow(arrowUp, currentRow - 1, currentCol, new Vector2(0, -offset));    // Mũi tên trên, dịch xuống sát cạnh
        UpdateArrow(arrowDown, currentRow + 1, currentCol, new Vector2(0, offset));   // Mũi tên dưới, dịch lên sát cạnh
        UpdateArrow(arrowLeft, currentRow, currentCol - 1, new Vector2(offset, 0));   // Mũi tên trái, dịch qua phải sát cạnh
        UpdateArrow(arrowRight, currentRow, currentCol + 1, new Vector2(-offset, 0));
    }
    private void UpdateArrow(GameObject arrow, int row, int col, Vector2 offset)
    {
        if (row >= 0 && row < rows && col >= 0 && col < cols)
        {
            Vector3 cellPosition = grid[row, col].transform.position;
            arrow.transform.position = new Vector3(cellPosition.x + offset.x, cellPosition.y + offset.y, cellPosition.z);
            arrow.SetActive(true);
        }
        else
        {
            arrow.SetActive(false);
        }
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
        ActiveHiddenObject();
        UpdateArrowPositions();

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
}
