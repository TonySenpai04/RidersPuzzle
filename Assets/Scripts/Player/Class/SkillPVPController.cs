using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class SkillPVPController
{
    private List<SkillData> skillsDataPVP;
    private int currentIDHero;
    private GridController gridController;

    public SkillPVPController(GridController gridController)
    {
        this.gridController = gridController;
        skillsDataPVP = new List<SkillData>();
        InitSkill();
    }

    private void InitSkill()
    {
        AddSkillPVP(1001, new ItsuiSkill(gridController, 1, 1001));
        AddSkillPVP(1002, new NigoutoSkill(gridController, 1, 1002));
        AddSkillPVP(1003, new SkulzSkill(gridController, 1, 1003));
    }

    public void LoadSkill()
    {
        GetSkillPVPById(currentIDHero)?.SetNumberOfSkill(1);
    }

    public void AddSkillPVP(int id, ISkill skill)
    {
        skillsDataPVP.Add(new SkillData(id, skill));
    }

    public ISkill GetSkillPVPById(int id)
    {
        var skillData = skillsDataPVP.FirstOrDefault(s => s.id == id);
        if (skillData.skill == null)
            Debug.LogWarning("Không tìm thấy kỹ năng PVP cho ID: " + id);
        return skillData.skill;
    }

    public void SetSkillId(int id)
    {
        if (skillsDataPVP.Any(skillData => skillData.id == id))
            currentIDHero = id;
        else
            Debug.LogWarning($"Skill ID {id} không tồn tại trong danh sách PVP.");
    }

    public void ActiveSkillPVP()
    {
        if (GameManager.instance.isEnd) return;
        GetSkillPVPById(currentIDHero)?.ActivateSkill();
    }

    public void IncreaseSkillUsesForCurrentHero(int amount)
    {
        GetSkillPVPById(currentIDHero)?.IncreaseUses(amount);
    }

    public ISkill GetCurrentSkill()
    {
        return GetSkillPVPById(currentIDHero);
    }
    public  List<SkillData>  GetAllSkill()
    {
        return this.skillsDataPVP;
    }
}