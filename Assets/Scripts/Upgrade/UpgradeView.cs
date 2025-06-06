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
  //  public Image masterImg;
    public int currentID;
    public TextMeshProUGUI heroNametxt;
    public Image heroImg;
    public void SetUpgradeView(RiderUpgradeLevel upgradeData)
    {
        int hepi = 0;
        int exp = 0;
        int mastery = 0;
        var hero = HeroManager.instance.GetHero(this.currentID);
        this.heroImg.sprite = hero.Value.heroImage;
        this.heroNametxt.text = $"{hero.Value.name}";
        ApplyText.instance.UpdateUpgrade(currentID);
        foreach (var req in upgradeData.upgradeRequirements)
        {
            if (req.resourceType == 0) 
            {
                hepi = req.amount;

                var info = ResourceManager.Instance.resourceInfos.Find(r =>
                    r.resourceType == req.resourceType && r.resourceId == req.resourceId);
               
                 
            }
            else if (req.resourceType == 2) // Hero EXP
            {
                exp = req.amount;
                var info = ResourceManager.Instance.resourceInfos.Find(r =>
                    r.resourceType == req.resourceType && r.resourceId == req.resourceId);
                if (info != null)
                {
                    expImg.sprite = info.icon;
                    currentExpImg.sprite = info.icon;
                }
            }
            else if (req.resourceType == 3) // Mastery
            {
                mastery = req.amount;
                var info = ResourceManager.Instance.resourceInfos.Find(r =>
                    r.resourceType == req.resourceType && r.resourceId == req.resourceId);
                if (info != null)
                {
                    currentMasterImg.sprite = info.icon;
                }
            }
        }

        expAmount.text = exp.ToString();
        hepiAmount.text = hepi.ToString();
        
        currentHepiAmount.text = ResourceManager.Instance.GetQuantity(0,1).ToString();
        currentExpAmount.text = ResourceManager.Instance.GetQuantity(2, currentID).ToString();
        currentMasterAmount.text = ResourceManager.Instance.GetQuantity(3, currentID).ToString();
        


    }
}
