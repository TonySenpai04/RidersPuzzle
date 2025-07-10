using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ChangeHeroButton : MonoBehaviour
{

    [SerializeField] public int heroID;
    [SerializeField] private TextMeshProUGUI levelTxt;
    [SerializeField] private TextMeshProUGUI healthTxt;
    [SerializeField] private TextMeshProUGUI mpTxt;
    [SerializeField] private Image heroImg;
    [SerializeField] public Image selectImg;
    [SerializeField] private Button button;

    public void SetData(int heroID, int level, int health,
        int mp, Sprite heroSprite,Action<int> onClickCallback)
    {
        this.heroID = heroID;
        levelTxt.text = LocalizationManager.instance.GetLocalizedText("level_title") + " " + level.ToString();
        healthTxt.text = health.ToString();
        mpTxt.text = mp.ToString();
        heroImg.sprite = heroSprite;
        button.onClick.RemoveAllListeners(); // luôn clear trước khi gán mới
        button.onClick.AddListener(() => onClickCallback?.Invoke(heroID));


    }
    public void SetSelected(bool isSelected)
    {
        selectImg.gameObject.SetActive(isSelected);
    }

}
