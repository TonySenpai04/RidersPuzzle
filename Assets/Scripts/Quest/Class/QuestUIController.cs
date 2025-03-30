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

    void Start()
     {
        Init();
        CompleteAll.onClick.AddListener(() => CompleteQuestReward());
        QuestData questData = QuestManager.instance.LoadQuestData();
        stampCount=questData.stampCount;
     }
    private void UpdateTimer()
    {
        
            TimeSpan timeLeft = TimeManager.Instance.GetTimeUntilMidnight();
            timerText.text = $"Daily quest reset in: {timeLeft.Hours:D2}:{timeLeft.Minutes:D2}:{timeLeft.Seconds:D2}";
       
    }
    private void Update()
    {
        UpdateTimer();
    }
    private void UpdateStampUI()
    {
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
            CompleteAll.gameObject.SetActive(false);
        }
        else
        {
            CompleteAll.gameObject.SetActive(true);
        }
        UpdateStampUI();
    }
    public void CompleteQuestReward()
    {
        List<QuestUI> completedQuests = new List<QuestUI>(); // Danh sách tạm để lưu các quest hoàn thành

        foreach (QuestUI quest in quests)
        {
            QuestBase questBase = QuestManager.instance.GetQuestById(quest.QuestID);
            if (questBase.CheckCompletion())
            {
                GoldManager.instance.AddGold(questBase.reward);
                questBase.isReward = true;
                questBase.SaveQuest();
                completedQuests.Add(quest); // Đánh dấu để xóa sau
            }
        }

        // Xóa các quest đã hoàn thành khỏi danh sách và UI
        foreach (QuestUI quest in completedQuests)
        {
            quests.Remove(quest);
            Destroy(quest.gameObject); // Xóa UI của quest
        }

        currentCompleteQuest = 0; // Reset lại số quest hoàn thành
    }

}
