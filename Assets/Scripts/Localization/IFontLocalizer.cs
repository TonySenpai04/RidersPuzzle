
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
internal interface IFontLocalizer
{
    void SetLocalizedFont(string key, TMP_Text text);
    void SetLocalizedFont(string key, TMP_Text[] texts);
}