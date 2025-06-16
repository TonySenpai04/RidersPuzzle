
using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UpgradeView : MonoBehaviour
{
    public TextMeshProUGUI currentHepiAmount;
    public TextMeshProUGUI currentExpAmount;
    public TextMeshProUGUI currentMasterAmount;
    public TextMeshProUGUI expAmount;
    public TextMeshProUGUI hepiAmount;
    public Image currentExpImg;
    public Image currentMasterImg;
    public Image expImg;
    public TextMeshProUGUI HpTxt;
    public int currentID;
    public TextMeshProUGUI heroNametxt;
    public Image heroImg;
    public Button levelupBtn;
    public Sprite levelupActive;
    public Sprite levelupNotActive;
    public TextMeshProUGUI heroNameLeveluptxt;
    public TextMeshProUGUI currentHpTxt;
    public TextMeshProUGUI upgradeHpTxt;
    public TextMeshProUGUI currentMpTxt;
    public TextMeshProUGUI upgradeMpTxt;
    public TextMeshProUGUI successRatetxt;
    public Image upgradeExpImg;
    public Image upgradeMasterImg;
    public TextMeshProUGUI upgradeHepiAmount;
    public TextMeshProUGUI upgradeExpAmount;
    public TextMeshProUGUI upgradeMasterAmount;
    public GameObject popupUpgrade;
    public Slider mpBar;
    public Image mpBarFillObject;
    public TextMeshProUGUI mpBarFillTxt;
    public int currentMP;
    public int maxMp;
    public TextMeshProUGUI currentRegenTxt;
    public TextMeshProUGUI maxRegenTxt;
    public TextMeshProUGUI rengenTxtName;
    public TextMeshProUGUI regenAmountTxt;
    public int regenAmount;
    public TextMeshProUGUI needRegenTxt;
    public Button regenBtn;
    public Image levelCircle;
    public int maxLevel;
    public RectTransform levelPointer;
    public float radius = 100f;
    public void SetUpgradeView(RiderUpgradeLevel currentData, RiderUpgradeLevel upgradeData)
    {
        int hepi = 0;
        int exp = 0;
        int mastery = 0;

        bool hasEnoughHepi = true;
        bool hasEnoughExp = true;
        bool hasEnoughMastery = true;
        bool hasRegen = true;

        var hero = HeroManager.instance.GetHero(this.currentID);
        this.heroImg.sprite = hero.Value.heroImage;
        this.heroNametxt.text = $"{hero.Value.name}";
        rengenTxtName.text = LocalizationManager.instance.GetLocalizedText("button_regen") + ": " + hero.Value.name.ToUpper();
      //  ApplyText.instance.UpdateUpgrade(currentID);
       ApplyTextManager.instance.UpdateUpgrade(currentID);
        hasRegen = ResourceManager.Instance.GetQuantity(0, 1) >= UpgradeManager.Instance.GetRengenMP();
        if (upgradeData != null)
        {
            foreach (var req in upgradeData.upgradeRequirements)
            {
                var info = ResourceManager.Instance.resourceInfos.Find(r =>
                    r.resourceType == req.resourceType && r.resourceId == req.resourceId);

                if (req.resourceType == 0)
                {
                    hepi = req.amount;
                    hasEnoughHepi = ResourceManager.Instance.HasEnough(req.resourceType, req.resourceId, req.amount);
                }
                else if (req.resourceType == 2)
                {
                    exp = req.amount;
                    hasEnoughExp = ResourceManager.Instance.HasEnough(req.resourceType, req.resourceId, req.amount);

                    if (info != null)
                    {
                        expImg.sprite = info.icon;
                       // currentExpImg.sprite = info.icon;
                    }
                }
                else if (req.resourceType == 3)
                {
                    mastery = req.amount;
                    hasEnoughMastery = ResourceManager.Instance.HasEnough(req.resourceType, req.resourceId, req.amount);

                    if (info != null)
                    {
                        //currentMasterImg.sprite = info.icon;
                    }
                }
            }
        }
 
        // Gán số lượng
        expAmount.text = exp.ToString();
        hepiAmount.text = hepi.ToString();
        HpTxt.text = currentData.hp.ToString();

        needRegenTxt.text = UpgradeManager.Instance.GetRengenMP().ToString();
        expImg.sprite = currentExpImg.sprite =  ResourceManager.Instance.resourceInfos.Find(r =>
                    r.resourceType == 2 && r.resourceId == currentID).icon;
    
        currentMasterImg.sprite = ResourceManager.Instance.resourceInfos.Find(r =>
                    r.resourceType == 3 && r.resourceId == currentID).icon;
        currentHepiAmount.text = ResourceManager.Instance.GetQuantity(0, 1).ToString();
        currentExpAmount.text = ResourceManager.Instance.GetQuantity(2, currentID).ToString();
        currentMasterAmount.text = ResourceManager.Instance.GetQuantity(3, currentID).ToString();
        UpdateMP(currentData);
        // Màu mặc định và màu đỏ
        Color normalColor = Color.black;
        Color redColor = Color.red;
        maxLevel = ReadCSVDataHeroStat.instance.GetMaxLevel(currentID);
        // Đổi màu text nếu thiếu tài nguyên
        hepiAmount.color = hasEnoughHepi ? normalColor : redColor;
        expAmount.color = hasEnoughExp ? normalColor : redColor;
        needRegenTxt.color = hasRegen ? normalColor : redColor;
        currentMasterAmount.color = hasEnoughMastery ? normalColor : redColor;

        // Cập nhật trạng thái nút
        bool needRegen=true;
        needRegen = HeroManager.instance.heroDatas.Find(h => h.id == currentID).currentMP < currentData.masteryPoint;
        bool canUpgrade = hasEnoughHepi && hasEnoughExp && hasEnoughMastery && upgradeData!=null;
        bool canRegen = hasRegen && needRegen;
        levelupBtn.interactable = canUpgrade;
        regenBtn.interactable = canRegen;
        regenBtn.image.sprite = canRegen ? levelupActive : levelupNotActive;
        levelupBtn.image.sprite = canUpgrade ? levelupActive : levelupNotActive;
        if (!canUpgrade)
        {
            popupUpgrade.SetActive(false);
        }
        SetlevelUpData(currentData, upgradeData);
        UpdateLevelCircle(currentData.level);
    }
    public void UpdateMP(RiderUpgradeLevel currentData)
    
    {
        maxMp = currentData.masteryPoint; 
        currentMP = HeroManager.instance.heroDatas.Find(h => h.id == currentID).currentMP;
        if (maxMp <= 0) {
            mpBar.value = 0;
            mpBarFillTxt.text = "0MP";
            return;
        }
        float fillAmount = Mathf.Clamp01((float)currentMP / maxMp);

        mpBar.value = fillAmount;
        mpBarFillTxt.text = currentMP+"/"+maxMp+ " " +LocalizationManager.instance.GetLocalizedText("rider_stat_type_2")  ;
        if (currentMP < 1)
        {
            mpBarFillObject.color = new Color32(0xFF, 0x00, 0x00, 0xFF); // đỏ #FF0000
        }
        else if (currentMP >= 1 && currentMP < maxMp)
        {
            mpBarFillObject.color = new Color32(0xFF, 0xAE, 0x00, 0xFF); // cam #FFAE00
        }
        else if (currentMP >= maxMp)
        {
            mpBarFillObject.color = new Color32(0x48, 0xFF, 0x05, 0xFF); // xanh #48FF05
        }
        int missing = maxMp - currentMP;
        if (currentMP< maxMp)
        {
            regenAmount = 1;
        }
        currentRegenTxt.text = LocalizationManager.instance.GetLocalizedText("rider_stat_type_2")+": "+currentMP.ToString();
        regenAmountTxt.text = (UpgradeManager.Instance.GetRengenMP()).ToString();
        maxRegenTxt.text = maxMp.ToString();
    }
    public void SetlevelUpData(RiderUpgradeLevel currentData, RiderUpgradeLevel upgradeData)
    {
        var hero = HeroManager.instance.GetHero(this.currentID);
        if (upgradeData == null)
            return;
        successRatetxt.text = upgradeData.upgradeRate*100 + "%";
        currentHpTxt.text = LocalizationManager.instance.GetLocalizedText("rider_stat_type_1") + " " +currentData.hp.ToString() ;
        currentMpTxt.text = LocalizationManager.instance.GetLocalizedText("rider_stat_type_2") + " " + currentData.masteryPoint.ToString();
        upgradeHpTxt.text = upgradeData.hp.ToString() ;
        upgradeMpTxt.text = upgradeData.masteryPoint.ToString();
        heroNameLeveluptxt.text = LocalizationManager.instance.GetLocalizedText("button_level_up")+ ":" + hero.Value.name.ToUpper();
        upgradeHepiAmount.text = hepiAmount.text;
        upgradeExpAmount.text = expAmount.text;
        foreach (var req in upgradeData.upgradeRequirements)
        {
            var info = ResourceManager.Instance.resourceInfos.Find(r =>
                r.resourceType == req.resourceType && r.resourceId == req.resourceId);

           if (req.resourceType == 3)
           {
                upgradeMasterAmount.text = req.amount.ToString();

                
           }
        }
        
        upgradeExpImg.sprite = expImg.sprite;
        upgradeMasterImg.sprite= currentMasterImg.sprite;

    }
    public void OnPlusClicked()
    {
        int missing = maxMp - currentMP;

        if (regenAmount < missing)
        {
            regenAmount++;
            regenAmountTxt.text = (UpgradeManager.Instance.GetRengenMP() * regenAmount).ToString();
        }
    }

    public void OnMinusClicked()
    {
        regenAmount--;
        regenAmountTxt.text = (UpgradeManager.Instance.GetRengenMP() * regenAmount).ToString();
        if (regenAmount < 1)
        {
            regenAmount = maxMp - currentMP;
            regenAmountTxt.text = (UpgradeManager.Instance.GetRengenMP() * regenAmount).ToString();
        }
    }
    public void OnClickRegen()
    {
        int missing = maxMp - currentMP;
        if (missing == 0)
        {
            NotiManager.instance.ShowNotification("MP is full");
            return;
        }

        UpgradeManager.Instance.RegenMP(this.currentID,regenAmount);
        ResourceManager.Instance.ConsumeResource(0, 1, (int)UpgradeManager.Instance.GetRengenMP() * regenAmount);
        currentMP = HeroManager.instance.heroDatas.Find(h => h.id == currentID).currentMP;
        currentRegenTxt.text = LocalizationManager.instance.GetLocalizedText("rider_stat_type_2") + ": " + currentMP.ToString();
    }
    private void UpdateLevelCircle(int currentLevel)
    {
        if (maxLevel <= 0) return;                 // tránh chia 0
        float ratio = Mathf.Clamp01((float)currentLevel / maxLevel);

        levelCircle.fillAmount = ratio;
        float fill = levelCircle.fillAmount;
        float angle = fill * 180f;
        if (levelCircle.fillClockwise)
            angle = -angle;

        Vector2 dir = AngleToVector(angle);
        float r = levelCircle.rectTransform.rect.width * 0.5f;
        levelPointer.anchoredPosition = dir * r;
    }

    Vector2 AngleToVector(float angle)
    {
        float rad = angle * Mathf.Deg2Rad;
        return new Vector2(Mathf.Sin(rad), Mathf.Cos(rad));
    }

    public void UpgradeHero()
    {
        bool isupgrade=UpgradeManager.Instance.TryUpgradeHero(currentID);

        if (isupgrade)
        {
            NotiManager.instance.ShowNotification(LocalizationManager.instance.GetLocalizedText("level_up_succeeded"));

            int newLevel = HeroManager.instance.GetHero(currentID).Value.level; // Giả sử bạn có field `level`
            var newCurrentData = ReadCSVDataHeroStat.instance.GetHeroLevelData(currentID, newLevel);
            var newUpgradeData = ReadCSVDataHeroStat.instance.GetHeroLevelData(currentID, newLevel + 1);
            UpdateLevelCircle(newCurrentData.level);
            UpdateMP(newCurrentData);
            if (newCurrentData != null && newUpgradeData != null)
            {
                // Gọi lại hàm để update UI
                SetUpgradeView(newCurrentData, newUpgradeData);
            }
            else
            {
                // Nếu không còn cấp độ mới → disable nút nâng cấp hoặc ẩn popup
                
                levelupBtn.interactable = false;
                levelupBtn.image.sprite = levelupNotActive;
                popupUpgrade.SetActive(false);
                NotiManager.instance.ShowNotification("Hero is at max level");
            }
        }
        else
        {
            NotiManager.instance.ShowNotification(LocalizationManager.instance.GetLocalizedText("level_up_failed"));
        }
    }
}
