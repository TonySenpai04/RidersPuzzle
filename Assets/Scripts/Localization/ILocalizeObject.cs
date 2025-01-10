using System.Collections.Generic;
using TMPro;

public interface ILocalizeObject
{
    void ApplyText(ref Dictionary<string, string> localizedRichText, ref Dictionary<string, string> localizedTexts);
}