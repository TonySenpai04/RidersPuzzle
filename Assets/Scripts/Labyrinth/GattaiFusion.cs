using System.Collections.Generic;
using Unity.VisualScripting.Antlr3.Runtime.Misc;
using UnityEngine;
using UnityEngine.UI;

public class GattaiFusionUI : MonoBehaviour
{
    public List<int> selectedHeroes = new List<int>();
    public SkillFushionButton skillButtonPrefab;
    public Transform skillButtonContainer;
    public Button heroConfirmButton;
    public HeroFusionButton heroButtonPrefab;
    public Transform heroButtonContainer;
    private int selectedSkillIndex = -1;
    private List<ISkill> availableSkills = new List<ISkill>();
    public List<HeroFusionButton> heros;
    public List<SkillFushionButton> skills;
    public int totalHP;
    public int totalId;
    public int masteryPoints=5;
    public GameObject skillsView;
    public GameObject herosView;
    private void Start()
    {
        //foreach (var heroData in HeroManager.instance.GetUnlockHero())
        //{
        //    HeroFusionButton button = Instantiate(heroButtonPrefab, heroButtonContainer);
        //    button.SetData(heroData.id, heroData.heroCardImage,heroData.level,heroData.hp,heroData.mp, OnClickHeroButton);
        //    heros.Add(button);

        //}
        heroConfirmButton.onClick.AddListener(() => ShowSkill());
        heroConfirmButton.gameObject.SetActive(false);

    }
    public void CreateButtons()
    {
        var unlockHeros = HeroManager.instance.GetUnlockHero();
        int index = 0;

        foreach (var heroData in unlockHeros)
        {
            if (index < heros.Count)
            {
                heros[index].SetData(
                    heroData.id,
                    heroData.heroCardImage,
                    heroData.level,
                    heroData.hp,
                    heroData.mp,
                    OnClickHeroButton);
                heros[index].gameObject.SetActive(true);
            }
            else
            {
                var button = Instantiate(heroButtonPrefab, heroButtonContainer);
                button.SetData(
                    heroData.id,
                    heroData.heroCardImage,
                    heroData.level,
                    heroData.hp,
                    heroData.mp,
                    OnClickHeroButton);
                heros.Add(button);
            }
            index++;
        }

        for (int i = index; i < heros.Count; i++)
        {
            heros[i].gameObject.SetActive(false);
        }


    }
    public void ShowSkill()
    {
        if (selectedHeroes.Count >= 3)
        {
            skillsView.SetActive(true);
            ShowSkillSelection();
            herosView.SetActive(false );
        }
        
    }
    private void OnEnable()
    {
        CreateButtons();
    }

    public void OnClickHeroButton(int heroID)
    {
        bool isSelected = selectedHeroes.Contains(heroID);

        if (!isSelected)
        {

            if (selectedHeroes.Count >= 5)
            {
                Debug.Log("Chỉ được chọn tối đa 5 Rider.");
                return;
            }
            selectedHeroes.Add(heroID);
        }
        else
        {
            selectedHeroes.Remove(heroID);
        }

        foreach (var btn in heros)
        {
            bool selected = selectedHeroes.Contains(btn.GetID());
            btn.SetHighlight(selected);
        }

        if (selectedHeroes.Count >= 3)
        {
            heroConfirmButton.gameObject.SetActive(true);
        }
        else
        {
            heroConfirmButton.gameObject.SetActive(false);
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
                skillBtn.SetData(availableSkills.Count - 1,
                    LocalizationManager.instance.GetLocalizedText($"skill_info_hero_{id}"), OnClickSkillButton);
                skills.Add(skillBtn);
            }
        }

        heroConfirmButton.gameObject.SetActive(true);
    }

    void ClearSkillSelection()
    {
        foreach (Transform child in skillButtonContainer)
        {
            Destroy(child.gameObject);
        }
        heroConfirmButton.gameObject.SetActive(false);
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
            ApplyTextManager.instance.UpdateSkillInfoOnStage(availableSkills[index].GetSkillId());
          //  ApplyText.instance.UpdateSkillInfoOnStage(availableSkills[index].GetSkillId());
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
        if (masteryPoints < 1) return;
        foreach (var heroID in selectedHeroes)
        {
            totalHP += HeroManager.instance.GetHero(heroID).Value.hp;
            totalId += HeroManager.instance.GetHero(heroID).Value.id;
        }

        ISkill selectedSkill = availableSkills[selectedSkillIndex];
        SkillManager.instance.AddSkillPVE(totalId, selectedSkill);
        SkillManager.instance.SetSkillId(totalId);
        DataHero data=new DataHero();
        data.hp = totalHP;
        data.id = totalId;

        LabyrinthController.instance.SetGataiData(data);
        LabyrinthController.instance.Randomlevel();
        masteryPoints--;
        Debug.Log("Fusion thành công! HP: " + totalHP + " | Skill: " + selectedSkill.GetSkillId());

        // Reset lại
       // selectedHeroes.Clear();
        //ClearSkillSelection();
    }
}
