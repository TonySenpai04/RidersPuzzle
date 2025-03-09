using System.Collections.Generic;
using System.IO;
using System.Linq;
using TMPro;
using UnityEngine;

public class LocalizeController : MonoBehaviour
{
    public string languagePath => Path.Combine(Application.persistentDataPath, "Language.json");
    private Dictionary<string, string> localizedTexts;
    private Dictionary<string, TMP_FontAsset> localizedFonts;
    private Dictionary<string, TMP_FontAsset> loadedFonts;
    private Dictionary<string, string> richText;
    public static LocalizeController instance;
    private int currentLanguage;
    private void Awake()
    {
        instance=this;
        this.localizedTexts = new Dictionary<string, string>();
        this.richText = new Dictionary<string, string>();
        this.localizedFonts = new Dictionary<string, TMP_FontAsset>(); 
        this.loadedFonts = new Dictionary<string, TMP_FontAsset>();
    }
    public void SaveLanguage()
    {
        var data = new LanguageData { languageCode = currentLanguage };
        string json = JsonUtility.ToJson(data);
        File.WriteAllText(languagePath, json);

    }
    public void LoadLanguage()
    {
        if (File.Exists(languagePath))
        {
            string json = File.ReadAllText(languagePath);
            var language = JsonUtility.FromJson<LanguageData>(json);
            this.currentLanguage = language.languageCode;
        }
        else
        {
            currentLanguage = 3;
        }
    }
    private void Start()
    {
        LoadLanguage();
        Setup();
    }
    public int GetCurrentLangaue()
    {
        return currentLanguage;
    }
    public void Setup()
    {
        LoadLanguage((Language)(currentLanguage));
    }

    public void ChangeLanguage(int languageCode)
    {
           this.currentLanguage = languageCode;
        SaveLanguage();
       // LoadLanguage((Language)(languageCode));
    }
    public Dictionary<string, string> GetLocalizedRichText() => richText;

    public Dictionary<string, string> GetLocalizedTexts() => localizedTexts;


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
                    var localizeText = segments[(int)language];
                    var richText = segments[7];
                    var fontName = segments[(int)language + 1];

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
                aplier.ApplyTxt(ref this.richText, ref this.localizedTexts);
                aplier.ApplyFont(ref this.localizedFonts);
            }
        }
        else
        {
            Debug.LogError("Not found localization files !");
        }
    }
    public void SetSkillInfo()
    {
        var appliers = Object.FindObjectsOfType<MonoBehaviour>().OfType<ILocalizeObject>();
        foreach (var aplier in appliers)
        {
            aplier.ApplyTxt(ref this.richText, ref this.localizedTexts);
            aplier.ApplyFont(ref this.localizedFonts);
        }
    }
    public enum Language
    {
        English = 3,
        Vietnamese = 5
    }
}
public class LanguageData
{
    public int languageCode;
}