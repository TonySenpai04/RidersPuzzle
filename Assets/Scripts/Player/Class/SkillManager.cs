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
    }

    void Start()
    {
        AddSkill(1001, new GeatsSkill(gridController,1));
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
        if (skillsData.Any(skillData => skillData.id == id)) // Giả sử SkillData có thuộc tính Id
        {
            this.currentIDHero = id;
        }
        else
        {
            // Optional: In ra log nếu muốn biết id không tồn tại
            Console.WriteLine($"Skill ID {id} không tồn tại trong danh sách.");
        }
    }
    public void ActiveSkill()
    {
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
