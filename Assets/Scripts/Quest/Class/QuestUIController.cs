using NUnit.Framework.Interfaces;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class QuestUIController : MonoBehaviour
{
    [SerializeField] private QuestUI questUI;
    [SerializeField] private List<QuestUI> quests;
    [SerializeField] private Transform content;
    [SerializeField] private Button CompleteAll;
    [SerializeField] private TextMeshProUGUI completedTxt;
    [SerializeField] private int currentCompleteQuest;
    [SerializeField] private int stampCount ;
    [SerializeField] private List<Image> completedImages;
    [SerializeField] private Sprite stampImage;
    [SerializeField] private TextMeshProUGUI timerText;
    [SerializeField] private GameObject rewardObj;
    [SerializeField] private TextMeshProUGUI rewardTxt;
    [SerializeField] private Sprite unselectSprite;
    [SerializeField] private Sprite selectSprite;
    void Start()
     {
        Init();
        CompleteAll.onClick.AddListener(() => CompleteQuestReward());
        GetQuest7Day();

    }
    public async void GetQuest7Day()
    {
        QuestData questData = await QuestManager.instance.LoadQuestData();

        stampCount = questData.stampCount;
        if (stampCount >= 7)
        {

            int reward = 888;
            rewardObj.SetActive(true);
            rewardTxt.text = reward.ToString();
            GoldManager.instance.AddGold(reward);
            StartCoroutine(ShowRewardTemporarily());
            questData.stampCount = 0;
            QuestManager.instance.SaveQuestData(questData);
            UpdateStampUI();

        }
    }
    private void UpdateTimer()
    {
        
            TimeSpan timeLeft = TimeManager.Instance.GetTimeUntilMidnight();
        string localize = LocalizationManager.instance.GetLocalizedText("quest_reset_time");
            timerText.text = $"{localize} {timeLeft.Hours:D2}:{timeLeft.Minutes:D2}:{timeLeft.Seconds:D2}";
       
    }
    private void Update()
    {
        UpdateTimer();
        
    }
    private async void UpdateStampUI()
    {
        QuestData questData = await QuestManager.instance.LoadQuestData();
        stampCount = questData.stampCount;
        for (int i = 0; i < completedImages.Count; i++)
        {
            if (i < stampCount)
            {
                completedImages[i].sprite = stampImage;
                completedImages[i].gameObject.SetActive(true);
            }
            else
            {
                completedImages[i].gameObject.SetActive(false);
            }
        }
    }

    public void Init()
    {
        for (int i = 0; i < QuestManager.instance.activeQuests.Count; i++)
        {
            if (!QuestManager.instance.activeQuests[i].isReward)
            {
                QuestUI quest = Instantiate(questUI, content);
                quest.SetQuestData(QuestManager.instance.activeQuests[i].questId,
                    QuestManager.instance.activeQuests[i].description, QuestManager.instance.activeQuests[i].reward.ToString(),
                    QuestManager.instance.activeQuests[i].GetProgress());
                quests.Add(quest);
            }
        }
    }
    private void FixedUpdate()
    {
        int currentComplete = QuestManager.instance.activeQuests.Where(h => h.isReward).Count();
        if (currentComplete>= QuestManager.instance.activeQuests.Count)
        {
            completedTxt.gameObject.SetActive(true);
        }
        else
        {
            completedTxt.gameObject.SetActive(false);   
        }
        foreach(var quest in quests)
        {
            if(QuestManager.instance.GetQuestById(quest.QuestID).CheckCompletion())
            {
                currentCompleteQuest += 1;
            }
        }
        if (currentCompleteQuest == 0)
        {
            CompleteAll.GetComponent<Image>().sprite= unselectSprite;
            CompleteAll.interactable = false;
            NotiManager.instance.ClearNotiRedDot("quest");
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
    
        bool hasQuestDot = NotiManager.instance.IsRedDotActive("achievement");
        bool hasAchievementDot = NotiManager.instance.IsRedDotActive("quest");

        bool shouldShowLibraryDot =  hasQuestDot || hasAchievementDot;

        if (shouldShowLibraryDot)
        {
            NotiManager.instance.ShowNotiRedDot("questcomplete");
        }
        else
        {
            NotiManager.instance.ClearNotiRedDot("questcomplete");
        }


    }
    public void CompleteQuestReward()
    {
        int reward = 0;
        List<QuestUI> completedQuests = new List<QuestUI>(); // Danh sách tạm để lưu các quest hoàn thành

        foreach (QuestUI quest in quests)
        {
            QuestBase questBase = QuestManager.instance.GetQuestById(quest.QuestID);
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
        rewardTxt.text= reward.ToString();
        StartCoroutine(ShowRewardTemporarily());
        NotiManager.instance.ClearNotiRedDot("quest");
        // Xóa các quest đã hoàn thành khỏi danh sách và UI
        foreach (QuestUI quest in completedQuests)
        {
            quests.Remove(quest);
            Destroy(quest.gameObject); // Xóa UI của quest
        }
        GetQuest7Day();
        currentCompleteQuest = 0; // Reset lại số quest hoàn thành
    }
    private IEnumerator ShowRewardTemporarily()
    {
        rewardObj.SetActive(true);
        yield return new WaitForSeconds(3f);
        rewardObj.SetActive(false);
    }
    private void OnEnable()
    {
        GetQuest7Day();
        UpdateStampUI();
        if (QuestManager.instance.isShowNoti)
        {
            NotiManager.instance.ShowNotification(LocalizationManager.instance.GetLocalizedText("quest_stamp_earn_tip"));
            QuestManager.instance.isShowNoti = false;
        }
    }
}
