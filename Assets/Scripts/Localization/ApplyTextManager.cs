// ApplyTextManager with override key handling and auto grouping
#if UNITY_EDITOR
using UnityEditor;
#endif
using UnityEngine;
using TMPro;
using System.Reflection;
using System.Collections.Generic;
using System.Linq;
using UnityEngine.UI;

[System.Serializable]
public class LocalizedTextItem
{
    public string key;
    public TMP_Text text;
}
[System.Serializable]
public class LocalizedTextNorItem
{
    public string key;
    public Text text;
}

public class ApplyTextManager : MonoBehaviour, ILocalizeObject
{
    public static ApplyTextManager instance;
    public TextLocalizer textLocalizer;

    [Header("Common Texts")] public List<LocalizedTextItem> commonTexts = new();
    [Header("Shop Texts")] public List<LocalizedTextItem> shopTexts = new();
    [Header("Rider Texts")] public List<LocalizedTextItem> riderTexts = new();
    [Header("Login Texts")] public List<LocalizedTextItem> loginTexts = new();
    [Header("Tutorial Texts")] public List<LocalizedTextItem> tutorialTexts = new();
    [Header("Quest Texts")] public List<LocalizedTextItem> questTexts = new();
    [Header("Setting Texts")] public List<LocalizedTextItem> settingTexts = new();
    [Header("Story Texts")] public List<LocalizedTextItem> storyTexts = new();
    [Header("Enum Type Texts")] public List<LocalizedTextItem> enumTypeTexts = new();
    [Header("Font Targets")] public List<TMP_Text> fontTargets = new();
    [Header("Nor texts")]
    public List<LocalizedTextNorItem> norTexts = new();
    private void Awake()
    {
        instance = this;
    }
    private void Start()
    {
        UpdateLibInfo();
        UpdateToturialInfo();
    }
    public void LoadLangue()
    {
        UpdateLibInfo();
        UpdateToturialInfo();
        UpdateEnumType();
    

    }
    private TMP_Text GetTextByKey(string key)
    {
        return commonTexts.Concat(shopTexts).Concat(riderTexts).Concat(loginTexts)
                          .Concat(tutorialTexts).Concat(questTexts).Concat(settingTexts)
                          .Concat(storyTexts)
                          .FirstOrDefault(item => item.key == key)?.text;
    }

    private void SetInfo(string key, TMP_Text targetText, Dictionary<string, string> richText, Dictionary<string, string> plainText)
    {
        if (targetText != null && plainText.TryGetValue(key, out var value))
        {
            targetText.SetText(value);
        }
    }

    public void UpdateSkillInfo(int heroId, TMP_Text targetText)
    {
        var richText = LocalizationManager.instance.GetLocalizedRichText();
        var plainText = LocalizationManager.instance.GetLocalizedTexts();

        SetInfo($"skill_info_hero_{heroId}", targetText, richText, plainText);
        SetInfo($"hero_story_{heroId}", GetTextByKey("riderStory"), richText, plainText);
        SetInfo($"hero_weight_{heroId}", GetTextByKey("riderWeight"), richText, plainText);
        SetInfo($"hero_height_{heroId}", GetTextByKey("riderHeight"), richText, plainText);
        SetInfo($"hero_punch_{heroId}", GetTextByKey("riderPunch"), richText, plainText);
        SetInfo($"hero_kick_{heroId}", GetTextByKey("riderKick"), richText, plainText);

        SetInfo($"skill_info_hero_{heroId}", GetTextByKey("riderSkillShop"), richText, plainText);
        SetInfo($"hero_story_{heroId}", GetTextByKey("riderStoryShop"), richText, plainText);
        SetInfo($"hero_weight_{heroId}", GetTextByKey("riderWeightShop"), richText, plainText);
        SetInfo($"hero_height_{heroId}", GetTextByKey("riderHeightShop"), richText, plainText);
        SetInfo($"hero_punch_{heroId}", GetTextByKey("riderPunchShop"), richText, plainText);
        SetInfo($"hero_kick_{heroId}", GetTextByKey("riderKickShop"), richText, plainText);
    }

    public void UpdateSkillInfoOnStage(int heroId) => UpdateSkillInfo(heroId, GetTextByKey("skill_info_hero_onstage"));
    public void UpdateSkillInfoStage(int heroId) => UpdateSkillInfo(heroId, GetTextByKey("skill_info_hero_stage"));
    public void UpdateSkillInfoLib(int heroId) => UpdateSkillInfo(heroId, GetTextByKey("skill_info_hero_lib"));

    public void UpdateUpgrade(int heroId)
    {
        var richText = LocalizationManager.instance.GetLocalizedRichText();
        var plainText = LocalizationManager.instance.GetLocalizedTexts();
        SetInfo($"skill_info_hero_{heroId}", GetTextByKey("heroSkill"), richText, plainText);
    }

