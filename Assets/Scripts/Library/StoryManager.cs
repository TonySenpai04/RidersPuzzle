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
[Serializable]
public class StorySeenData
{
    public List<string> seenStoryIds = new List<string>();
}

public class StoryManager : MonoBehaviour
{
    public  List<Story> stories;
    public static StoryManager instance;
    public int count;
    public bool isLoaded;
    private string path => Path.Combine(Application.persistentDataPath, "LoginData.json");
    private List<string> seenStoryIds = new List<string>();

    private string loginDataPath => Path.Combine(Application.persistentDataPath, "LoginData.json");
    private string seenStoryPath => Path.Combine(Application.persistentDataPath, "SeenStories.json");
    private List<string> newStoryIds = new List<string>();
    private string newStoryPath => Path.Combine(Application.persistentDataPath, "NewStoryIds.json");


    private void Awake()
    {
        instance = this;
        LoadSeenStories();
        LoadNewStories();
        UpdateStoryQuantity();
        isLoaded = !File.Exists(loginDataPath); // Nếu chưa có login file thì bắt đầu unlock
    }

    private void FixedUpdate()
    {
        if (isLoaded)
        {
            int levelWinCount = LevelManager.instance.GetAllLevelComplete();
            int storyToUnlock = levelWinCount / 30;
            UnlockStories(storyToUnlock);
        }
    }
    public bool IsNewStory(string id)
    {
        return newStoryIds.Contains(id);
    }
    private void SaveNewStories()
    {
        var data = new StorySeenData { seenStoryIds = newStoryIds };
        string json = JsonUtility.ToJson(data);
        File.WriteAllText(newStoryPath, json);
    }
    private void LoadNewStories()
    {
        if (File.Exists(newStoryPath))
        {
            string json = File.ReadAllText(newStoryPath);
            var data = JsonUtility.FromJson<StorySeenData>(json);
            newStoryIds = data.seenStoryIds ?? new List<string>();
        }
        else
        {
            newStoryIds = new List<string>();
        }
    }

    public void UpdateStoryQuantity()
    {
        count = LevelManager.instance.GetAllLevelComplete() / 30;
    }

    public Story GetByStoryId(string id)
    {
        return stories.Find(s => s.id == id);
    }

    private void UnlockStories(int unlockCount)
    {
        bool hasNewStory = false;

        for (int i = 0; i < stories.Count; i++)
        {
            if (i < unlockCount)
            {
                Story story = stories[i];

                if (!seenStoryIds.Contains(story.id))
                {
                    seenStoryIds.Add(story.id);
                    newStoryIds.Add(story.id);
                    NotiManager.instance.ShowMultipleNotiRedDots(new List<string> { "storylib", "library" });
                    story.isSeen = true;
                    Debug.Log($"🔓 Story mới mở: {story.id}");
                    hasNewStory = true;
                }

                else
                {
                    story.isSeen = true;
                }
            }
            else
            {
                stories[i].isSeen = false;
            }
        }

        if (hasNewStory)
        {
            SaveSeenStories();
            SaveNewStories();
        }

    }
    public void MarkStoryAsSeen(string id)
    {
        if (newStoryIds.Contains(id))
        {
            newStoryIds.Remove(id);
            SaveNewStories();
        }
        if (newStoryIds.Count == 0)
        {
            NotiManager.instance.ClearMultipleNotiRedDots(new List<string> { "storylib" });
        }
    }

    private void SaveSeenStories()
    {
        StorySeenData data = new StorySeenData { seenStoryIds = seenStoryIds };
        string json = JsonUtility.ToJson(data);
        File.WriteAllText(seenStoryPath, json);
    }

    private void LoadSeenStories()
    {
        if (File.Exists(seenStoryPath))
        {
            string json = File.ReadAllText(seenStoryPath);
            StorySeenData data = JsonUtility.FromJson<StorySeenData>(json);
            seenStoryIds = data.seenStoryIds ?? new List<string>();
        }
        else
        {
            seenStoryIds = new List<string>();
        }

        foreach (var story in stories)
        {
            story.isSeen = seenStoryIds.Contains(story.id);
        }
    }
}
