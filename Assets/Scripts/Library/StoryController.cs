using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class StoryController : MonoBehaviour
{
    [SerializeField] private StoryLibrary ObjectLibraryPrefabs;
    [SerializeField] private Transform content;
    [SerializeField] private StoryView storyView;
    [SerializeField] private GameObject objectParent;
    [SerializeField] private List<StoryLibrary> storyLibaries;
    [SerializeField] private TextMeshProUGUI objectSeenTxt;

    public List<StoryLibrary> ObjectLibraries { get => storyLibaries; set => storyLibaries = value; }

    void Start()
    {
        int count = StoryManager.instance.stories.Count;
        for (int i = 0; i < count; i++)
        {
            StoryLibrary objectLib = Instantiate(ObjectLibraryPrefabs, content.transform);
            objectLib.SetStory(StoryManager.instance.stories[i].id,
                StoryManager.instance.stories[i].sprite);
            var hiddenObject = StoryManager.instance.GetByStoryId(objectLib.Id);
            if (hiddenObject.isSeen)
            {
                objectLib.GetComponent<Button>().onClick.AddListener(() => SetStoryView(objectLib.Id));
                objectLib.GetComponent<Button>().onClick.AddListener(() => SoundManager.instance.PlaySFX("Click Sound"));
            }
            else
            {
                objectLib.GetComponent<Button>().onClick.AddListener(() => SoundManager.instance.PlaySFX("Click Sound"));
                objectLib.GetComponent<Button>().onClick.AddListener(() =>
                NotiManager.instance.ShowNotification("Unlock after completing each 50 stage"));
            }
            storyLibaries.Add(objectLib);

        }
        int unlockCount = LevelManager.instance.GetAllLevelComplete() / 30;
        objectSeenTxt.text = unlockCount.ToString();
        UpdateSeenStory();
    }
    private void OnEnable()
    {
        UpdateSeenStory();
    }
    void UnlockStoriesByLevel()
    {
        int seen = LevelManager.instance.GetAllLevelComplete();
        int unlockCount = seen / 30;
        objectSeenTxt.text = unlockCount.ToString();
        for (int i = 0; i <StoryManager.instance.stories.Count; i++)
        {
            if (i < unlockCount)
            {
                StoryManager.instance.stories[i].isSeen = true;
            }
            else
            {
                StoryManager.instance.stories[i].isSeen = false;
            }
        }
    }


    public void UpdateSeenStory()
    {
        UnlockStoriesByLevel();
        foreach (var obj in storyLibaries)
        {
            var hiddenObject = StoryManager.instance.GetByStoryId(obj.Id);
            if (hiddenObject.isSeen)
            {
                obj.SetSeenObject(true);
                obj.UpdateVisibility();
                obj.GetComponent<Button>().onClick.RemoveAllListeners();
                obj.GetComponent<Button>().onClick.AddListener(() => SetStoryView(obj.Id));
                obj.GetComponent<Button>().onClick.AddListener(() => SoundManager.instance.PlaySFX("Click Sound"));

            }
            else
            {
                obj.GetComponent<Button>().onClick.RemoveAllListeners();
                obj.GetComponent<Button>().onClick.AddListener(() => SoundManager.instance.PlaySFX("Click Sound"));
                obj.GetComponent<Button>().onClick.AddListener(() =>
                NotiManager.instance.ShowNotification("Unlock after completing each 50 stage"));
            }
        }
    }
    public void SetStoryView(string id)
    {
        this.storyView.gameObject.SetActive(true);
        storyView.SetData(id);
        objectParent.gameObject.SetActive(false);
        BackgroundManager.instance.SetObjectBg(3000);
       ApplyText.instance.UpdateStoryInfo(id);
    }
}
