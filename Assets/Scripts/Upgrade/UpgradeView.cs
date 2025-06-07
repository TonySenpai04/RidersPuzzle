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
        ApplyText.instance.UpdateUpgrade(currentID);

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
                    currentExpImg.sprite = info.icon;
                }
            }
            else if (req.resourceType == 3)
            {
                mastery = req.amount;
                hasEnoughMastery = ResourceManager.Instance.HasEnough(req.resourceType, req.resourceId, req.amount);

                if (info != null)
                {
                    currentMasterImg.sprite = info.icon;
                }
            }
        }

        // Gán số lượng
        expAmount.text = exp.ToString();
        hepiAmount.text = hepi.ToString();
        HpTxt.text = currentData.hp.ToString();


        currentHepiAmount.text = ResourceManager.Instance.GetQuantity(0, 1).ToString();
        currentExpAmount.text = ResourceManager.Instance.GetQuantity(2, currentID).ToString();
        currentMasterAmount.text = ResourceManager.Instance.GetQuantity(3, currentID).ToString();

        // Màu mặc định và màu đỏ
        Color normalColor = Color.black;
        Color redColor = Color.red;

        // Đổi màu text nếu thiếu tài nguyên
        hepiAmount.color = hasEnoughHepi ? normalColor : redColor;
        currentHepiAmount.color = hasEnoughHepi ? normalColor : redColor;

        expAmount.color = hasEnoughExp ? normalColor : redColor;
        currentExpAmount.color = hasEnoughExp ? normalColor : redColor;

        currentMasterAmount.color = hasEnoughMastery ? normalColor : redColor;

        // Cập nhật trạng thái nút
        bool canUpgrade = hasEnoughHepi && hasEnoughExp && hasEnoughMastery;
        levelupBtn.onClick.AddListener(()=>UpgradeManager.Instance.TryUpgradeHero(currentID));
        levelupBtn.interactable = canUpgrade;
        levelupBtn.image.sprite = canUpgrade ? levelupActive : levelupNotActive;
        SetlevelUpData(currentData, upgradeData);
    }

    public void SetlevelUpData(RiderUpgradeLevel currentData, RiderUpgradeLevel upgradeData)
    {
        var hero = HeroManager.instance.GetHero(this.currentID);
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
}
