using System.Collections.Generic;
using TMPro;
using UnityEngine;

public interface IReadDataLocalize
{
    void LoadLocalization(int currentLanguage, Dictionary<string, string> localizedTexts,
         Dictionary<string, TMP_FontAsset> localizedFonts, TextAsset textAsset, Dictionary<string, string> richText);
}