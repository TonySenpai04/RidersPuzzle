using System;
using System.Collections;
using System.Collections.Generic;
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
    void Start()
    {
        instance = this;
       count = LevelManager.instance.GetAllLevelComplete()/30;
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
        if(Input.GetKey(KeyCode.L))
        {
            Debug.Log(LevelManager.instance.GetAllLevelComplete());
        }
        int levelWinCount = LevelManager.instance.GetAllLevelComplete();
        int storyToUnlock = levelWinCount / 30;
        if(count < storyToUnlock)
        {
            NotiManager.instance.ShowNotiRedDot("library");
            NotiManager.instance.ShowNotiRedDot("storylib");
            count = storyToUnlock;
        }
    }

}
