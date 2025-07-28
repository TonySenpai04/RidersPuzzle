using System;
using System.Reflection;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class HeroFusionButton: MonoBehaviour
{
    [SerializeField] private int id;
    [SerializeField] private Image heroIcon;
    [SerializeField] private Button button;
    [SerializeField] private TextMeshProUGUI levelTxt;
    [SerializeField] private GameObject highlightBorder;
    [SerializeField] private TextMeshProUGUI healthTxt;
    [SerializeField] private TextMeshProUGUI mpTxt;
    public void SetData(int id,Sprite sprite, int level, int health,
        int mp, System.Action<int> onClickAction)
    {
        this.id = id;
        this.heroIcon.sprite = sprite;
        levelTxt.text = LocalizationManager.instance.GetLocalizedText("level_title") + " " + level.ToString();
        healthTxt.text = health.ToString();
        mpTxt.text = mp.ToString();
        button.onClick.AddListener(()=>onClickAction(id));
        SetHighlight(false);
    }
    public void SetHighlight(bool isOn)
    {
        if (highlightBorder != null)
        {
            highlightBorder.SetActive(isOn);
        }
    }

    public int GetID() => id;
}