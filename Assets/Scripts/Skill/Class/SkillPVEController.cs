using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class SkillPVEController
{
    private List<SkillData> skillsDataPVE;
    private int currentIDHero;
    private GridController gridController;

    public SkillPVEController(GridController gridController)
    {
        this.gridController = gridController;
        skillsDataPVE = new List<SkillData>();
        InitSkill();
    }

    private void InitSkill()
    {
        AddSkillPVE(1001, new ItsuiSkill(gridController, 1, 1001));
        AddSkillPVE(1002, new NigoutoSkill(gridController, 1, 1002));
        AddSkillPVE(1003, new SkulzSkill(gridController, 1, 1003));
        AddSkillPVE(1004, new AkaleSkill(gridController, 1, 1004));
    }

    public void LoadSkill()
    {
        GetSkillPVEById(currentIDHero)?.SetNumberOfSkill(1);
    }

    public void AddSkillPVE(int id, ISkill skill)
    {
        skillsDataPVE.Add(new SkillData(id, skill));
    }

    public ISkill GetSkillPVEById(int id)
    {
        var skillData = skillsDataPVE.FirstOrDefault(s => s.id == id);
        if (skillData.skill == null)
            Debug.LogWarning("Không tìm thấy kỹ năng PVE cho ID: " + id);
        return skillData.skill;
    }

    public void SetSkillId(int id)
    {
        if (skillsDataPVE.Any(skillData => skillData.id == id))
            currentIDHero = id;
        else
            Debug.LogWarning($"Skill ID {id} không tồn tại trong danh sách PVE.");
    }
    public List<SkillData> GetAllSkill()
    {
        return this.skillsDataPVE;
    }
    public void ActiveSkillPVE()
    {
        if (GameManager.instance.isEnd) return;
        GetSkillPVEById(currentIDHero)?.ActivateSkill();

    }

    public void IncreaseSkillUsesForCurrentHero(int amount)
    {
        GetSkillPVEById(currentIDHero)?.IncreaseUses(amount);
    }

    public ISkill GetCurrentSkill()
    {
        return GetSkillPVEById(currentIDHero);
    }

}