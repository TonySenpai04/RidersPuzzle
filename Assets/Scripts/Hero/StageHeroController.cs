using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

[Serializable]
public struct DataHero {
    public int id;
    public Sprite icon;
}

public class StageHeroController : MonoBehaviour
{

    [SerializeField] private ButtonStage stageButtonPrefab;
    [SerializeField] private Transform buttonParent;
    [SerializeField] private GameObject stageChracter;
    [SerializeField] private GameObject playZone;
    [SerializeField] private SkillManager skillManager;
    [SerializeField] private List<DataHero> heroID;
    [SerializeField] private int currentId;
    void Start()
    {
        CreateButtons();
    }

    private void CreateButtons()
    {
        for (int i = 0; i < heroID.Count; i++)
        {
            ButtonStage button = Instantiate(stageButtonPrefab, buttonParent);
            button.GetComponent<Image>().sprite = heroID[i].icon;
            button.Initialize(heroID[i].id, SetHeroID);
        }
    }
    public void SetHeroID(int id)
    {
        skillManager.SetSkillId(id);
        currentId = id;
    }

    public void LoadLevel()
    {
        if (heroID.Any(hero => hero.id == currentId))
        {
            playZone.gameObject.SetActive(true);
            LevelManager.instance.LoadLevel();
            GameManager.instance.LoadLevel();
            PlayerController.instance.LoadLevel();
            stageChracter.gameObject.SetActive(false);
        }
        else
        {
            Debug.LogWarning($"Current ID {currentId} không nằm trong danh sách heroID!");


        }
    }
}
