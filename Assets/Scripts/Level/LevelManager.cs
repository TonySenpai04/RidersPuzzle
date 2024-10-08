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
public struct Level
{
    public List<HiddenObjectInfo> hiddenObjects; 
}

public class LevelManager : MonoBehaviour
{
    public GridController gridController;
    public List<Level> levels;
    public int currentLevelIndex = 1;
    public static LevelManager instance;
    private Dictionary<Vector2Int, GameObject> hiddenObjectInstances = new Dictionary<Vector2Int, GameObject>();
    private void Awake()
    {
        instance = this;
    }
    void Start()
    {
        LoadLevel(currentLevelIndex);
    }

    void LoadLevel(int levelIndex)
    {
        int lv = levelIndex - 1;
        if (lv >= 0 && lv < levels.Count)
        {
            Level level = levels[lv];
            ClearHiddenObjects();
            foreach (HiddenObjectInfo hiddenObjectInfo in level.hiddenObjects)
            {
                GameObject cell = gridController.grid[hiddenObjectInfo.row, hiddenObjectInfo.col];
                GameObject hiddenObject = Instantiate(hiddenObjectInfo.objectPrefab, cell.transform.position, Quaternion.identity);
                hiddenObject.transform.SetParent(cell.transform);
                hiddenObject.SetActive(false); 

                Vector2Int positionKey = new Vector2Int(hiddenObjectInfo.row, hiddenObjectInfo.col);
                hiddenObjectInstances[positionKey] = hiddenObject;
            }
        }
        else
        {
            Debug.LogWarning("Level index is out of range.");
        }
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
}

