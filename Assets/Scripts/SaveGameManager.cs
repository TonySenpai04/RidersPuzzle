using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SaveGameManager : MonoBehaviour
{
   public static SaveGameManager instance;
    void Awake()
    {
        instance = this;


    }

    public void SaveLevelProgress(int levelIndex, bool isUnlocked, bool isComplete)
    {
        string unlockKey = "level" + (levelIndex + 1) + "_unlocked";
        string completeKey = "level" + (levelIndex + 1) + "_complete";

        // Lưu trạng thái mở khóa và hoàn thành của màn
        PlayerPrefs.SetInt(unlockKey, isUnlocked ? 1 : 0);
        PlayerPrefs.SetInt(completeKey, isComplete ? 1 : 0);
        PlayerPrefs.Save();
    }

}