    public void UpdateObjectInfo(int objectId)
    {
        
        var richText = LocalizationManager.instance.GetLocalizedRichText();
        var plainText = LocalizationManager.instance.GetLocalizedTexts();

        var objectStory = GetTextByKey("objectStory");
        var objectBattle = GetTextByKey("objectBattle");

        SetInfo($"object_name_{objectId}", GetTextByKey("objectName"), richText, plainText);
        SetInfo($"object_story_{objectId}", objectStory, richText, plainText);
        SetInfo($"object_battle_info_{objectId}", objectBattle, richText, plainText);

        if (objectId == 2032)
        {
            if (objectStory != null)
                objectStory.text = LocalizationManager.instance.GetLocalizedText($"object_story_{objectId}", "Hepi");
            if (objectBattle != null)
                objectBattle.text = LocalizationManager.instance.GetLocalizedText($"object_battle_info_{objectId}", "Hepi");
        }
        else if (objectId == 2030)
        {
            string fallback = LocalizationManager.instance.GetLocalizedText("object_name_2001");
            if (objectStory != null)
                objectStory.text = LocalizationManager.instance.GetLocalizedText($"object_story_{objectId}", fallback);
            if (objectBattle != null)
                objectBattle.text = LocalizationManager.instance.GetLocalizedText($"object_battle_info_{objectId}", fallback);
        }
    

    }

    public void UpdateTitleStage(int level)
    {
        var localizedRichText = LocalizationManager.instance.GetLocalizedRichText();
        var localizedTexts = LocalizationManager.instance.GetLocalizedTexts();

        if (localizedTexts.ContainsKey("title_stage"))
        {
            GetTextByKey("title_stage").SetText(localizedTexts["title_stage"] + " " + level);
        }
        //var plainText = LocalizationManager.instance.GetLocalizedTexts();
        //if (plainText.TryGetValue("title_stage", out var baseTitle))
        //{
        //    var target = GetTextByKey("title_stage");
        //    if (target != null) target.SetText($"{baseTitle} {level}");
        //}
    }

    public void UpdateStoryInfo(string storyId)
    {
        var richText = LocalizationManager.instance.GetLocalizedRichText();
        var plainText = LocalizationManager.instance.GetLocalizedTexts();

        SetInfo($"story_title_{storyId}", GetTextByKey("titleStory"), richText, plainText);
        SetInfo($"story_detail_{storyId}", GetTextByKey("contentStory"), richText, plainText);
    }

    public void UpdateLibInfo()
    {
        var richText = LocalizationManager.instance.GetLocalizedRichText();
        var plainText = LocalizationManager.instance.GetLocalizedTexts();

        
        foreach (var key in new[]
        {
            "library_feature_name", "library_button_tutorial",
            "library_rider_section_title", "library_object_section_title",
            "library_rider_amount", "library_object_amount", "library_story_amount",
            "library_rider_story", "library_rider_battle_infor", "library_rider_form",
            "library_rider_weight", "library_rider_height", "library_rider_punch_power",
            "library_rider_kick_power", "library_object_story", "library_object_battle_infor"
        })
        {
            SetInfo(key, GetTextByKey(key), richText, plainText);
        }
    }

    public void UpdateToturialInfo()
    {
        var richText = LocalizationManager.instance.GetLocalizedRichText();
        var plainText = LocalizationManager.instance.GetLocalizedTexts();

        foreach (var key in new[]
        {
            "tutorial_how_to_win", "tutorial_how_to_win_explain",
            "tutorial_how_to_move", "tutorial_how_to_move_explain",
            "tutorial_use_skill", "tutorial_use_skill_explain",
            "tutorial_replay", "tutorial_replay_explain"
        })
        {
            SetInfo(key, GetTextByKey(key), richText, plainText);
        }
    }

    public void ApplyTxt(ref Dictionary<string, string> localizedRichText, ref Dictionary<string, string> localizedTexts)
    {
        textLocalizer = new TextLocalizer(localizedRichText, localizedTexts);
        ApplyLocalizedTexts(commonTexts, textLocalizer);
        ApplyLocalizedTexts(riderTexts, textLocalizer);
        ApplyLocalizedTexts(shopTexts, textLocalizer);
        ApplyLocalizedTexts(settingTexts, textLocalizer);
        ApplyLocalizedTexts(tutorialTexts, textLocalizer);
        ApplyLocalizedTexts(questTexts, textLocalizer);
        ApplyLocalizedTexts(loginTexts, textLocalizer);
        ApplyLocalizedTexts(storyTexts, textLocalizer);
        ApplyLocalizedTexts(enumTypeTexts, textLocalizer);
        ApplyLocalizedNorTexts(norTexts, textLocalizer);
    }
    void UpdateEnumType()
    {
        var richText = LocalizationManager.instance.GetLocalizedRichText();
        var plainText = LocalizationManager.instance.GetLocalizedTexts();
        foreach (var key in enumTypeTexts)
        {
            SetInfo(key.key, GetTextByKey(key.key), richText, plainText);
        }
    }
    public void ApplyFont(ref Dictionary<string, TMP_FontAsset> localizedFonts)
    {
        var fontLocalizer = new FontLocalizer(localizedFonts);
        foreach (var target in fontTargets)
        {
            if (target != null)
                fontLocalizer.SetLocalizedFont(target.name, target);
        }
    }

    private void ApplyLocalizedTexts(List<LocalizedTextItem> items, TextLocalizer localizer)
    {
        foreach (var item in items)
        {
            if (item.text != null && !string.IsNullOrEmpty(item.key))
                localizer.SetLocalizedText(item.key, item.text);
        }
    }
    private void ApplyLocalizedNorTexts(List<LocalizedTextNorItem> items, TextLocalizer localizer)
    {
        foreach (var item in items)
        {
            if (item.text != null && !string.IsNullOrEmpty(item.key))
                localizer.SetLocalizedText(item.key, item.text);
        }
    }
}
