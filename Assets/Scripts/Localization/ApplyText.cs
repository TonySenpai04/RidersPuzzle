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
    [SerializeField] private TMP_Text skill_info_hero_1001;
    [SerializeField] private TMP_Text skill_info_hero_1002;
    [SerializeField] private TMP_Text button_skill;
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
    [SerializeField] private TMP_Text title_notification;
    [SerializeField] private TMP_Text popup_detail_quit;
    [SerializeField] private TMP_Text popup_detail_language_change;
    [SerializeField] private TMP_Text popup_detail_credit;
    [SerializeField] private TMP_Text button_yes;
    [SerializeField] private TMP_Text button_no;
    [SerializeField] private TMP_Text[] button_ok;

    void ILocalizeObject.ApplyText(ref Dictionary<string, string> localizedRichText, ref Dictionary<string, string> localizedTexts)
    {
        this.loading.SetText(localizedRichText["loading"] + localizedTexts["loading"]);
        this.button_start.SetText(localizedRichText["button_start"] + localizedTexts["button_start"]);
        this.title_choose_a_stage.SetText(localizedRichText["title_choose_a_stage"] + localizedTexts["title_choose_a_stage"]);
        this.warning_not_unlocked.SetText(localizedRichText["warning_not_unlocked"] + localizedTexts["warning_not_unlocked"]);
        this.warning_coming_soon.SetText(localizedRichText["warning_coming_soon"] + localizedTexts["warning_coming_soon"]);
        this.guide_game_rule.SetText(localizedRichText["guide_game_rule"] + localizedTexts["guide_game_rule"]);
        this.title_rider_information.SetText(localizedRichText["title_rider_information"] + localizedTexts["title_rider_information"]);
        this.hp_hero_1001.SetText(localizedRichText["hp_hero_1001"] + localizedTexts["hp_hero_1001"]);
        this.hp_hero_1002.SetText(localizedRichText["hp_hero_1002"] + localizedTexts["hp_hero_1002"]);
        this.skill_info_hero_1001.SetText(localizedRichText["skill_info_hero_1001"] + localizedTexts["skill_info_hero_1001"]);
        this.skill_info_hero_1002.SetText(localizedRichText["skill_info_hero_1002"] + localizedTexts["skill_info_hero_1002"]);
        this.button_skill.SetText(localizedRichText["button_skill"] + localizedTexts["button_skill"]);
        this.button_replay.SetText(localizedRichText["button_replay"] + localizedTexts["button_start"]);
        this.title_setting.SetText(localizedRichText["title_setting"] + localizedTexts["title_setting"]);
        this.setting_sound.SetText(localizedRichText["setting_sound"] + localizedTexts["setting_sound"]);
        this.setting_sound_on.SetText(localizedRichText["setting_sound_on"] + localizedTexts["setting_sound_on"]);
        this.setting_sound_off.SetText(localizedRichText["setting_sound_off"] + localizedTexts["setting_sound_off"]);
        this.setting_language.SetText(localizedRichText["setting_language"] + localizedTexts["setting_language"]);
        this.setting_language_1.SetText(localizedRichText["setting_language_1"] + localizedTexts["setting_language_1"]);
        this.setting_language_2.SetText(localizedRichText["setting_language_2"] + localizedTexts["setting_language_2"]);
        this.setting_privacy_policy.SetText(localizedRichText["setting_privacy_policy"] + localizedTexts["setting_privacy_policy"]);
        foreach (var buttonview in button_view)
        {
            buttonview.SetText(localizedRichText["button_view"] + localizedTexts["button_view"]);
        }
        this.setting_term_of_condition.SetText(localizedRichText["setting_term_of_condition"] + localizedTexts["setting_term_of_condition"]);
        this.setting_credit.SetText(localizedRichText["setting_credit"] + localizedTexts["setting_credit"]);
        this.title_notification.SetText(localizedRichText["title_notification"] + localizedTexts["title_notification"]);
        this.popup_detail_quit.SetText(localizedRichText["popup_detail_quit"] + localizedTexts["popup_detail_quit"]);
        this.popup_detail_language_change.SetText(localizedRichText["popup_detail_language_change"] + localizedTexts["popup_detail_language_change"]);
        this.popup_detail_credit.SetText(localizedRichText["popup_detail_credit"] + localizedTexts["popup_detail_credit"]);
        this.button_yes.SetText(localizedRichText["button_yes"] + localizedTexts["button_yes"]);
        this.button_no.SetText(localizedRichText["button_no"] + localizedTexts["button_no"]);
        foreach (var buttonok in button_ok)
        {
            buttonok.SetText(localizedRichText["button_ok"] + localizedTexts["button_ok"]);
        }
      


    }
}
