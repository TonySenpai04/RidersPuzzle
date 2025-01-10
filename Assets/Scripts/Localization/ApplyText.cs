using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ApplyText : MonoBehaviour, ILocalizeObject
{
    [SerializeField] private TMP_Text loading;
    [SerializeField] private TMP_Text start;

    void ILocalizeObject.ApplyText(ref Dictionary<string, string> localizedRichText, ref Dictionary<string, string> localizedTexts)
    {
        this.loading.SetText(localizedRichText["Loading"] + localizedTexts["Loading"]);
        this.start.SetText(localizedRichText["Start"] + localizedTexts["Start"]);
        
    }
}
