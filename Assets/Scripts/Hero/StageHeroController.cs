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
    void Start()
    {
        CreateButtons();
    }

    private void CreateButtons()
    {
        for (int i = 0; i < heroManager.heroDatas.Count; i++)
        {
            ButtonHero button = Instantiate(heroButtonPrefab, buttonParent);
            button.GetComponent<Image>().sprite = heroManager.heroDatas[i].icon;
            button.Initialize(heroManager.heroDatas[i].id, SetHeroID);
        }
    }
    public void SetHeroID(int id)
    {
        skillManager.SetSkillId(id);
        currentId = id;
    }

    public void LoadLevel()
    {
        if (heroManager.heroDatas.Any(hero => hero.id == currentId))
        {
            playZone.gameObject.SetActive(true);
            foreach (DataHero hero in heroManager.heroDatas)
            {
                if (hero.id == currentId)
                {
                    PlayerController.instance.SetCurrentData(hero);
                    break;
                }

            }
            GameManager.instance.LoadLevel();
            stageChracter.gameObject.SetActive(false);
        }
        else
        {
            Debug.LogWarning($"Current ID {currentId} không nằm trong danh sách heroID!");


        }
    }
}
