using System.Collections.Generic;
using TMPro;
using UnityEngine;
using System.Text.RegularExpressions;

internal class ReadCSVLocalizeQuest : ReadCSVLocalizeBase
{
    public override void LoadLocalization(int currentLanguage, Dictionary<string, string> localizedTexts,
        Dictionary<string, TMP_FontAsset> localizedFonts, TextAsset textAsset, Dictionary<string, string> richText)
    {

        string[] lines = textAsset.text.Split('\n');
        for (int i = 1; i < lines.Length; i++)
        {
            string[] values = csvSplit.Split(lines[i]);
            for (int j = 0; j < values.Length; j++)
            {
                values[j] = values[j].Trim().Trim('"');
            }

            if (values.Length >= 7)
            {
                string key = values[1];
                string enText = values[3];
                string viText = values[5];
                string fontName = currentLanguage == 5 ? values[6] : values[4];
                var richTxt = values[5];

                localizedTexts[key] = currentLanguage == 5 ? viText : enText;
                localizedFonts[key] = Resources.Load<TMP_FontAsset>($"Fonts/{fontName}");
                richText.Add(key, richTxt);
            }
        }

    }
}