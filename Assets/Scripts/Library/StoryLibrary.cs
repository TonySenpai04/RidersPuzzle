﻿using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class StoryLibrary:MonoBehaviour
{
    [SerializeField] private string id;
    [SerializeField] private Image storyImage;
    [SerializeField] private TextMeshProUGUI txtID;
    [SerializeField] private GameObject redNotiDot;

    public string Id { get => id; set => id = value; }

    private void Start()
    {

        UpdateVisibility();

    }

    public void SetStory(string id, Sprite storyImage)
    {
        this.Id = id;
        this.storyImage.sprite = storyImage;
        this.txtID.text = id;
        txtID.gameObject.SetActive(false);
       
    }
    private void OnEnable()
    {

        UpdateVisibility();

    }
    //private void FixedUpdate()
    //{

    //    redNotiDot.SetActive(StoryManager.instance.IsNewStory(this.Id));
    //}
    public void UpdateVisibility()
    {
        var hiddenObject = StoryManager.instance.GetByStoryId(this.Id);
        if (hiddenObject != null && hiddenObject.isSeen)
        {
            txtID.gameObject.SetActive(false);
            storyImage.gameObject.SetActive(true);
            storyImage.sprite = hiddenObject.sprite;
            
        }
        else
        {
            txtID.gameObject.SetActive(true);
            storyImage.gameObject.SetActive(false);
            redNotiDot.SetActive(false);
            StoryManager.instance.MarkStoryAsSeen(id);
        }

        redNotiDot.SetActive(StoryManager.instance.IsNewStory(this.Id));
    }
    public void SetSeenObject(bool isSeen)
    {
        var hiddenObject = StoryManager.instance.GetByStoryId(this.Id);
        if (hiddenObject != null)
        {
            hiddenObject.isSeen = isSeen;
        }
    }
}