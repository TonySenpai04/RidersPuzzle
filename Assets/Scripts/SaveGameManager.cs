

using System.Collections.Generic;
using System;
using System.IO;
using UnityEngine;



public class SaveGameManager : MonoBehaviour
{
    public static SaveGameManager instance;

    private string saveFilePath;

    private void Awake()
    {
        instance = this;
        saveFilePath = Path.Combine(Application.persistentDataPath, "savegame.json");
    }

    public void SaveLevelProgress(int levelIndex, bool isUnlocked, bool isComplete)
    {
        LevelProgressData progressData = new LevelProgressData
        {
            levelIndex = levelIndex,
            isUnlocked = isUnlocked,
            isComplete = isComplete
        };

        // Đọc dữ liệu cũ (nếu có)
        List<LevelProgressData> allProgress = LoadAllProgress();

        // Cập nhật dữ liệu cho level hiện tại
        int index = allProgress.FindIndex(p => p.levelIndex == levelIndex);
        if (index >= 0)
        {
            allProgress[index] = progressData;
        }
        else
        {
            allProgress.Add(progressData);
        }

        // Lưu lại toàn bộ dữ liệu vào file
        string jsonData = JsonUtility.ToJson(new SerializableList<LevelProgressData>(allProgress), true);
        File.WriteAllText(saveFilePath, jsonData);
    }

    public List<LevelProgressData> LoadAllProgress()
    {
        if (File.Exists(saveFilePath))
        {
            string jsonData = File.ReadAllText(saveFilePath);
            SerializableList<LevelProgressData> progressList = JsonUtility.FromJson<SerializableList<LevelProgressData>>(jsonData);
            return progressList.data;
        }

        return new List<LevelProgressData>();
    }
}

[Serializable]
public class SerializableList<T>
{
    public List<T> data;

    public SerializableList(List<T> data)
    {
        this.data = data;
    }
}
