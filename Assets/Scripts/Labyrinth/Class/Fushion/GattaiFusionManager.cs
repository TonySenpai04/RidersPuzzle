using System.Collections.Generic;
using Unity.VisualScripting;
using Unity.VisualScripting.Antlr3.Runtime.Misc;
using UnityEngine;
using UnityEngine.UI;

public class GattaiFusionManager : MonoBehaviour
{
    [SerializeField] private SkillFushionButton skillButtonPrefab;
    [SerializeField] private Transform skillButtonContainer;
    [SerializeField] private Button heroConfirmButton;
    [SerializeField] private HeroFusionButton heroButtonPrefab;
    [SerializeField] private Transform heroButtonContainer;
    [SerializeField] private int selectedSkillIndex = -1;
    [SerializeField] private int totalHP;
    [SerializeField] private int totalId;
    [SerializeField] private GameObject skillsView;
    [SerializeField] private GameObject herosView;
    [SerializeField] private GameObject selectView;
    [SerializeField] private GameObject heroFushion;
    [SerializeField] private Image  newHero;
    [SerializeField] private HeroFushionSlotController fushionSlotController;

    public CardFusionUI cardFusionUI;
    private IHeroSelection heroSelection;
    private ISkillSelection skillSelection;
    private IFusionService fusionService;
    //private void Start()
    //{
    //    newHeroData = new DataHero();
    //    heroConfirmButton.onClick.AddListener(() => ConfirmHeroSelection());
    //    heroConfirmButton.gameObject.SetActive(false);

    //}
    //public void CreateButtons()
    //{
    //    var unlockHeros = HeroManager.instance.GetUnlockHero();
    //    int index = 0;

    //    foreach (var heroData in unlockHeros)
    //    {
    //        if(heroData.currentMP<=0) continue;
    //        HeroFusionButton btn;
    //        if (index < heros.Count)
    //        {
    //            btn = heros[index];
    //            heros[index].SetData(
    //                heroData.id,
    //                heroData.heroCardImage,
    //                heroData.level,
    //                heroData.hp,
    //                heroData.mp,
    //                OnClickHeroButton);
    //            heros[index].gameObject.SetActive(true);
    //        }
    //        else
    //        {
    //            btn = Instantiate(heroButtonPrefab, heroButtonContainer);
    //            btn.SetData(
    //                heroData.id,
    //                heroData.heroCardImage,
    //                heroData.level,
    //                heroData.hp,
    //                heroData.mp,
    //                OnClickHeroButton);
    //            heros.Add(btn);
    //        }
    //        btn.SetHighlight(selectedHeroes.Contains(heroData.id));
    //        index++;
    //    }

    //    for (int i = index; i < heros.Count; i++)
    //    {
    //        heros[i].gameObject.SetActive(false);
    //    }


    //}
    //private void ConfirmHeroSelection()
    //{
    //    selectedHeroes = new List<int>(tempSelectedHeroes);
    //    ShowSkill();
    //}
    //public void ShowSkill()
    //{
    //    if (selectedHeroes.Count >= 3)
    //    {
    //        skillsView.SetActive(true);
    //        ShowSkillSelection();
    //        herosView.SetActive(false );
    //    }

    //}
    //private void OnEnable()
    //{
    //    CreateButtons();
    //    skillsView.SetActive(false);
    //    herosView.SetActive(false);
    //    heroFushion.SetActive(false);
    //    selectView.SetActive(true);

    //}

    //public void OnClickHeroButton(int heroID)
    //{

    //    bool isSelected = tempSelectedHeroes.Contains(heroID);

    //    if (!isSelected)
    //    {
    //        if (tempSelectedHeroes.Count >= 5)
    //        {
    //            Debug.Log("Chỉ được chọn tối đa 5 Rider.");
    //            return;
    //        }
    //        tempSelectedHeroes.Add(heroID);
    //    }
    //    else
    //    {
    //        tempSelectedHeroes.Remove(heroID);
    //    }
    //    SoundManager.instance.PlaySFX("Click Sound");
    //    foreach (var btn in heros)
    //    {
    //        bool selected = tempSelectedHeroes.Contains(btn.GetID());
    //        btn.SetHighlight(selected);
    //    }

    //    heroConfirmButton.gameObject.SetActive(tempSelectedHeroes.Count >= 3);
    //}


    //void ShowSkillSelection()
    //{
    //    ClearSkillSelection();
    //    availableSkills.Clear();

    //    for (int i = 0; i < 5; i++)
    //    {
    //        SkillFushionButton skillBtn = Instantiate(skillButtonPrefab, skillButtonContainer);
    //        if (i < selectedHeroes.Count)
    //        {
    //            int id = selectedHeroes[i];
    //            ISkill skill = SkillManager.instance.GetSkillPVEById(id);
    //            if (skill != null)
    //            {
    //                availableSkills.Add(skill);
    //                skillBtn.SetData(availableSkills.Count - 1,
    //                    LocalizationManager.instance.GetLocalizedText($"skill_info_hero_{id}"), OnClickSkillButton);
    //            }
    //            else
    //            {
    //                skillBtn.SetData(i, "", null);
    //            }
    //        }
    //        else
    //        {
    //            skillBtn.SetData(i, "", null);
    //        }
    //        skills.Add(skillBtn);
    //    }

