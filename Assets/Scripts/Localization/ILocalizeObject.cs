using System.Collections.Generic;
using TMPro;

public interface ILocalizeObject
{
    void ApplyText1(ref Dictionary<string, string> localizedRichText, ref Dictionary<string, string> localizedTexts);
    void ApplyFont(ref Dictionary<string, TMP_FontAsset> localizedFonts);
}