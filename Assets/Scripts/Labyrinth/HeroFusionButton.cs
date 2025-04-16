using System;
using System.Reflection;
using UnityEngine;
using UnityEngine.UI;

public class HeroFusionButton: MonoBehaviour
{
    [SerializeField] private int id;
    [SerializeField] private Image heroIcon;
    [SerializeField] private Button button;
    [SerializeField] private GameObject highlightBorder;
    public void SetData(int id,Sprite sprite, System.Action<int> onClickAction)
    {
        this.id = id;
        this.heroIcon.sprite = sprite;
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