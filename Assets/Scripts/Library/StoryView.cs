using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class StoryView : MonoBehaviour
{
    public string id;
    public TextMeshProUGUI titleTxt;
    public TextMeshProUGUI descriptionTxt;
    public GameObject content;

    private ISwipeDetector swipeDetector;
    private List<Story> seenStories = new List<Story>();
    private int currentIndex;

    void Start()
    {
        swipeDetector = new SwipeDetector(NextStory, PreviousStory);

        // Lấy danh sách story đã được xem (isSeen = true)
        seenStories = StoryManager.instance.stories.FindAll(s => s.isSeen);
    }

    void Update()
    {
        swipeDetector.DetectSwipe();
    }

    private void OnEnable()
    {
        seenStories.Clear();
        seenStories = StoryManager.instance.stories.FindAll(s => s.isSeen);
    }

    private IEnumerator AdjustContentHeight()
    {
        // Chờ 1 frame để TextMeshPro cập nhật preferredHeight
        yield return null;

        float preferredHeight = descriptionTxt.preferredHeight;

        float extraPadding = 200f; 

        RectTransform contentRect = content.GetComponent<RectTransform>();
        if (contentRect != null)
        {
            Vector2 sizeDelta = contentRect.sizeDelta;
            sizeDelta.y = preferredHeight + extraPadding; 
            contentRect.sizeDelta = sizeDelta;
        }
    }

    public void SetData(string id)
    {
        this.id = id;

        // Reset scroll của content
        RectTransform rectTransform = content.GetComponent<RectTransform>();
        if (rectTransform != null)
        {
            rectTransform.anchoredPosition = new Vector2(rectTransform.anchoredPosition.x, 0);
        }

        // Gán text dựa trên dữ liệu của story
        Story story = StoryManager.instance.GetByStoryId(id);
        if (story != null)
        {
            // ApplyText.instance.UpdateStoryInfo(id);
            ApplyTextManager.instance.UpdateStoryInfo(id);
            StartCoroutine(AdjustContentHeight());

        }

        currentIndex = seenStories.FindIndex(s => s.id == id);
    }

    private void LoadStory(int index)
    {
        if (index >= 0 && index < seenStories.Count)
        {
            SetData(seenStories[index].id);
            StoryManager.instance.MarkStoryAsSeen(seenStories[index].id);
        }
    }

    public void NextStory()
    {
        if (currentIndex < seenStories.Count - 1)
        {
            currentIndex++;
            
            LoadStory(currentIndex);
        }
    }

    public void PreviousStory()
    {
        if (currentIndex > 0)
        {
            currentIndex--;
            LoadStory(currentIndex);
        }
    }
}
