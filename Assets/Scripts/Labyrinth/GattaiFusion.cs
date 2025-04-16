using System.Collections.Generic;
using Unity.VisualScripting.Antlr3.Runtime.Misc;
using UnityEngine;
using UnityEngine.UI;

public class GattaiFusionUI : MonoBehaviour
{
    public List<int> selectedHeroes = new List<int>();
    public SkillFushionButton skillButtonPrefab;
    public Transform skillButtonContainer;
    public Button okButton;
    public HeroFusionButton heroButtonPrefab;
    public Transform heroButtonContainer;
    private int selectedSkillIndex = -1;
    private List<ISkill> availableSkills = new List<ISkill>();
    public List<HeroFusionButton> heros;
    public List<SkillFushionButton> skills;
    private void Start()
    {
        foreach (var heroData in HeroManager.instance.heroDatas)
        {
            HeroFusionButton button = Instantiate(heroButtonPrefab, heroButtonContainer);
            button.SetData(heroData.id, heroData.icon, OnClickHeroButton);
            heros.Add(button);

        }
        okButton.onClick.AddListener(() => OnClickOKFusion());
        
    }
    public void OnClickHeroButton(int heroID)
    {
        if (!selectedHeroes.Contains(heroID))
        {
            selectedHeroes.Add(heroID);
        }
        else
        {
            selectedHeroes.Remove(heroID);
        }
        foreach (var btn in heros)
        {
            bool isSelected = selectedHeroes.Contains(btn.GetID());
            btn.SetHighlight(isSelected);
        }
        if (selectedHeroes.Count >= 3)
        {
            ShowSkillSelection();
        }
        else
        {
            ClearSkillSelection();
        }
    }

    void ShowSkillSelection()
    {
        ClearSkillSelection();
        availableSkills.Clear();

        foreach (int id in selectedHeroes)
        {
            ISkill skill = SkillManager.instance.GetSkillPVEById(id);
            if (skill != null)
            {
                availableSkills.Add(skill);
                SkillFushionButton skillBtn = Instantiate(skillButtonPrefab, skillButtonContainer);
                skillBtn.SetData(availableSkills.Count - 1, HeroManager.instance.GetHero(id).Value.name, OnClickSkillButton);
                skills.Add(skillBtn);
            }
        }

        okButton.gameObject.SetActive(true);
    }

    void ClearSkillSelection()
    {
        foreach (Transform child in skillButtonContainer)
        {
            Destroy(child.gameObject);
        }
        okButton.gameObject.SetActive(false);
        selectedSkillIndex = -1;
    }

    public void OnClickSkillButton(int index)
    {
        if (selectedSkillIndex == index)
        {
            // Click lần nữa để bỏ chọn
            Debug.Log("Bỏ chọn skill: " + availableSkills[index].GetSkillId());
            selectedSkillIndex = -1;
        }
        else
        {
            // Chọn mới
            selectedSkillIndex = index;
            Debug.Log("Skill được chọn: " + availableSkills[index].GetSkillId());
        }

        for (int i = 0; i < skills.Count; i++)
        {
            skills[i].SetHighlight(i == selectedSkillIndex);
        }
        
    }

    public void OnClickOKFusion()
    {
        if (selectedSkillIndex < 0) return;

        int totalHP = 0;
        int totalId = 0;
        foreach (var heroID in selectedHeroes)
        {
            totalHP += HeroManager.instance.GetHero(heroID).Value.hp;
            totalId += HeroManager.instance.GetHero(heroID).Value.id;
        }

        ISkill selectedSkill = availableSkills[selectedSkillIndex];
        SkillManager.instance.AddSkillPVE(totalId, selectedSkill);
        SkillManager.instance.SetSkillId(totalId);

        Debug.Log("Fusion thành công! HP: " + totalHP + " | Skill: " + selectedSkill.GetSkillId());

        // Reset lại
       // selectedHeroes.Clear();
        //ClearSkillSelection();
    }
}
