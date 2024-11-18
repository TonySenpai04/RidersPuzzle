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
    public GridController gridController;
    public List<Level> levels;
    public int currentLevelIndex = 1;
    public static LevelManager instance;
    public GameObject wallPrefab;
    public  Dictionary<Vector2Int, GameObject> hiddenObjectInstances = new Dictionary<Vector2Int, GameObject>();
    private Dictionary<Vector2Int, GameObject> wallInstances = new Dictionary<Vector2Int, GameObject>();
    private Dictionary<Vector2Int, BoxCollider2D> cellColliders = new Dictionary<Vector2Int, BoxCollider2D>();
    public CSVReader cSVReader;
    public HiddenObjectManager HiddenObjectManager;
    private void Awake()
    {
        instance = this;
       
    }
    void Start()
    {
        LoadLevelData();
        LoadLevel(currentLevelIndex);
    }
    public void LoadLevelData()
    {
        foreach (var levelData in cSVReader.levelData)
        {
            Level level = new Level();
            level.hiddenObjects = new List<HiddenObjectInfo>(); // Khởi tạo hiddenObjects
            level.isActiveObject = levelData.Value.isActive;
            Debug.Log(levelData.Value.positions.Count);
            foreach (var entry in levelData.Value.positions)
            {
                HiddenObjectInfo hiddenObjectInfo = new HiddenObjectInfo();
                Vector2Int pos = entry.Key;
                hiddenObjectInfo.row = pos.x;
                hiddenObjectInfo.col = pos.y;

               // Lấy đối tượng từ HiddenObjectManager
               HiddenObject hiddenObject = HiddenObjectManager.GetById(entry.Value.ToString());
                if (hiddenObject != null) // Kiểm tra đối tượng không null
                {
                    hiddenObjectInfo.objectPrefab = hiddenObject.gameObject;
                    level.hiddenObjects.Add(hiddenObjectInfo);
                }
                else
                {
                    Debug.LogWarning($"Không tìm thấy HiddenObject với ID {entry.Value}");
                }
            }
            levels.Add(level);
        }
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
        ClearHiddenObjects();
        foreach (HiddenObjectInfo hiddenObjectInfo in level.hiddenObjects)
        {
            GameObject cell = gridController.grid[hiddenObjectInfo.row, hiddenObjectInfo.col];
            GameObject hiddenObject = Instantiate(hiddenObjectInfo.objectPrefab, cell.transform.position, Quaternion.identity);
            hiddenObject.transform.SetParent(cell.transform);

            Vector2Int positionKey = new Vector2Int(hiddenObjectInfo.row, hiddenObjectInfo.col);
            hiddenObjectInstances[positionKey] = hiddenObject;
            if (!level.isActiveObject)
            {
                hiddenObject.SetActive(false);
            }
            else
            {
                hiddenObject.SetActive(true);
            }
        }
    }
    private void LoadWall(Level level)
    {
        ClearWalls();
        if (level.wallPositions.Count > 0)
        {
            foreach (WallInfo wallInfo in level.wallPositions)
            {
                GameObject cell = gridController.grid[wallInfo.row, wallInfo.col];
                GameObject wallObject = Instantiate(wallPrefab, cell.transform.position, Quaternion.identity);
                wallObject.transform.SetParent(cell.transform);
                BoxCollider2D collider = wallObject.GetComponentInParent<BoxCollider2D>();
                if (collider != null)
                {
                    // Lưu collider vào cellColliders
                    Vector2Int positionKey = new Vector2Int(wallInfo.row, wallInfo.col);
                    cellColliders[positionKey] = collider;
                    collider.enabled = false; // Tắt collider
                }

                Vector2Int wallPositionKey = new Vector2Int(wallInfo.row, wallInfo.col);
                wallInstances[wallPositionKey] = wallObject;
            }
        }
    }
    void ClearWalls()
    {

        foreach (var entry in wallInstances)
        {
            Destroy(entry.Value);
        }
        wallInstances.Clear();

        foreach (var entry in cellColliders)
        {
            if (entry.Value != null)
            {
                entry.Value.enabled = true; // Bật collider lại
            }
        }
        cellColliders.Clear();

        foreach (var colliderEntry in cellColliders)
        {
            if (colliderEntry.Value != null)
            {
                colliderEntry.Value.enabled = true; // Bật collider lại
            }
        }

        //foreach (var entry in wallInstances)
        //{
        //    BoxCollider2D collider = entry.Value.GetComponentInParent<BoxCollider2D>();
        //    if (collider != null)
        //    {
        //        collider.enabled = true; 
        //    }
        //}
        wallInstances.Clear();
    }
    void ClearHiddenObjects()
    {
        // Dọn dẹp các hidden objects cũ
        foreach (GameObject hiddenObject in hiddenObjectInstances.Values)
        {
            Destroy(hiddenObject);
        }
        hiddenObjectInstances.Clear(); // Xóa dictionary để chuẩn bị cho level mới
    }
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
        List<GameObject> objectsInRow = new List<GameObject>();

        foreach (var entry in hiddenObjectInstances)
        {
            if (entry.Key.x == row) 
            {
                objectsInRow.Add(entry.Value); 
            }
        }

        return objectsInRow;
    }
    public List<GameObject> GetHiddenObjectsInColumn(int col)
    {
        List<GameObject> objectsInColumn = new List<GameObject>();

        foreach (var entry in hiddenObjectInstances)
        {
            if (entry.Key.y == col)
            {
                objectsInColumn.Add(entry.Value); 
            }
        }

        return objectsInColumn;
    }
    public List<GameObject> GetHiddenObjectsAround(int row, int col)
    {
        List<GameObject> objectsAround = new List<GameObject>();

        Vector2Int[] directions = {
        new Vector2Int(-1, 0), // Ô trên
        new Vector2Int(1, 0),  // Ô dưới
        new Vector2Int(0, -1), // Ô bên trái
        new Vector2Int(0, 1)   // Ô bên phải
    };

        foreach (var dir in directions)
        {
            int newRow = row + dir.x;
            int newCol = col + dir.y;
            Vector2Int positionKey = new Vector2Int(newRow, newCol);
            if (hiddenObjectInstances.ContainsKey(positionKey))
            {
                objectsAround.Add(hiddenObjectInstances[positionKey]); 
            }
        }

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