    //    heroConfirmButton.gameObject.SetActive(true);
    //}

    //void ClearSkillSelection()
    //{
    //    foreach (Transform child in skillButtonContainer)
    //    {
    //        Destroy(child.gameObject);
    //    }
    //    skills.Clear();
    //    heroConfirmButton.gameObject.SetActive(false);
    //    selectedSkillIndex = -1;
    //}

    //public void OnClickSkillButton(int index)
    //{
    //    if (selectedSkillIndex == index)
    //    {
    //        Debug.Log("Bỏ chọn skill: " + availableSkills[index].GetSkillId());
    //        selectedSkillIndex = -1;
    //    }
    //    else
    //    {
    //        selectedSkillIndex = index;
    //        ApplyTextManager.instance.UpdateSkillInfoOnStage(availableSkills[index].GetSkillId());
    //        Debug.Log("Skill được chọn: " + availableSkills[index].GetSkillId());
    //    }

    //    for (int i = 0; i < skills.Count; i++)
    //    {
    //        skills[i].SetHighlight(i == selectedSkillIndex);
    //    }

    //}

    //public void OnClickOKFusion()
    //{
    //    if (selectedSkillIndex < 0) return;
    //    if (masteryPoints < 1) return;
    //    totalHP = 0;
    //    foreach (var heroID in selectedHeroes)
    //    {
    //        totalHP += HeroManager.instance.GetHero(heroID).Value.hp;
    //        totalId += HeroManager.instance.GetHero(heroID).Value.id;
    //    }

    //    ISkill selectedSkill = availableSkills[selectedSkillIndex];
    //    SkillManager.instance.AddSkillPVE(totalId, selectedSkill);
    //    SkillManager.instance.SetSkillId(totalId);

    //    newHeroData.hp = totalHP;
    //    newHeroData.id = totalId;

    //    //LabyrinthController.instance.SetGataiData(data);
    //    //LabyrinthController.instance.Randomlevel();
    //    //masteryPoints--;
    //    //heroFushion.SetActive(true);
    //   // PlayerController.instance.GetComponent<SpriteRenderer>().sprite = newHero.sprite;
    //    Debug.Log("Fusion thành công! HP: " + totalHP + " | Skill: " + selectedSkill.GetSkillId());

    //    // Reset lại
    //   // selectedHeroes.Clear();
    //    //ClearSkillSelection();
    //}
    //public void OnSelectSkill()
    //{
    //    if(selectedSkillIndex < 0) return;
    //    skillsView.SetActive(false);
    //    selectView.SetActive(true);

    //}
    private void Awake()
    {
        heroSelection = new HeroSelectionController( heroButtonPrefab, heroButtonContainer, heroConfirmButton);
        skillSelection = new SkillSelectionController(skillButtonPrefab, skillButtonContainer);
        fusionService = new FusionService();
    }
    private void Start()
    {
        heroConfirmButton.onClick.AddListener(()=>heroSelection.ConfirmSelection());
        heroConfirmButton.onClick.AddListener(OnConfirmHeroSelection);
    }

    private void OnEnable()
    {
        heroSelection.CreateHeroButtons();
        skillsView.SetActive(false);
        herosView.SetActive(false);
        heroFushion.SetActive(false);
        selectView.SetActive(true);
        fushionSlotController.UpdateSlotState();
    }

    private void OnConfirmHeroSelection()
    {
        List<int> selectedHeroes = heroSelection.GetSelectedHeroes();
        Debug.Log(selectedHeroes.Count);
        if (selectedHeroes.Count >= 3)
        {
            skillsView.SetActive(true);
            skillSelection.CreateSkillButtons(selectedHeroes);
            herosView.SetActive(false);
        }
     
    }

    public void OnClickOKFusion()
    {
        ISkill selectedSkill = skillSelection.GetSelectedSkill();
        if (selectedSkill == null)
        {
            NotiManager.instance.ShowNotification("Please select skill");
            return;
        }

        var fusedHero = fusionService.FuseHeroes(heroSelection.GetSelectedHeroes(), selectedSkill);
        cardFusionUI.StartFusion();
        Debug.Log($"Fusion thành công! HP: {fusedHero.hp} | Skill: {selectedSkill.GetSkillId()}");
    }
    public void OnSelectSkill()
    {
        if (skillSelection.GetSelectedSkill() == null)
            return;

        skillsView.SetActive(false);
        selectView.SetActive(true);
    }
    public List<int> GetSelectedHeroes() => new List<int>(heroSelection.GetSelectedHeroes());
}
