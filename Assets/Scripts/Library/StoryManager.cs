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


    private string currentUID = "guest";

    //public int count;
    //public bool isLoaded = false;

    private void Awake()
    {
        instance = this;
    }

    // Gọi khi login / logout (sau khi có UID hoặc guest)
    public void Init(string uid)
    {
        currentUID = string.IsNullOrEmpty(uid) ? "guest" : uid;
        foreach (var story in stories)
        {
            story.isSeen = false;
        }

        LoadSeenStories();
        LoadNewStories();
        UpdateStoryQuantity();
        UnlockStories(LevelManager.instance.GetAllLevelComplete() / 30);
    }

    public void UpdateStoryQuantity()
    {
        count = LevelManager.instance.GetAllLevelComplete() / 30;
    }

    public Story GetByStoryId(string id) => stories.Find(s => s.id == id);

    public bool IsNewStory(string id) => newStoryIds.Contains(id);

    private void UnlockStories(int unlockCount)
    {
        bool hasNewStory = false;

        for (int i = 0; i < stories.Count; i++)
        {
            var story = stories[i];

            if (i < unlockCount)
            {
                if (!seenStoryIds.Contains(story.id))
                {
                    seenStoryIds.Add(story.id);
                    newStoryIds.Add(story.id);
                    NotiManager.instance.ShowMultipleNotiRedDots(new List<string> { "storylib", "library" });
                    Debug.Log($"🔓 Story mới mở: {story.id}");
                    hasNewStory = true;
                }
                story.isSeen = true;
            }
            else
            {
                story.isSeen = false;
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

    private string GetSeenPath() => Path.Combine(Application.persistentDataPath, $"SeenStories_{currentUID}.json");
    private string GetNewPath() => Path.Combine(Application.persistentDataPath, $"NewStories_{currentUID}.json");

    private void SaveSeenStories()
    {
        var data = new StorySeenData { seenStoryIds = seenStoryIds };
        File.WriteAllText(GetSeenPath(), JsonUtility.ToJson(data));
    }

    private void SaveNewStories()
    {
        var data = new StorySeenData { seenStoryIds = newStoryIds };
        File.WriteAllText(GetNewPath(), JsonUtility.ToJson(data));
    }

    private void LoadSeenStories()
    {
        string path = GetSeenPath();
        if (File.Exists(path))
        {
            var data = JsonUtility.FromJson<StorySeenData>(File.ReadAllText(path));
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

    private void LoadNewStories()
    {
        string path = GetNewPath();
        if (File.Exists(path))
        {
            var data = JsonUtility.FromJson<StorySeenData>(File.ReadAllText(path));
            newStoryIds = data.seenStoryIds ?? new List<string>();
        }
        else
        {
            newStoryIds = new List<string>();
        }
    }
    public void CheckAndUnlockNewStories()
    {
        int unlockCount = LevelManager.instance.GetAllLevelComplete() / 30;
        UnlockStories(unlockCount);
    }
    private void FixedUpdate()
    {
        CheckAndUnlockNewStories();

        if (newStoryIds.Count > 0)
        {
            NotiManager.instance.ShowMultipleNotiRedDots(new List<string> { "storylib" ,"library"});
        }
        else
        {
            NotiManager.instance.ClearMultipleNotiRedDots(new List<string> { "storylib"});
        }
    }

}