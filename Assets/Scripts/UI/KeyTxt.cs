using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class KeyTxt : MonoBehaviour
{
    public TextMeshProUGUI txt;
    public void Update()
    {
        txt.text="x"+LevelManager.instance.GetCurrentLevelKey().ToString();
    }
}
