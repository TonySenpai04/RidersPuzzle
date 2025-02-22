using System.Collections.Generic;
using TMPro;

internal class FontLocalizer:IFontLocalizer
{
    private readonly Dictionary<string, TMP_FontAsset> _fonts;

    public FontLocalizer(Dictionary<string, TMP_FontAsset> fonts)
    {
        _fonts = fonts;
    }

    public void SetLocalizedFont(string key, TMP_Text text)
    {
        if (_fonts.ContainsKey(key))
            text.font = _fonts[key];
    }

    public void SetLocalizedFont(string key, TMP_Text[] texts)
    {
        foreach (var t in texts)
            SetLocalizedFont(key, t);
    }
}