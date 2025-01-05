using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;

public class HeroInfoManager : MonoBehaviour
{
    [Header("In Choose Stage")]
    public GameObject panelInfo;
    public TextMeshProUGUI txtHealth;
    public TextMeshProUGUI txtSkill;
    public HeroManager heroManager;
    public static HeroInfoManager instance;
    public RectTransform arrow;
    [Header("In Game")]
    public GameObject panelInfoInGame;
    public TextMeshProUGUI txtHealthInGame;
    public TextMeshProUGUI txtSkillInGame;
    private void Awake()
    {
        instance = this;
    }
    public void ShowInfo(int id, RectTransform clickedObject)
    {

        if (heroManager.heroDatas.Any(hero => hero.id == id))
        {

            DataHero hero = heroManager.heroDatas.FirstOrDefault(h => h.id == id);
            panelInfo.gameObject.SetActive (true);
            txtHealth.text = hero.hp.ToString();
            txtSkill.text = hero.skillDescription;
            UpdateArrowPosition(clickedObject);


        }
    

    }
    public void ShowInfoInGame()
    {
        DataHero hero = heroManager.heroDatas.FirstOrDefault(h => h.id == PlayerController.instance.currentHero.id);
        panelInfoInGame.gameObject.SetActive(true);
        txtHealthInGame.text = hero.hp.ToString();
        txtSkillInGame.text = hero.skillDescription;
    }
    private void UpdateArrowPosition(RectTransform clickedObject)
    {
        // Xác định vị trí của object được click trong không gian của tooltip
        Vector3 objectPositionInTooltip = panelInfo.GetComponent<RectTransform>().InverseTransformPoint(clickedObject.position);

        // Cập nhật vị trí của mũi tên
        arrow.anchoredPosition = new Vector2(objectPositionInTooltip.x+70,-170);
    }
}
