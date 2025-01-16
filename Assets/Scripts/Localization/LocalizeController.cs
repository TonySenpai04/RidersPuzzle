using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;

public class LocalizeController : MonoBehaviour
{
    private Dictionary<string, string> localizedTexts;
    private Dictionary<string, TMP_FontAsset> localizedFonts;
    private Dictionary<string, TMP_FontAsset> loadedFonts;
    private Dictionary<string, string> richText;
    private void Awake()
    {
        this.localizedTexts = new Dictionary<string, string>();
        this.richText = new Dictionary<string, string>();
    }

    private void Start()
    {
        Setup();
    }

    public void Setup()
    {
        LoadLanguage(Language.Vietnamese);
    }

    public void ChangeLanguage(int languageCode)
    {
        LoadLanguage((Language)(languageCode));
    }

    public void LoadLanguage(Language language)
    {
        this.localizedTexts.Clear();
        this.richText.Clear();
        this.localizedFonts.Clear();
        this.loadedFonts.Clear();

        var fileCsv = Resources.Load<TextAsset>($"Localization/Localize");
        if (fileCsv != null)
        {
            var csvText = fileCsv.text.Trim().Replace("\r\n", "\n");
            var lines = csvText.Split("\n");
            for (int i = 1; i < lines.Length; i++)
            {
                var segments = lines[i].Split(';');
                if (segments.Length > 0)
                {
                    var localizeKey = segments[1];
                    var localizeOrigin = segments[2];                    
                    var localizeText = segments[(int)language + 3];
                    var richText = segments[5];
                    var fontName = segments[6];

                    Debug.LogWarning((int)language);

                    this.localizedTexts.Add(localizeKey, localizeText);
                    this.richText.Add(localizeKey, richText);

                    if (this.loadedFonts.ContainsKey(fontName) == false)
                    {
                        var loadedFont = Resources.Load<TMP_FontAsset>($"Fonts/{fontName}");
                        this.loadedFonts.Add(fontName, loadedFont);
                    }

                    this.localizedFonts.Add(localizeKey, this.loadedFonts[fontName]);

                    //Debug.LogWarning(localizeOrigin);
                    //Debug.LogWarning(localizeText);
                    //Debug.LogWarning(richText);

                }
            }

            var appliers = Object.FindObjectsOfType<MonoBehaviour>().OfType<ILocalizeObject>();
            foreach (var aplier in appliers)
            {
                aplier.ApplyText(ref this.richText, ref this.localizedTexts);
                aplier.ApplyFont(ref this.localizedFonts);
            }
        }
        else
        {
            Debug.LogError("Not found localization files !");
        }
    }

    public enum Language
    {
        English,
        Vietnamese
    }
}