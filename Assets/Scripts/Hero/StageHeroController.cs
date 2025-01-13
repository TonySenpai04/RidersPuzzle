using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;



public class StageHeroController : MonoBehaviour
{

    [SerializeField] private ButtonHero heroButtonPrefab;
    [SerializeField] private Transform buttonParent;
    [SerializeField] private GameObject stageChracter;
    [SerializeField] private GameObject playZone;
    [SerializeField] private SkillManager skillManager;
    [SerializeField] private HeroManager heroManager;
    [SerializeField] private int currentId;
    [SerializeField] private PlayerController playerController;
    [SerializeField] private List<ButtonHero> heroButtons = new List<ButtonHero>();
    void Start()
    {
        CreateButtons();
    }

    private void CreateButtons()
    {
        foreach (var heroData in heroManager.heroDatas)
        {
            ButtonHero button = Instantiate(heroButtonPrefab, buttonParent);
            button.Initialize(
                heroData.id,
                SetHeroID,
                heroData.isUnlock,
                false , heroData.icon
            );

            heroButtons.Add(button);
        }
 
    }
    void FixedUpdate()
    {
        UpdateButtonStates();
    }

    private void UpdateButtonStates()
    {
        for (int i = 0; i < heroButtons.Count; i++)
        {
            bool isUnlockedInManager = heroManager.heroDatas[i].isUnlock;
            if (heroButtons[i].isUnlocked != isUnlockedInManager)
            {
                heroButtons[i].UpdateButtonState(isUnlockedInManager, heroButtons[i].Index == currentId);
            }
        }
    }
    public void SetHeroID(int id)
    {
        var clickedButton = heroButtons.Find(button => button.Index == id);

        if (clickedButton != null && !clickedButton.isUnlocked)
        {
            Debug.LogWarning($"Hero với ID {id} đang bị khóa, không thể chọn.");
            return;
        }
        skillManager.SetSkillId(id);
        currentId = id;
        foreach (var button in heroButtons)
        {
            bool isSelected = button.Index == id;
            button.UpdateButtonState(button.isUnlocked, isSelected);
        }
        SoundManager.instance.PlayHeroSFX(id);
    }

    public void LoadLevel()
    {
        if (isHero())
        {
            foreach (DataHero hero in heroManager.heroDatas)
            {
                if (hero.id == currentId)
                {
                    playerController.SetCurrentData(hero);
                    break;
                }

            }
        }
    }
    public bool isHero()
    {
        return heroManager.heroDatas.Any(hero => hero.id == currentId && hero.isUnlock);
    }
    public DataHero GetCurrentHeroData()
    {
        DataHero currentHero=new DataHero();
        foreach (DataHero hero in heroManager.heroDatas)
        {
            if (hero.id == currentId)
            {
                currentHero= hero;
            }

        }
        return currentHero;
    }
}
