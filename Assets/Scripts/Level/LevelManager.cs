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
    public bool isComplete;
    public bool isUnlock;
    public string difficulty;
}

public class LevelManager : MonoBehaviour
{
    [SerializeField] private GridController gridController;
    [SerializeField] private List<Level> levels;
    [SerializeField] public int currentLevelIndex = 1;
    [SerializeField] private TextAsset data;
    [SerializeField] private HiddenObjectManager hiddenObjectManager;
    // public GameObject wallPrefab;
    public  Dictionary<Vector2Int, GameObject> hiddenObjectInstances = new Dictionary<Vector2Int, GameObject>();
    private Dictionary<Vector2Int, GameObject> wallInstances = new Dictionary<Vector2Int, GameObject>();
    //private Dictionary<Vector2Int, BoxCollider2D> cellColliders = new Dictionary<Vector2Int, BoxCollider2D>();
 // [SerializeField] private List< BoxCollider2D> cellColliders = new List<BoxCollider2D>();
    private IReadData cSVReader;
    private LevelDataController levelDataController;
    public HiddenObjectHandler hiddenObjectHandler;
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
        LoadLevel();
    }
    public void ReplaceHiddenObject(Vector2Int position, GameObject newObject)
    {
        if (hiddenObjectInstances.ContainsKey(position))
        {
            hiddenObjectInstances[position] = newObject;
        }
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
    public void LoadLevel()
    {
     //   currentLevelIndex = levelIndex;
        int lv = currentLevelIndex - 1;
        if (!levels[lv].isUnlock)
            return;
        if (lv >= 0 && lv < levels.Count)
        {
            Level level = levels[lv];
            gridController.ClearCollider();
            LoadObject(level);


        }
        else
        {
            Debug.LogWarning("Level index is out of range.");
        }
    }
    public void UnlockNextLevel()
    {
        int lv = currentLevelIndex - 1;
        Level tempLevel = levels[lv];
        tempLevel.isComplete = true;
        levels[lv] = tempLevel;
        int nextLevelIndex = currentLevelIndex; 
        if (nextLevelIndex < levels.Count) 
        {
            Level nextLevel = levels[nextLevelIndex];
            nextLevel.isUnlock = true; 
            levels[nextLevelIndex] = nextLevel;
        }
    }

    public bool IsLevelUnlocked(int levelIndex)
    {
        int lv = levelIndex - 1;

        if (lv >= 0 && lv < levels.Count)
        {
            return levels[lv].isUnlock;
        }

        return false; // Mặc định là chưa mở khóa nếu index không hợp lệ
    }


    public void SetLevel(int index)
    {
        this.currentLevelIndex = index;
    }
    public void LoadNextLevel()
    {
        currentLevelIndex++;
        LoadLevel();
    }
    public void LoadObject(Level level)
    {
        hiddenObjectHandler.ClearHiddenObjectsNoCroutine(hiddenObjectInstances, gridController);
         hiddenObjectHandler.LoadHiddenObjects(level, gridController, level.isActiveObject, hiddenObjectInstances);

    }
    public void ClearObject()
    {
        hiddenObjectHandler.ClearHiddenObjects(hiddenObjectInstances, gridController);
    }
    public Level GetLevel(int level)
    {
        return levels[level];
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
    public void UnlockAllLevels()
    {
        for (int i = 0; i < levels.Count; i++)
        {
            Level tempLevel = levels[i];
            tempLevel.isUnlock = true;
            levels[i] = tempLevel;
        }
    }


}

