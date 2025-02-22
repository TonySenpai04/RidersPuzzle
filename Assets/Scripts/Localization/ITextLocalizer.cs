

using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
internal interface ITextLocalizer
{
    void SetLocalizedText(string key, TMP_Text text);
    void SetLocalizedText(string key, TMP_Text[] texts);
}