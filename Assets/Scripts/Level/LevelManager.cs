using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public struct HiddenObjectInfo
{
    public int row;   
    public int col;   
    public GameObject objectPrefab; 
}
[Serializable]
public struct WallInfo
{
    public int row; 
    public int col;  

}

[Serializable]
public struct Level
{
    public List<HiddenObjectInfo> hiddenObjects;
    public List<WallInfo> wallPositions;
    public int key;
    public bool isActiveObject;
}

public class LevelManager : MonoBehaviour
{
    [SerializeField] private GridController gridController;
    [SerializeField] private List<Level> levels;
    [SerializeField] private int currentLevelIndex = 1;
    [SerializeField] private TextAsset data;
    [SerializeField] private HiddenObjectManager hiddenObjectManager;
    // public GameObject wallPrefab;
    public  Dictionary<Vector2Int, GameObject> hiddenObjectInstances = new Dictionary<Vector2Int, GameObject>();
    private Dictionary<Vector2Int, GameObject> wallInstances = new Dictionary<Vector2Int, GameObject>();
    private Dictionary<Vector2Int, BoxCollider2D> cellColliders = new Dictionary<Vector2Int, BoxCollider2D>();
    private IReadData cSVReader;
    private LevelDataController levelDataController;
    private HiddenObjectHandler hiddenObjectHandler;
    public static LevelManager instance;
    private void Awake()
    {
        instance = this;
        cSVReader = new CSVReader();
        levelDataController = new LevelDataController(data, cSVReader, hiddenObjectManager);
        hiddenObjectHandler = new HiddenObjectHandler();
    }
    void Start()
    {
        LoadLevelData();
        LoadLevel(currentLevelIndex);
    }
    public GridController GetGrid()
    {
        return this.gridController;
    }
    public void LoadLevelData()
    {
        levelDataController.LoadLevelData(levels);
       
    }
    public  LevelDataInfo GetCurrentLevelData()
    {
       return levelDataController.GetLevelData(levels, currentLevelIndex);
    }
    void LoadLevel(int levelIndex)
    {
        int lv = levelIndex - 1;
        if (lv >= 0 && lv < levels.Count)
        {
            Level level = levels[lv];
            LoadObject(level);
            //LoadWall(level);

        }
        else
        {
            Debug.LogWarning("Level index is out of range.");
        }
    }
    private void LoadObject(Level level)
    {
        hiddenObjectHandler.LoadHiddenObjects(level, gridController, level.isActiveObject, hiddenObjectInstances);
       
    }
    //private void LoadWall(Level level)
    //{
    //    ClearWalls();
    //    if (level.wallPositions.Count > 0)
    //    {
    //        foreach (WallInfo wallInfo in level.wallPositions)
    //        {
    //            GameObject cell = gridController.grid[wallInfo.row, wallInfo.col];
    //            GameObject wallObject = Instantiate(wallPrefab, cell.transform.position, Quaternion.identity);
    //            wallObject.transform.SetParent(cell.transform);
    //            BoxCollider2D collider = wallObject.GetComponentInParent<BoxCollider2D>();
    //            if (collider != null)
    //            {
    //                // Lưu collider vào cellColliders
    //                Vector2Int positionKey = new Vector2Int(wallInfo.row, wallInfo.col);
    //                cellColliders[positionKey] = collider;
    //                collider.enabled = false; // Tắt collider
    //            }

    //            Vector2Int wallPositionKey = new Vector2Int(wallInfo.row, wallInfo.col);
    //            wallInstances[wallPositionKey] = wallObject;
    //        }
    //    }
    //}
    //void ClearWalls()
    //{

    //    foreach (var entry in wallInstances)
    //    {
    //        Destroy(entry.Value);
    //    }
    //    wallInstances.Clear();

    //    foreach (var entry in cellColliders)
    //    {
    //        if (entry.Value != null)
    //        {
    //            entry.Value.enabled = true; // Bật collider lại
    //        }
    //    }
    //    cellColliders.Clear();

    //    foreach (var colliderEntry in cellColliders)
    //    {
    //        if (colliderEntry.Value != null)
    //        {
    //            colliderEntry.Value.enabled = true; // Bật collider lại
    //        }
    //    }

    //    //foreach (var entry in wallInstances)
    //    //{
    //    //    BoxCollider2D collider = entry.Value.GetComponentInParent<BoxCollider2D>();
    //    //    if (collider != null)
    //    //    {
    //    //        collider.enabled = true; 
    //    //    }
    //    //}
    //    wallInstances.Clear();
    //}

    public int GetCurrentLevelKey()
    {
        int lv = currentLevelIndex - 1;
        if (lv >= 0 && lv < levels.Count)
        {
            return levels[lv].key;
        }
        else
        {
            Debug.LogWarning("Level index is out of range.");
            return -1; 
        }
    }
    public void DecreaseLevelKey()
    {
        int lv = currentLevelIndex - 1;
        Level currentLevel = levels[lv];
        if (currentLevel.key > 0)
        {
            currentLevel.key--;
        }
        levels[lv] = currentLevel;
    }
    public void IncreaseLevelKey()
    {
        int lv = currentLevelIndex - 1;
        Level currentLevel = levels[lv];
        if (currentLevel.key > 0)
        {
            currentLevel.key++;
        }
        levels[lv] = currentLevel;
    }
    public GameObject CheckForHiddenObject(int row, int col)
    {
        // Kiểm tra xem vị trí (row, col) có object nào không
        Vector2Int positionKey = new Vector2Int(row, col);
        if (hiddenObjectInstances.ContainsKey(positionKey))
        {
            GameObject hiddenObject = hiddenObjectInstances[positionKey];
            if (hiddenObject != null)
            {
                hiddenObject.SetActive(true);
                return hiddenObject;
                
            }
            

        }
        return null;

    }
    public List<GameObject> GetHiddenObjectsInRow(int row)
    {   
        List<GameObject> objectsInRow = hiddenObjectHandler.GetHiddenObjectsInRow(row, hiddenObjectInstances);

        return objectsInRow;
    }
    public List<GameObject> GetHiddenObjectsInColumn(int col)
    {
        List<GameObject> objectsInColumn = hiddenObjectHandler.GetHiddenObjectsInColumn(col, hiddenObjectInstances);
 
        return objectsInColumn;
    }
    public List<GameObject> GetHiddenObjectsAround(int row, int col)
    {
        List<GameObject> objectsAround = hiddenObjectHandler.GetHiddenObjectsAround(row,col, hiddenObjectInstances);

        return objectsAround;
    }
   
    public void AddHiddenObjectToCurrentLevel(int row, int col, GameObject prefab)
    {
        Level currentLevel = levels[currentLevelIndex - 1];

        HiddenObjectInfo newHiddenObject = new HiddenObjectInfo
        {
            row = row,
            col = col,
            objectPrefab = prefab
        };

        currentLevel.hiddenObjects.Add(newHiddenObject);
        Vector2Int positionKey = new Vector2Int(newHiddenObject.row, newHiddenObject.col);
        hiddenObjectInstances[positionKey] = prefab;
    }


}

