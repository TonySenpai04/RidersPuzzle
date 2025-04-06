using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

[Serializable]
public class Story {
    public string id;
    public string title;
    public string content;
    public bool isSeen;
    public Sprite sprite;
}

public class StoryManager : MonoBehaviour
{
    public  List<Story> stories;
    public static StoryManager instance;
    public int count;
    public bool isLoaded;
    private string path => Path.Combine(Application.persistentDataPath, "LoginData.json");
    void Start()
    {
        instance = this;
        UpdateStoryQuantity();
        if (!File.Exists(path))
        {
            isLoaded=true;

        }
    }

    public void UpdateStoryQuantity()
    {
        count = LevelManager.instance.GetAllLevelComplete() / 30;
    }
    public Story GetByStoryId(string id)
    {
        foreach (var obj in stories)
        {
            if (obj.id == id)
            {
                return obj;
            }
        }
        return null;
    }
    public void FixedUpdate()
    {
        if (isLoaded)
        {
            int levelWinCount = LevelManager.instance.GetAllLevelComplete();
            int storyToUnlock = levelWinCount / 30;
            UnlockStories(storyToUnlock);

        }
    }
    private void UnlockStories(int unlockCount)
    {
        for (int i = 0; i < stories.Count; i++)
        {
            if (i < unlockCount)
            {
                stories[i].isSeen = true;
            }
            else
            {
                stories[i].isSeen = false;
            }
        }
    }

}
