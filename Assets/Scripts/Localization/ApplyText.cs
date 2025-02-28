using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ApplyText : MonoBehaviour, ILocalizeObject
{
    [SerializeField] private TMP_Text loading;
    [SerializeField] private TMP_Text button_start;
    [SerializeField] private TMP_Text title_choose_a_stage;
    [SerializeField] private TMP_Text warning_not_unlocked;
    [SerializeField] private TMP_Text warning_coming_soon;
    [SerializeField] private TMP_Text title_stage;
    [SerializeField] private TMP_Text guide_game_rule;
    [SerializeField] private TMP_Text title_rider_information;
    [SerializeField] private TMP_Text hp_hero_1001;
    [SerializeField] private TMP_Text hp_hero_1002;
    [SerializeField] private TMP_Text skill_info_hero_stage;
    [SerializeField] private TMP_Text skill_info_hero_onstage;
    [SerializeField] private TMP_Text[] button_skill;
    [SerializeField] private TMP_Text button_replay;
    [SerializeField] private TMP_Text title_setting;
    [SerializeField] private TMP_Text setting_sound;
    [SerializeField] private TMP_Text setting_sound_on;
    [SerializeField] private TMP_Text setting_sound_off;
    [SerializeField] private TMP_Text setting_language;
    [SerializeField] private TMP_Text setting_language_1;
    [SerializeField] private TMP_Text setting_language_2;
    [SerializeField] private TMP_Text setting_privacy_policy;
    [SerializeField] private TMP_Text[] button_view;
    [SerializeField] private TMP_Text setting_term_of_condition;
    [SerializeField] private TMP_Text setting_credit;
    [SerializeField] private TMP_Text[] title_notification;
    [SerializeField] private TMP_Text popup_detail_quit;
    [SerializeField] private TMP_Text popup_detail_language_change;
    [SerializeField] private TMP_Text popup_detail_credit;
    [SerializeField] private TMP_Text[] button_yes;
    [SerializeField] private TMP_Text[] button_no;
    [SerializeField] private TMP_Text[] button_ok;
    [SerializeField] private TMP_Text skill_info_hero_lib;
    [Header("Object")]
    [SerializeField] private TMP_Text objectName;
    [SerializeField] private TMP_Text objectStory;
    [SerializeField] private TMP_Text objectBattle;
    [SerializeField] private TMP_Text libraryObjectStory;
    [SerializeField] private TMP_Text libraryObjectBattleInfor;
    [Header("Rider")]
    [SerializeField] private TMP_Text riderNameLib;
    [SerializeField] private TMP_Text riderStory;
    [SerializeField] private TMP_Text riderWeight;
    [SerializeField] private TMP_Text riderHeight;
    [SerializeField] private TMP_Text riderPunch;
    [SerializeField] private TMP_Text riderKick;
    [Header("Lib")]
    [SerializeField] private TMP_Text libraryFeatureName;
    [SerializeField] private TMP_Text libraryButtonTutorial;
    [SerializeField] private TMP_Text libraryRiderSectionTitle;
    [SerializeField] private TMP_Text libraryObjectSectionTitle;
    [SerializeField] private TMP_Text libraryRiderTitle;
    [SerializeField] private TMP_Text libraryObjectTitle;
    [SerializeField] private TMP_Text libraryRiderAmount;
    [SerializeField] private TMP_Text libraryObjectAmount;
    [SerializeField] private TMP_Text libraryRiderStory;
    [SerializeField] private TMP_Text libraryRiderBattleInfor;
    [SerializeField] private TMP_Text libraryRiderForm;
    [SerializeField] private TMP_Text libraryRiderWeight;
    [SerializeField] private TMP_Text libraryRiderHeight;
    [SerializeField] private TMP_Text libraryRiderPunchPower;
    [SerializeField] private TMP_Text libraryRiderKickPower;
    [Header("Toturial")]
    [SerializeField] private TMP_Text tutorial_how_to_win;
    [SerializeField] private TMP_Text tutorial_how_to_win_explain;
    [SerializeField] private TMP_Text tutorial_how_to_move;
    [SerializeField] private TMP_Text tutorial_how_to_move_explain;
    [SerializeField] private TMP_Text tutorial_use_skill;
    [SerializeField] private TMP_Text tutorial_use_skill_explain;
    [SerializeField] private TMP_Text tutorial_replay;
    [SerializeField] private TMP_Text tutorial_replay_explain;

    public static ApplyText instance;

    private void Awake() => instance = this;
    private void Start()
    {
        UpdateLibInfo();
        UpdateToturialInfo();
    }
    public void LoadLangue()
    {
        UpdateLibInfo();
        UpdateToturialInfo();

    }
    private void SetInfo(string key, TMP_Text targetText, Dictionary<string, string> richText, Dictionary<string, string> plainText)
    {
        if (richText.ContainsKey(key) && plainText.ContainsKey(key))
            targetText.SetText(plainText[key]);
        else
            targetText.SetText("Thông tin kỹ năng không khả dụng.");
    }
    public void UpdateLibInfo()
    {
        var richText = LocalizationManager.instance.GetLocalizedRichText();
        var plainText = LocalizationManager.instance.GetLocalizedTexts();
        SetInfo("library_feature_name", libraryFeatureName, richText, plainText);
        SetInfo("library_button_tutorial", libraryButtonTutorial, richText, plainText);
        SetInfo("library_rider_section_title", libraryRiderSectionTitle, richText, plainText);
        SetInfo("library_object_section_title", libraryObjectSectionTitle, richText, plainText);
        SetInfo("library_rider_section_title", libraryRiderTitle, richText, plainText);
        SetInfo("library_object_section_title", libraryObjectTitle, richText, plainText);
        SetInfo("library_rider_amount", libraryRiderAmount, richText, plainText );
        SetInfo("library_object_amount", libraryObjectAmount, richText, plainText);
        SetInfo("library_rider_story", libraryRiderStory, richText, plainText);
        SetInfo("library_rider_battle infor", libraryRiderBattleInfor, richText, plainText);
        SetInfo("library_rider_story", libraryObjectStory, richText, plainText);
        SetInfo("library_rider_battle infor", libraryObjectBattleInfor, richText, plainText);
        SetInfo("library_rider_form", libraryRiderForm, richText, plainText);
        SetInfo("library_rider_weight", libraryRiderWeight, richText, plainText);
        SetInfo("library_rider_height", libraryRiderHeight, richText, plainText);
        SetInfo("library_rider_punch_power", libraryRiderPunchPower, richText, plainText);
        SetInfo("library_rider_kick_power", libraryRiderKickPower, richText, plainText);


    }
    public void UpdateToturialInfo()
    {
        var richText = LocalizationManager.instance.GetLocalizedRichText();
        var plainText = LocalizationManager.instance.GetLocalizedTexts();
        SetInfo("tutorial_how_to_win", tutorial_how_to_win, richText, plainText);
        SetInfo("tutorial_how_to_win_explain", tutorial_how_to_win_explain, richText, plainText);
        SetInfo("tutorial_how_to_move", tutorial_how_to_move, richText, plainText);
        SetInfo("tutorial_how_to_move_explain", tutorial_how_to_move_explain, richText, plainText);
        SetInfo("tutorial_use_skill", tutorial_use_skill, richText, plainText);
        SetInfo("tutorial_use_skill_explain", tutorial_use_skill_explain, richText, plainText);
        SetInfo("tutorial_replay", tutorial_replay, richText, plainText);
        SetInfo("tutorial_replay_explain", tutorial_replay_explain, richText, plainText);

    }
    public void UpdateSkillInfo(int heroId, TMP_Text targetText)
    {
        var richText = LocalizationManager.instance.GetLocalizedRichText();
        var plainText = LocalizationManager.instance.GetLocalizedTexts();

        SetInfo($"skill_info_hero_{heroId}", targetText, richText, plainText);
        SetInfo($"hero_story_{heroId}", riderStory, richText, plainText);
        SetInfo($"hero_weight_{heroId}", riderWeight, richText,plainText);
        SetInfo($"hero_height_{heroId}", riderHeight, richText, plainText);
        SetInfo($"hero_punch_{heroId}", riderPunch, richText, plainText);
        SetInfo($"hero_kick_{heroId}", riderKick, richText, plainText);
    }
    public void UpdateObjectInfo(int objectId)
    {
        var richText = LocalizationManager.instance.GetLocalizedRichText();
        var plainText = LocalizationManager.instance.GetLocalizedTexts();
        SetInfo($"object_name_{objectId}", objectName, richText, plainText);
        SetInfo($"object_story_{objectId}", objectStory, richText, plainText);
        SetInfo($"object_battle_info_{objectId}", objectBattle, richText, plainText);

    }

    public void UpdateSkillInfoOnStage(int heroId) => UpdateSkillInfo(heroId, skill_info_hero_onstage);
    public void UpdateSkillInfoStage(int heroId) => UpdateSkillInfo(heroId, skill_info_hero_stage);
    public void UpdateSkillInfoLib(int heroId) => UpdateSkillInfo(heroId, skill_info_hero_lib);

    public void UpdateTitleStage(int level)
    {
        var localizedRichText = LocalizationManager.instance.GetLocalizedRichText();
        var localizedTexts = LocalizationManager.instance.GetLocalizedTexts();
  
        if ( localizedTexts.ContainsKey("title_stage"))
        {
            this.title_stage.SetText( localizedTexts["title_stage"] + " " + level);
        }
    }

    public void ApplyText1(ref Dictionary<string, string> localizedRichText, ref Dictionary<string, string> localizedTexts)
    {
        var textLocalizer = new TextLocalizer(localizedRichText, localizedTexts);

        textLocalizer.SetLocalizedText("loading", loading);
        textLocalizer.SetLocalizedText("button_start", button_start);
        textLocalizer.SetLocalizedText("title_stage", title_stage);
        textLocalizer.SetLocalizedText("title_choose_a_stage", title_choose_a_stage);
        textLocalizer.SetLocalizedText("warning_not_unlocked", warning_not_unlocked);
        textLocalizer.SetLocalizedText("warning_coming_soon", warning_coming_soon);
        textLocalizer.SetLocalizedText("guide_game_rule", guide_game_rule);
        textLocalizer.SetLocalizedText("title_rider_information", title_rider_information);
        textLocalizer.SetLocalizedText("hp_hero_1001", hp_hero_1001);
        textLocalizer.SetLocalizedText("hp_hero_1002", hp_hero_1002);
        textLocalizer.SetLocalizedText("skill_info_hero_1001", skill_info_hero_stage);
        textLocalizer.SetLocalizedText("skill_info_hero_1002", skill_info_hero_onstage);
        textLocalizer.SetLocalizedText("button_skill", button_skill);
        textLocalizer.SetLocalizedText("button_replay", button_replay);
        textLocalizer.SetLocalizedText("title_setting", title_setting);
        textLocalizer.SetLocalizedText("setting_sound", setting_sound);
        textLocalizer.SetLocalizedText("setting_language", setting_language);
        textLocalizer.SetLocalizedText("setting_privacy_policy", setting_privacy_policy);
        textLocalizer.SetLocalizedText("setting_credit", setting_credit);
        textLocalizer.SetLocalizedText("title_notification", title_notification);
        textLocalizer.SetLocalizedText("setting_term_of_condition", setting_term_of_condition);
        textLocalizer.SetLocalizedText("popup_detail_quit", popup_detail_quit);
        textLocalizer.SetLocalizedText("button_yes", button_yes);
        textLocalizer.SetLocalizedText("button_no", button_no);
        textLocalizer.SetLocalizedText("setting_language_1", setting_language_1);
        textLocalizer.SetLocalizedText("setting_language_2", setting_language_2);
        textLocalizer.SetLocalizedText("setting_sound_on", setting_sound_on);
        textLocalizer.SetLocalizedText("setting_sound_off", setting_sound_off);
        textLocalizer.SetLocalizedText("button_view", button_view);
        textLocalizer.SetLocalizedText("button_ok", button_ok);
        textLocalizer.SetLocalizedText("title_notification", title_notification);
        textLocalizer.SetLocalizedText("popup_detail_language_change", popup_detail_language_change);
    }

    public void ApplyFont(ref Dictionary<string, TMP_FontAsset> localizedFonts)
    {
        var fontLocalizer = new FontLocalizer(localizedFonts);
        TMP_Text[] allTexts =
        {
            loading, button_start, title_stage, title_choose_a_stage, warning_not_unlocked, warning_coming_soon,
            guide_game_rule, title_rider_information, hp_hero_1001, hp_hero_1002, skill_info_hero_stage,
            skill_info_hero_onstage, skill_info_hero_lib, button_replay, title_setting,
            setting_sound, setting_sound_on, setting_sound_off, setting_language, setting_language_1,
            setting_language_2, setting_privacy_policy, setting_term_of_condition, setting_credit
           , popup_detail_quit, popup_detail_language_change, popup_detail_credit,
         
        };

        foreach (var text in allTexts)
            fontLocalizer.SetLocalizedFont(text.name, text);

        fontLocalizer.SetLocalizedFont("button_view", button_view);
        fontLocalizer.SetLocalizedFont("button_ok", button_ok);
        fontLocalizer.SetLocalizedFont("button_no", button_no);
        fontLocalizer.SetLocalizedFont("button_yes", button_yes);
        fontLocalizer.SetLocalizedFont("button_skill", button_skill);
        fontLocalizer.SetLocalizedFont("title_notification", title_notification);
    }
}
