using System.Collections.Generic;
using System.IO;
using TMPro;
using UnityEngine;

public class LocalizationManager : MonoBehaviour
{
    public string languagePath => Path.Combine(Application.persistentDataPath, "Language.json");
    public static LocalizationManager instance;
    private Dictionary<string, string> localizedTexts;
    private Dictionary<string, TMP_FontAsset> localizedFonts;
    [SerializeField] private TextAsset riderAsset;
    [SerializeField] private TextAsset objectAsset;
    [SerializeField] private TextAsset commonAsset;
    [SerializeField] private ApplyText applyTextScript;
    private Dictionary<string, string> richText;
    private int currentLanguage;
    private IReadDataLocalize readCSVLocalizeRider;
    private IReadDataLocalize readCSVLocalizeObject;
    private IReadDataLocalize readCSVLocalizeCommon;
    void Awake()
    {
        instance = this;
        InitData();
        LoadData();

    }
    public void InitData()
    {
        localizedTexts = new Dictionary<string, string>();
        localizedFonts = new Dictionary<string, TMP_FontAsset>();
        this.richText = new Dictionary<string, string>();

        readCSVLocalizeRider = new ReadCSVLocalizeRider();
        readCSVLocalizeObject = new ReadCSVLocalizeObject();
        readCSVLocalizeCommon = new ReadCSVLocalizeCommon();
    }

    public void LoadData()
    {


        LoadLanguage();
        LoadLocalization();
        ApplyLocalizedTexts();
    }
    public int GetCurrentLangaue()
    {
        return currentLanguage;
    }

    void LoadLocalization()
    {
        readCSVLocalizeRider.LoadLocalization(currentLanguage,localizedTexts, localizedFonts,riderAsset,richText);
       readCSVLocalizeObject.LoadLocalization(currentLanguage, localizedTexts, localizedFonts, objectAsset, richText);
        readCSVLocalizeCommon.LoadLocalization(currentLanguage, localizedTexts, localizedFonts, commonAsset, richText);
        foreach (var text in localizedTexts)
        {
            Debug.Log(text.Key + "-" + text.Value);
        }

    }
    public void LoadLanague()
    {
        localizedTexts.Clear();
        localizedFonts.Clear();
        richText.Clear();
        LoadLocalization();
        ApplyLocalizedTexts();
        applyTextScript.LoadLangue();
        GameManager.instance.LoadLangue();
    }
    void ApplyLocalizedTexts()
    {
        if (applyTextScript != null)
        {
            applyTextScript.ApplyText1(ref  richText, ref this.localizedTexts);
            applyTextScript.ApplyFont(ref localizedFonts);
        }
    }

    public string GetLocalizedText(string key)
    {
        return localizedTexts.ContainsKey(key) ? localizedTexts[key] : key;
    }

    public TMP_FontAsset GetLocalizedFont(string key)
    {
        return localizedFonts.ContainsKey(key) ? localizedFonts[key] : null;
    }
    public Dictionary<string, string> GetLocalizedRichText() => richText;

    public Dictionary<string, string> GetLocalizedTexts() => localizedTexts;
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
    public void ChangeLanguage(int languageCode)
    {
        this.currentLanguage = languageCode;
        SaveLanguage();
        // LoadLanguage((Language)(languageCode));
    }
    public enum Language
    {
        English = 3,
        Vietnamese = 5
    }
}
