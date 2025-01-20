using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;

public class SkillManager : MonoBehaviour
{
    [SerializeField] private List<SkillData> skillsData;
    [SerializeField] private int currentIDHero;
    [SerializeField] private GridController gridController;
    public static SkillManager instance;
    private void Awake()
    {
        instance = this;
        AddSkill(1001, new GeatsSkill(gridController, 1, 1001));
        AddSkill(1002, new GavvSkill(gridController, 1, 1002));

    }

    void Start()
    {
       
    }
    public void LoadSkill()
    {
        GetSkillById(currentIDHero).SetNumberOfSkill(1);
    }

    public void AddSkill(int id, ISkill skill)
    {
        skillsData.Add(new SkillData(id, skill));
    }
    public ISkill GetSkillById(int id)
    {
        foreach (SkillData skillData in skillsData)
        {
            if (skillData.id == id)
            {
                return skillData.skill;
            }
        }
        Debug.LogWarning("Không tìm thấy kỹ năng cho ID: " + id);
        return null;
    }
    public void SetSkillId(int id)
    {
        if (skillsData.Any(skillData => skillData.id == id)) 
        {
            this.currentIDHero = id;
        }
        else
        {
            
            Console.WriteLine($"Skill ID {id} không tồn tại trong danh sách.");
        }
    }

    public void ActiveSkill()
    {
        if (GameManager.instance.isEnd)
            return;
        ISkill skill= GetSkillById(currentIDHero);
        skill.ActivateSkill();
    }
    public void IncreaseSkillUsesForCurrentHero(int amount)
    {
        ISkill skill = GetSkillById(currentIDHero);
        if (skill != null)
        {
            skill.IncreaseUses(amount); 
        }
    }
    public ISkill GetCurrentSkill()
    {
        return GetSkillById(currentIDHero); 
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
