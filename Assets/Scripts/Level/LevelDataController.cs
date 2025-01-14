using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelDataController 
{
    public TextAsset data;
    public IReadData csvReader;
    public HiddenObjectManager hiddenObjectManager;
    public LevelDataController( TextAsset data, IReadData csvReader, HiddenObjectManager hiddenObjectManager)
    {

        this.data = data;
        this.csvReader = csvReader;
        this.hiddenObjectManager = hiddenObjectManager;
    }
    public LevelDataInfo GetLevelData(List<Level> levels,int levelIndex)
    {
        var levelDatas = csvReader.ReadLevelData(data);
        if (levelIndex < 0 || levelIndex > levels.Count)
        {
            Debug.LogWarning("Level index is out of range.");
            return new LevelDataInfo(); 
        }

        Level level = levels[levelIndex-1];
        LevelDataInfo levelDataInfo = new LevelDataInfo();
        foreach (var levelData in levelDatas)
        {
            if(levelData.Key == levelIndex)
            {
                levelDataInfo = levelData.Value;
                break ;
            }
        }
        return levelDataInfo;
          
    }
    public void LoadLevelData(List<Level> levels)
    {
        var levelDatas = csvReader.ReadLevelData(data);
        foreach (var levelData in levelDatas)
        {
            Level level = new Level();
            level.hiddenObjects = new List<HiddenObjectInfo>(); 
            level.isActiveObject = levelData.Value.isActive;
            level.difficulty=levelData.Value.difficulty;
            string key = "level" + (levelData.Value.level + 1) + "_unlocked";
            bool isUnlocked = PlayerPrefs.GetInt(key, 0) == 1;
            level.isUnlock = isUnlocked;
            foreach (var entry in levelData.Value.positions)
            {
                HiddenObjectInfo hiddenObjectInfo = new();
                Vector2Int pos = entry.Key;
                hiddenObjectInfo.row = pos.x;
                hiddenObjectInfo.col = pos.y;

                HiddenObject hiddenObject = hiddenObjectManager.GetById(entry.Value.ToString());
                if (hiddenObject != null)
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
        Level tempLevel = levels[0]; 
        tempLevel.isUnlock = true;   
        levels[0] = tempLevel;
    }

    public Level GetLevel(List<Level> levels,int levelIndex)
    {
        return levels[levelIndex];
    }
}
