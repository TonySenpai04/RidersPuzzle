using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class LibraryController : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI heroTxt;
    [SerializeField] private TextMeshProUGUI objectTxt;
    [SerializeField] private TextMeshProUGUI storyTxt;
    [SerializeField] private GameObject menu;
    [SerializeField] private GameObject riderLibrary;
    [SerializeField] private GameObject objectLibrary;
    [SerializeField] private GameObject riderView;
    [SerializeField] private GameObject toturialView;
    [SerializeField] private GameObject objectview;
    [SerializeField] private GameObject storyLibrary;
    [SerializeField] private GameObject storyview;
    [SerializeField] private HeroLibraryController heroLibraryController;
    private void Start()
    {
        heroTxt.text = HeroManager.instance.HeroOwnedQuantity()+"/13";
        objectTxt.text = HiddenObjectManager.instance.GetSeenObject().Count
            + "/" + HiddenObjectManager.instance.ObjectQuantity();
         int unlockCount = LevelManager.instance.GetAllLevelComplete() / 30;
        storyTxt.text = unlockCount.ToString()+"/"+StoryManager.instance.stories.Count.ToString();
    }
    private void OnEnable()
    {
        menu.SetActive(true);
        storyLibrary.SetActive(false);
        storyview.SetActive(false);
        riderLibrary.SetActive(false);
        objectLibrary.SetActive(false);
        objectview.SetActive(false);
        toturialView.SetActive(false);
        riderView.SetActive(false);
        heroTxt.text = HeroManager.instance.HeroOwnedQuantity() + "/13" ;
        objectTxt.text = HiddenObjectManager.instance.GetSeenObject().Count
            + "/" + HiddenObjectManager.instance.ObjectQuantity();
        int unlockCount = LevelManager.instance.GetAllLevelComplete() / 30;
        storyTxt.text = unlockCount.ToString() + "/" + StoryManager.instance.stories.Count.ToString();
    }
}
