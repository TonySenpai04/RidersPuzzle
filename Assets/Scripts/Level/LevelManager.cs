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
            }
        }
        else
        {
            Debug.LogWarning("Level index is out of range.");
        }
    }
    void ClearHiddenObjects()
    {
        foreach (Transform child in gridController.transform)
        {
            HiddenObject hiddenObject = child.GetComponentInChildren<HiddenObject>();
            if (hiddenObject != null)
            {
                Destroy(hiddenObject.gameObject);
            }
        }
    }
    public void CheckForHiddenObject(int row, int col)
    {
        // Lặp qua tất cả các hidden objects và kiểm tra xem có object nào ở vị trí (row, col) không
        foreach (HiddenObjectInfo hiddenObjectInfo in levels[currentLevelIndex - 1].hiddenObjects)
        {
            if (hiddenObjectInfo.row == row && hiddenObjectInfo.col == col)
            {
                // Kích hoạt object ẩn
                hiddenObjectInfo.objectPrefab.SetActive(true);
                Debug.Log("Kích hoạt object ẩn tại: " + row + ", " + col);
            }
        }
    }
}

