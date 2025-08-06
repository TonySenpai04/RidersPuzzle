using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class HeroFusionSlot : MonoBehaviour
{
    [SerializeField] private int id;
    [SerializeField] private GameObject card;
    [SerializeField] private Image heroIcon;
    [SerializeField] private Button button;
    [SerializeField] private Button heroButton;

    [SerializeField] private TextMeshProUGUI levelTxt;
    [SerializeField] private GameObject highlightBorder;
    [SerializeField] private TextMeshProUGUI healthTxt;
    [SerializeField] private TextMeshProUGUI mpTxt;
    public void SetData(int id, Sprite sprite, int level, int health,
        int mp)
    {
        this.id = id;
        this.heroIcon.sprite = sprite;
        levelTxt.text = LocalizationManager.instance.GetLocalizedText("level_title") + " " + level.ToString();
        healthTxt.text = health.ToString();
        mpTxt.text = mp.ToString();
  
        card.SetActive(true);
 
    }
    public void SetClickAction(System.Action onClickAction)
    {
        button.onClick.AddListener(() => onClickAction());
        heroButton.onClick.AddListener(() => onClickAction());

    }
    public void ResetData()
    {
        card.SetActive(false);
    }
    public int GetID() => id;
}
