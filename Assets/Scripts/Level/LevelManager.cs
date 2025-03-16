using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
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
    [SerializeField] private List<Level> levelsClone;
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
        //LoadLevelData();
        // LoadLevel();
        levelDataController.LoadLevelData(levels);
        levelDataController.LoadLevelData(levelsClone);
     
            List<LevelProgressData> allProgress = SaveGameManager.instance.LoadAllProgress();
            foreach (var progress in allProgress)
            {
                int lv = progress.levelIndex;
                if (lv >= 0 && lv < levels.Count)
                {
                    Level tempLevel = levels[lv];
                    tempLevel.isUnlock = progress.isUnlocked;
                    tempLevel.isComplete = progress.isComplete;
                    levels[lv] = tempLevel;
                }
            }
            int highestCompleteLevel = -1;
            for (int i = 0; i < levels.Count; i++)
            {
                if (levels[i].isUnlock && levels[i].isComplete)
                {
                    highestCompleteLevel = i;
                }
            }


            if (highestCompleteLevel + 1 < levels.Count)
            {
                Level tempLevel = levels[highestCompleteLevel + 1];
                tempLevel.isUnlock = true;
                levels[highestCompleteLevel + 1] = tempLevel;
            }

       
    }
    public int  GetTotalLevel()
    {
        return levels.Count;
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
        levels.Clear();
        levelsClone.Clear();
        levelDataController.LoadLevelData(levels);
        levelDataController.LoadLevelData(levelsClone);
        FirebaseDataManager.Instance.LoadPlayerData((loadedData) =>
        {
            List<LevelProgressData> allProgress = loadedData.levelData;
            foreach (var progress in allProgress)
            {
                int lv = progress.levelIndex;
                if (lv >= 0 && lv < levels.Count)
                {
                    Level tempLevel = levels[lv];
                    tempLevel.isUnlock = progress.isUnlocked;
                    tempLevel.isComplete = progress.isComplete;
                    levels[lv] = tempLevel;
                }
            }
             int highestCompleteLevel = -1; 
        for (int i = 0; i < levels.Count; i++)
        {
            if (levels[i].isUnlock && levels[i].isComplete)
            {
                highestCompleteLevel = i;
            }
        }


        if (highestCompleteLevel + 1 < levels.Count)
        {
            Level tempLevel = levels[highestCompleteLevel+1];
            tempLevel.isUnlock = true;
            levels[highestCompleteLevel + 1] = tempLevel;
        }

        });
        
       
    }

    public  LevelDataInfo GetCurrentLevelData()
    {
       return levelDataController.GetLevelData(levels, currentLevelIndex);
    }
    public void LoadLevel()
    {
        ResetObjectData();
     //   currentLevelIndex = levelIndex;
        int lv = currentLevelIndex - 1;

        if (lv < 0 || lv >= levels.Count)
        {
            Debug.LogWarning("Level index is out of range.");
            return;
        }
        if (!levels[lv].isUnlock)
            return;
        if (lv >= 0 && lv <= levels.Count)
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
        if (lv < 0 || lv >= levels.Count)
        {
            Debug.LogWarning("Level index is out of range.");
            return;
        }
        Level tempLevel = levels[lv];
        tempLevel.isComplete = true;
        levels[lv] = tempLevel;
        int nextLevelIndex = currentLevelIndex; 
        if (nextLevelIndex < levels.Count) 
        {
            Level nextLevel = levels[nextLevelIndex];
            nextLevel.isUnlock = true; 
            levels[nextLevelIndex] = nextLevel;
            SaveGameManager.instance.SaveLevelProgress(lv, tempLevel.isUnlock, tempLevel.isComplete);
          //  SaveGameManager.instance.SaveLevelProgress(nextLevelIndex, nextLevel.isUnlock, nextLevel.isComplete);
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
        int lv=currentLevelIndex - 1;
        if (lv < 0 || lv > levels.Count)
        {
            Debug.LogWarning("Level index is out of range.");
            return;
        }
        Debug.Log(lv);
       LoadLevel();
    }
    public bool isFinal()
    {
        return currentLevelIndex > levels.Count;
    }
    public bool IsCompleteLoadObject()
    {
        return hiddenObjectHandler.IsCompleteLoadObject();
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
            return 0; 
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
        if (currentLevel.key < 0)
            currentLevel.key = 0; 
        levels[lv] = currentLevel;
    }
    public void IncreaseLevelKey()
    {
        int lv = currentLevelIndex - 1;
        Level currentLevel = levels[lv];
        if (currentLevel.key >= 0)
        {
            currentLevel.key++;
        }
        levels[lv] = currentLevel;
    }
    public GameObject CheckForHiddenObject(int row, int col)
    {
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
            tempLevel.isComplete = true;
            levels[i] = tempLevel;
            SaveGameManager.instance.SaveLevelProgress(i, tempLevel.isUnlock, tempLevel.isComplete);
        }
    }

    public void ResetObjectData()
    {
        if (levelsClone == null || levelsClone.Count != levels.Count)
        {
            Debug.LogWarning("LevelsClone data is not initialized or does not match levels.");
            return;
        }

        for (int i = 0; i < levelsClone.Count; i++)
        {
            Level currentLevel = levels[i];
            Level originalLevel = levelsClone[i];

            currentLevel.hiddenObjects = new List<HiddenObjectInfo>(originalLevel.hiddenObjects);
            levels[i] = currentLevel;
        }

        Debug.Log("Object data has been reset for all levels.");
    }
    
    public void ResetAllLevels()
    {
        for (int i = 0; i < levels.Count; i++)
        {
            // Đánh dấu tất cả các màn chơi là chưa mở khóa và chưa hoàn thành
            Level tempLevel = levels[i];
            if (i == 0)
            {
                tempLevel.isUnlock = true;
            }
            else
            {
                tempLevel.isUnlock = false;
            }
            tempLevel.isComplete = false;
            levels[i] = tempLevel;

            // Lưu lại trạng thái vào PlayerPrefs
         SaveGameManager.instance.SaveLevelProgress(i, tempLevel.isUnlock, tempLevel.isComplete);
        }

        Debug.Log("All levels have been reset.");
    }
    public int GetAllLevelComplete()
    {
        return levels.Where(h => h.isComplete).ToList().Count;
    }
}

