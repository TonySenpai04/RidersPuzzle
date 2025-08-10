using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class SkillSelectionController : ISkillSelection
{
    private List<ISkill> availableSkills = new List<ISkill>();
    private List<SkillFushionButton> skillButtons = new List<SkillFushionButton>();
    private SkillFushionButton skillButtonPrefab;
    private Transform skillButtonContainer;

    private int selectedSkillIndex = -1;
    private int savedSkillIndex = -1;

    private List<ISkill> savedAvailableSkills = new List<ISkill>();
    private List<int> lastHeroIds = new List<int>(); // để so sánh

    public SkillSelectionController(SkillFushionButton prefab, Transform container)
    {
        skillButtonPrefab = prefab;
        skillButtonContainer = container;
    }

    public void CreateSkillButtons(List<int> heroIds)
    {
        bool sameHeroes = heroIds.Count == lastHeroIds.Count && !heroIds.Except(lastHeroIds).Any();

        // Nếu danh sách hero thay đổi
        if (!sameHeroes)
        {
            lastHeroIds = new List<int>(heroIds);

            // Lấy ID skill đang chọn để kiểm tra có còn tồn tại không
            int selectedSkillId = (savedSkillIndex >= 0 && savedSkillIndex < savedAvailableSkills.Count)
                ? savedAvailableSkills[savedSkillIndex].GetSkillId()
                : -1;

            // Clear để tạo mới UI
            ClearSelection();
            availableSkills.Clear();

            for (int i = 0; i < 5; i++)
            {
                SkillFushionButton skillBtn = GameObject.Instantiate(skillButtonPrefab, skillButtonContainer);

                if (i < heroIds.Count)
                {
                    int id = heroIds[i];
                    ISkill skill = SkillManager.instance.GetSkillPVEById(id);

                    if (skill != null)
                    {
                        availableSkills.Add(skill);
                        skillBtn.SetData(
                            availableSkills.Count - 1,
                            LocalizationManager.instance.GetLocalizedText($"skill_info_hero_{id}"),
                            SelectSkill
                        );
                    }
                    else
                    {
                        skillBtn.SetData(i, "", null);
                    }
                }
                else
                {
                    skillBtn.SetData(i, "", null);
                }

                skillButtons.Add(skillBtn);
            }

            // Lưu lại skill list mới
            savedAvailableSkills = new List<ISkill>(availableSkills);

            // Khôi phục selection nếu skill cũ vẫn tồn tại
            savedSkillIndex = availableSkills.FindIndex(s => s.GetSkillId() == selectedSkillId);
            selectedSkillIndex = savedSkillIndex;

            // Highlight lại nếu có
            for (int i = 0; i < skillButtons.Count; i++)
            {
                skillButtons[i].SetHighlight(i == selectedSkillIndex);
            }
        }
        else
        {
            // Hero không đổi → khôi phục
            RestoreSavedSkills();
        }
    }

    private void RestoreSavedSkills()
    {
        ClearSelection();
        availableSkills = new List<ISkill>(savedAvailableSkills);

        for (int i = 0; i < 5; i++)
        {
            SkillFushionButton skillBtn = GameObject.Instantiate(skillButtonPrefab, skillButtonContainer);

            if (i < availableSkills.Count)
            {
                ISkill skill = availableSkills[i];
                skillBtn.SetData(i, LocalizationManager.instance.GetLocalizedText($"skill_info_hero_{skill.GetSkillId()}"),
                    SelectSkill);
            }
            else
            {
                skillBtn.SetData(i, "", null);
            }

            skillButtons.Add(skillBtn);
        }

        selectedSkillIndex = savedSkillIndex;
        for (int i = 0; i < skillButtons.Count; i++)
        {
            skillButtons[i].SetHighlight(i == selectedSkillIndex);
        }
    }

    public void SelectSkill(int index)
    {
        if (selectedSkillIndex == index)
            selectedSkillIndex = -1;
        else
            selectedSkillIndex = index;

        savedSkillIndex = selectedSkillIndex;

        for (int i = 0; i < skillButtons.Count; i++)
        {
            skillButtons[i].SetHighlight(i == selectedSkillIndex);
        }
    }

    public ISkill GetSelectedSkill()
    {
        if (selectedSkillIndex < 0 || selectedSkillIndex >= availableSkills.Count) return null;
        return availableSkills[selectedSkillIndex];
    }

    public void ClearSelection()
    {
        foreach (Transform child in skillButtonContainer)
        {
            GameObject.Destroy(child.gameObject);
        }
        skillButtons.Clear();
        selectedSkillIndex = -1;
    }
}
