using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LanguageSetting : MonoBehaviour
{
    [SerializeField] private Toggle eng;
    [SerializeField] private Toggle vn;
    [SerializeField] private GameObject notiChange;
    [SerializeField] private GameObject setting;
    private int currentLanguage;
    private int temp;
    private void Start()
    {
        LoadLanguage();
        eng.onValueChanged.AddListener(EngToggleOnChanged);
        vn.onValueChanged.AddListener(VnToggleOffChanged);
    }
    public void LoadLanguage()
    {
        currentLanguage = LocalizeController.instance.GetCurrentLangaue();
        if (currentLanguage == 3)
        {
            eng.isOn = true;
            vn.isOn = false;
        }
        else
        {
            eng.isOn = false;
            vn.isOn = true;
        }
    }
    private void EngToggleOnChanged(bool isOn)
    {
        if (isOn)
        {
            vn.isOn = false;
            temp = 3;
            setting.gameObject.SetActive(false);
            notiChange.gameObject.SetActive(true);
        }
    }

    private void VnToggleOffChanged(bool isOn)
    {
        if (isOn)
        {
            eng.isOn = false;
            temp = 5;
            setting.gameObject.SetActive(false);
            notiChange.gameObject.SetActive(true);
        }
    }
    public void SetLangaue()
    {
        LocalizeController.instance.ChangeLanguage(temp);
        Application.Quit();

    }
  
}
