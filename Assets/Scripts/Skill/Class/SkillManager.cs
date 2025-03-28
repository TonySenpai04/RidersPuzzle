using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;

public class SkillManager : MonoBehaviour
{
    //[SerializeField] private List<SkillData> skillsDataPVE;
    //[SerializeField] private List<SkillData> skillsDataPVP;
    [SerializeField] private int currentIDHero;
    [SerializeField] private GridController gridController;
    public static SkillManager instance;
    public SkillPVEController skillPVEController;
    public SkillPVPController skillPVPController;
    private void Awake()
    {
        instance = this;
        InitSkill();


    }
    public void InitSkill()
    {
        skillPVEController = new SkillPVEController(gridController);
        skillPVPController = new SkillPVPController(gridController);
    }

    void Start()
    {
       
    }
    public void LoadSkillPVE()
    {
        GetSkillPVEById(currentIDHero).SetNumberOfSkill(1);
    }

    public void AddSkillPVE(int id, ISkill skill)
    {
        skillPVEController.AddSkillPVE(id, skill);
    }
    public void AddSkillPVP(int id, ISkill skill)
    {
        skillPVPController.AddSkillPVP(id, skill);
    }
    public ISkill GetSkillPVEById(int id)
    {
        return skillPVEController.GetSkillPVEById(id);
    }
    public void SetSkillId(int id)
    {
        skillPVEController.SetSkillId(id);
        currentIDHero = id;

    }

    public void ActiveSkillPVE()
    {
        if (skillPVEController.GetCurrentSkill().GetNumberOfSkill() <= 0)
            return;
        skillPVEController.ActiveSkillPVE();
        foreach (var quest in QuestManager.instance.GetQuestsByType<UseRiderSkillQuest>())
        {
            QuestManager.instance.UpdateQuest(quest.questId, 1, 0);
        }
    }
    public void IncreaseSkillUsesForCurrentHero(int amount)
    {
        skillPVEController.IncreaseSkillUsesForCurrentHero(amount);
    }
    public ISkill GetCurrentSkill()
    {
        return skillPVEController.GetCurrentSkill();
    }
}
[Serializable]
public struct SkillData {
   public int id;
   public ISkill skill;
    public SkillData(int id, ISkill skill)
    {
        this.id = id;
        this.skill = skill;
    }
}
