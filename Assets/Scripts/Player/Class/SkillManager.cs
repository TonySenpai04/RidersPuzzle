using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class SkillManager : MonoBehaviour
{
    [SerializeField] private List<SkillData> skillsData;
    [SerializeField] private int currentIDHero;
    [SerializeField] private GridController gridController;
   
    void Start()
    {
        AddSkill(1001, new GeatsSkill(gridController));
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
    public void ActiveSkill()
    {
        ISkill skill= GetSkillById(currentIDHero);
        skill.ActivateSkill();
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
