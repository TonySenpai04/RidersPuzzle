using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
public class StoryView:MonoBehaviour
{
    public string id;
    public TextMeshProUGUI titleTxt;
    public TextMeshProUGUI descriptionTxt;
    public void SetData(string id)
    {
        this.id = id;
        ApplyText.instance.UpdateStoryInfo(id);
    }
}