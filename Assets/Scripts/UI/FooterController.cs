using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[Serializable]
public struct ButtonFotter 
{
    public string name;
    public Button button;
    public Sprite selected;
    public Sprite unselected;
}

public class FooterController : MonoBehaviour
{
    public static FooterController instance;
    public List<ButtonFotter> buttons;
    private void Awake()
    {
        instance = this;
    }
    private void Start()
    {
      foreach (var button in buttons)
       {
            button.button.GetComponent<Image>().sprite = button.unselected;
            button.button.onClick.AddListener(() => OnButtonClicked(button.button));
       }
        SelectButton("Map");
    }
    private void OnButtonClicked(Button clickedButton)
    {
        foreach (var button in buttons)
        {
            Image buttonImage = button.button.GetComponent<Image>();
            if (button.button == clickedButton)
            {
                buttonImage.sprite = button.selected;
            }
            else
            {
                buttonImage.sprite = button.unselected;
            }
        }
        BackgroundManager.instance.SetDefaultBg();
    }
    public void SelectButton(string name)
    {
        foreach (var button in buttons)
        {
            Image buttonImage = button.button.GetComponent<Image>();
            if (button.name == name)
            {
                buttonImage.sprite = button.selected;
            }
            else
            {
                buttonImage.sprite = button.unselected;
            }
            
        }
        BackgroundManager.instance.SetDefaultBg();

    }
}
