using NUnit.Framework.Interfaces;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;


public class AchivementUIController : MonoBehaviour
{
    [SerializeField] private AchievementUI questUI;
    [SerializeField] private List<AchievementUI> quests;
    [SerializeField] private Transform content;
    [SerializeField] private Button CompleteAll;
    [SerializeField] private TextMeshProUGUI completedTxt;
    [SerializeField] private int currentCompleteQuest;
    [SerializeField] private TextMeshProUGUI completeText;
    [SerializeField] private GameObject rewardObj;
    [SerializeField] private TextMeshProUGUI rewardTxt;
    [SerializeField] private Sprite unselectSprite;
    [SerializeField] private Sprite selectSprite;
    [SerializeField] private TextMeshProUGUI totalCompletedTxt;
    List<QuestBase> questsToShow;

    void Start()
    {
        Init();
        CompleteAll.onClick.AddListener(() => CompleteQuestReward());
    }


    private void OnEnable()
    {
        if (quests.Count > 0)
        {
            foreach (var q in quests)
            {
                Destroy(q.gameObject);
            }
            quests.Clear();
            Init();
            currentCompleteQuest = 0;
           
        }
    }
    public void Init()
    {
         questsToShow = AchievementManager.instance.GetFirstUncompletedQuestEachGroup();
        for (int i = 0; i < questsToShow.Count; i++)
        {
            AchievementUI quest = Instantiate(questUI, content);
            quest.SetQuestData(questsToShow[i].questId,
                   questsToShow[i].description, questsToShow[i].reward.ToString(),
                 questsToShow[i].GetProgress());
            quests.Add(quest);
            //if (!AchievementManager.instance.activeQuests[i].isReward)
            //{
            //    AchievementUI quest = Instantiate(questUI, content);
            //    quest.SetQuestData(AchievementManager.instance.activeQuests[i].questId,
            //        AchievementManager.instance.activeQuests[i].description, AchievementManager.instance.activeQuests[i].reward.ToString(),
            //        AchievementManager.instance.activeQuests[i].GetProgress());
            //    quests.Add(quest);
            //}
        }
    }
    private void FixedUpdate()
    {
        totalCompletedTxt.text= "Total completed achievement: "+AchievementManager.instance.GetTotalComplete();
        int currentComplete = questsToShow.Where(h => h.isReward).Count();
        if (currentComplete >= AchievementManager.instance.activeQuests.Count)
        {
            completedTxt.gameObject.SetActive(true);
        }
        else
        {
            completedTxt.gameObject.SetActive(false);
        }
        currentCompleteQuest = questsToShow.Where(h => h.CheckCompletion()).Count();

        if (currentCompleteQuest == 0)
        {
            CompleteAll.GetComponent<Image>().sprite = unselectSprite;
            CompleteAll.interactable = false;
        }
        else
        {
            CompleteAll.GetComponent<Image>().sprite = selectSprite;
            CompleteAll.interactable = true;
            CompleteAll.gameObject.SetActive(true);
        }
        if (quests.Count <= 0)
        {
            CompleteAll.gameObject.SetActive(false);
        }
        else
        {
            CompleteAll.gameObject.SetActive(true);
        }


    }
    public void CompleteQuestReward()
    {
        int reward = 0;
        List<AchievementUI> completedQuests = new List<AchievementUI>(); // Danh sách tạm để lưu các quest hoàn thành

        foreach (AchievementUI quest in quests)
        {
            QuestBase questBase = AchievementManager.instance.GetQuestById(quest.QuestID);
            if (questBase.CheckCompletion())
            {
                GoldManager.instance.AddGold(questBase.reward);
                reward += questBase.reward;
                questBase.isReward = true;
                questBase.SaveQuest();
                completedQuests.Add(quest); // Đánh dấu để xóa sau
            }
        }
        rewardObj.SetActive(true);
        rewardTxt.text = reward.ToString();
        StartCoroutine(ShowRewardTemporarily());

        // Xóa các quest đã hoàn thành khỏi danh sách và UI
        foreach (AchievementUI quest in completedQuests)
        {
            quests.Remove(quest);
            Destroy(quest.gameObject); // Xóa UI của quest
        }
        foreach (var q in quests)
        {
            Destroy(q.gameObject);
        }
        quests.Clear();
        Init();
        currentCompleteQuest = 0; // Reset lại số quest hoàn thành
        content.GetComponent<RectTransform>().anchoredPosition = new Vector2(0, 0);
    }

    private IEnumerator ShowRewardTemporarily()
    {
        rewardObj.SetActive(true);
        yield return new WaitForSeconds(1f);
        rewardObj.SetActive(false);
    }
}
