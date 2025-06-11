
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
    
    public void SetUpgradeView(RiderUpgradeLevel currentData, RiderUpgradeLevel upgradeData)
    {
        int hepi = 0;
        int exp = 0;
        int mastery = 0;

        bool hasEnoughHepi = true;
        bool hasEnoughExp = true;
        bool hasEnoughMastery = true;

        var hero = HeroManager.instance.GetHero(this.currentID);
        this.heroImg.sprite = hero.Value.heroImage;
        this.heroNametxt.text = $"{hero.Value.name}";
        //  ApplyText.instance.UpdateUpgrade(currentID);
        ApplyTextManager.instance.UpdateUpgrade(currentID);
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

        expImg.sprite = currentExpImg.sprite =  ResourceManager.Instance.resourceInfos.Find(r =>
                    r.resourceType == 2 && r.resourceId == currentID).icon;
    
        currentMasterImg.sprite = ResourceManager.Instance.resourceInfos.Find(r =>
                    r.resourceType == 3 && r.resourceId == currentID).icon;
        currentHepiAmount.text = ResourceManager.Instance.GetQuantity(0, 1).ToString();
        currentExpAmount.text = ResourceManager.Instance.GetQuantity(2, currentID).ToString();
        currentMasterAmount.text = ResourceManager.Instance.GetQuantity(3, currentID).ToString();
        UpdateMPBar(currentData);
        // Màu mặc định và màu đỏ
        Color normalColor = Color.black;
        Color redColor = Color.red;

        // Đổi màu text nếu thiếu tài nguyên
        hepiAmount.color = hasEnoughHepi ? normalColor : redColor;
  

        expAmount.color = hasEnoughExp ? normalColor : redColor;
     
        currentMasterAmount.color = hasEnoughMastery ? normalColor : redColor;
       
        // Cập nhật trạng thái nút
        bool canUpgrade = hasEnoughHepi && hasEnoughExp && hasEnoughMastery && upgradeData!=null;
       
        levelupBtn.interactable = canUpgrade;
        levelupBtn.image.sprite = canUpgrade ? levelupActive : levelupNotActive;
        SetlevelUpData(currentData, upgradeData);
    }
    public void UpdateMPBar(RiderUpgradeLevel currentData)
    
    {
        maxMp = currentData.masteryPoint; 
        currentMP = HeroManager.instance.heroDatas.Find(h => h.id == currentID).currentMP;
        if (maxMp <= 0) {
            mpBar.value = 0;
            mpBarFillTxt.text = "";
            return;
        }
        float fillAmount = Mathf.Clamp01((float)currentMP / maxMp);

        mpBar.value = fillAmount;
        mpBarFillTxt.text = currentMP+"/"+maxMp+" MP";
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
    }
    public void SetlevelUpData(RiderUpgradeLevel currentData, RiderUpgradeLevel upgradeData)
    {
        var hero = HeroManager.instance.GetHero(this.currentID);
        if (upgradeData == null)
            return;
        successRatetxt.text = upgradeData.upgradeRate*100 + "%";
        currentHpTxt.text ="HP "+currentData.hp.ToString() ;
        currentMpTxt.text = "MP " + currentData.masteryPoint.ToString();
        upgradeHpTxt.text = upgradeData.hp.ToString() ;
        upgradeMpTxt.text = upgradeData.masteryPoint.ToString();
        heroNameLeveluptxt.text = "LEVEL UP:" + hero.Value.name.ToUpper();
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
 
    public void UpgradeHero()
    {
        bool isupgrade=UpgradeManager.Instance.TryUpgradeHero(currentID);

        if (isupgrade)
        {
            NotiManager.instance.ShowNotification("Level up succeeded");

            int newLevel = HeroManager.instance.GetHero(currentID).Value.level; // Giả sử bạn có field `level`
            var newCurrentData = ReadCSVDataHeroStat.instance.GetHeroLevelData(currentID, newLevel);
            var newUpgradeData = ReadCSVDataHeroStat.instance.GetHeroLevelData(currentID, newLevel + 1);
            UpdateMPBar(newCurrentData);
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
            NotiManager.instance.ShowNotification("Level up failed");
        }
    }
}
