using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class LocalizeController : MonoBehaviour
{
    private Dictionary<string, string> localizedTexts;
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
        LoadLanguage(Language.English);
    }

    public void ChangeLanguage(int languageCode)
    {
        LoadLanguage((Language)(languageCode));
    }

    public void LoadLanguage(Language language)
    {
        this.localizedTexts.Clear();
        this.richText.Clear();

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
                    var localizeOrigin = segments[0];
                    var localizeText = segments[(int)language + 3];
                    var richText = segments[5];

                    this.localizedTexts.Add(localizeOrigin, localizeText);
                    this.localizedTexts.Add(localizeOrigin, richText);
                    
                  
                }
            }

            var appliers = Object.FindObjectsOfType<MonoBehaviour>().OfType<ILocalizeObject>();
            foreach (var aplier in appliers)
            {
                aplier.ApplyText(ref this.richText, ref this.localizedTexts);
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