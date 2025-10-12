using TMPro;
using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

public class InventoryPanelUI : MonoBehaviour
{
    [SerializeField] Transform contentRoot;
    [SerializeField] GameObject itemNotActiveCellPrefab;
    [SerializeField] GameObject itemTrialHeroCellPrefab;
    [SerializeField] GameObject itemInfo;
    [SerializeField] GameObject itemActiveInfo;
    [SerializeField] GameObject confirmTrialHero;
    [SerializeField] GameObject confirmTrialHeroUnlocked;
    [SerializeField] Image itemInfoImg;
    // cache để update nhanh không tạo‑xoá mỗi lần
    readonly Dictionary<(int, int), InventoryUIItem> lookup = new();
    [Header("Item Not Active Field")]
    [SerializeField] List<InventoryUIItem> items;
    [SerializeField] TextMeshProUGUI nameItemTxt;
    [SerializeField] TextMeshProUGUI desItemTxt;
    [Header("Item Trail Hero Field")]

    [SerializeField] int cuurentTrialHeroId;
    [SerializeField] TextMeshProUGUI nameHeroTxt;
    [SerializeField] TextMeshProUGUI totalItemTxt;
    [SerializeField] TextMeshProUGUI quantityItemTxt;
    [SerializeField] TextMeshProUGUI curentQuantityItemTxt;
    [SerializeField] Image heroIcon;
    [SerializeField] TextMeshProUGUI quantityItemTxtHeroCard;
    [SerializeField] Slider slide;
    [SerializeField] int currentQuantity;
    [SerializeField] int maxQuantity;
    [Header("Confirm Trail Hero Field")]
    [SerializeField] TextMeshProUGUI trialTimeTxt;
    [SerializeField] Image heroTrialIcon;
    [SerializeField] TextMeshProUGUI trialLevelTxt;
    [Header("Confirm Trail Hero Unlocked Field")]
    [SerializeField] TextMeshProUGUI hepiTxt;


    void Start()
    {
        BuildResourceCells();

    }
    private void OnEnable()
    {
        if (items.Count <= 0)
            return;
        RefreshAll();
        RefreshPanel();
    }
    void RefreshAll()
    {
        var rm = ResourceManager.Instance;

        foreach (var cell in items)
        {
            int qty = rm.GetQuantity(cell.Key.Item1, cell.Key.Item2);
            cell.gameObject.SetActive(qty > 0);
            if (qty > 0) cell.SetQuantity(qty);
        }
    }
    void RefreshPanel()
    {
        itemInfo.SetActive(false);
        itemActiveInfo.SetActive(false);
        confirmTrialHero.SetActive(false);
        confirmTrialHeroUnlocked.SetActive(false);


    }
    void BuildResourceCells()
    {
        var rm = ResourceManager.Instance;

        foreach (var info in rm.resourceInfos)
        {
            var key = info.GetKey();
            int qty = rm.GetQuantity(key.Item1, key.Item2);
            if (qty <= 0) qty = 0;         // vẫn tạo ô, lát nữa SetActive
            GameObject go = null;
            if (key.Item1 != 4)
            {
                go = Instantiate(itemNotActiveCellPrefab, contentRoot);
            }
            else
            {
                go = Instantiate(itemTrialHeroCellPrefab, contentRoot);
            }
            var cell = go.GetComponent<InventoryUIItem>();

            cell.Init(key, info.icon, qty, OnItemClicked);
            go.SetActive(qty > 0);

            lookup[key] = cell;
            items.Add(cell);
        }
    }

    void OnResChanged((int, int) key, int newQty)
    {
        if (lookup.TryGetValue(key, out var cell))
            cell.SetQuantity(newQty);
        else
            Debug.LogWarning($"Chưa có info cho resource {key}");
    }
    void OnItemClicked((int, int) key)
    {
        if (key.Item1 == 4)
        {
            cuurentTrialHeroId = key.Item2;
            itemActiveInfo.SetActive(true);
            heroIcon.sprite = ResourceManager.Instance.resourceInfos
                   .Find(r => r.resourceType == key.Item1 &&
                              r.resourceId == key.Item2).icon;
            nameHeroTxt.text = LocalizationManager.instance.GetLocalizedText($"hero_name_{key.Item2}", "Hero");
            int qty = ResourceManager.Instance.GetQuantity(key.Item1, key.Item2);
            currentQuantity = qty;
            maxQuantity = qty;
            slide.minValue = 0;
            slide.maxValue = currentQuantity;
            slide.value = maxQuantity;
            quantityItemTxt.SetText($"<color=#FF0000>{maxQuantity}</color> <color=#000000>day(s)</color>");
            curentQuantityItemTxt.SetText(
              $"Quantity: <color=#00FF00>{currentQuantity}</color>/<color=#FF0000>{maxQuantity}</color>"
                  );

            quantityItemTxtHeroCard.text = currentQuantity.ToString();
            return;

        }
        // Tìm metadata của resource vừa click
        var info = ResourceManager.Instance.resourceInfos
                   .Find(r => r.resourceType == key.Item1 &&
                              r.resourceId == key.Item2);

        if (info == null) return;        // đề phòng lỗi cấu hình

        itemInfoImg.sprite = info.icon;

        itemInfo.SetActive(true);
        string expKey = $"exp_{key.Item2}";
        string usageExpKey = $"usage_exp_{key.Item2}";
        string heroNameKey = $"hero_name_{key.Item2}";
        string mpKey = $"mp_{key.Item2}";
        string usageMpKey = $"usage_mp_{key.Item2}";
        string hapikey = $"resource_type_{key.Item1}_id_{key.Item2}";
        string usageHapikey = $"usage_resource_type_{key.Item1}_id_{key.Item2}";
        string heroName = LocalizationManager.instance.GetLocalizedText(heroNameKey, "Hero");
        if (key.Item1 == 2)
        {


            nameItemTxt.text = LocalizationManager.instance.GetLocalizedText(expKey, heroName);
            desItemTxt.text = LocalizationManager.instance.GetLocalizedText(usageExpKey, heroName);
        }
        else if (key.Item1 == 3)
        {

            nameItemTxt.text = LocalizationManager.instance.GetLocalizedText(mpKey, heroName);
            desItemTxt.text = LocalizationManager.instance.GetLocalizedText(usageMpKey, heroName);
        }
        else
        {
            nameItemTxt.text = LocalizationManager.instance.GetLocalizedText(hapikey);
            desItemTxt.text = LocalizationManager.instance.GetLocalizedText(usageHapikey);
        }
    }
    public void OnConfirmTrialHero()
    {
        var heroDatas = HeroManager.instance.heroDatas;
        int index = heroDatas.FindIndex(h => h.id == cuurentTrialHeroId);
        if (index == -1)
        {
            Debug.LogWarning("❌ Không tìm thấy hero.");
            return;
        }
        DataHero heroData = heroDatas[index];
        if (heroData.isUnlock)
        {
            confirmTrialHeroUnlocked.SetActive(true);
            ResourceManager.Instance.ConsumeResource(4, cuurentTrialHeroId, (int)slide.value);
            hepiTxt.text = ((int)slide.value * 200).ToString();
            ResourceManager.Instance.AddResource(0, 1, (int)slide.value * 200);
            itemActiveInfo.SetActive(false);
            RefreshAll();
            return;
        }
        HeroManager.instance.StartHeroTrial(cuurentTrialHeroId, (int)slide.value);
        confirmTrialHero.SetActive(true);
        itemActiveInfo.SetActive(false);
        heroTrialIcon.sprite = heroIcon.sprite;
        trialTimeTxt.SetText($"<color=#000000>Time remaining:</color><color=#FF0000>{HeroManager.instance.GetTrialRemainingTimeFull(cuurentTrialHeroId)}</color> ");
        trialLevelTxt.SetText("Level:1");
        RefreshAll();

    }

    public void IncreaseCurrentQuantity()
    {
        if (currentQuantity < maxQuantity)
        {
            currentQuantity++;
            slide.value = currentQuantity;
            quantityItemTxt.SetText($"<color=#FF0000>{maxQuantity}</color> <color=#000000>day(s)</color>");
            curentQuantityItemTxt.SetText(
             $"Quantity: <color=#00BFFF>{currentQuantity}</color><color=#FF0000>/{maxQuantity}</color>"
                 );
        }
    }

    public void DecreaseCurrentQuantity()
    {
        if (currentQuantity > 1)
        {
            currentQuantity--;
            slide.value = currentQuantity;
            quantityItemTxt.SetText($"<color=#FF0000>{maxQuantity}</color> <color=#000000>day(s)</color>");
            curentQuantityItemTxt.SetText(
            $"Quantity: <color=#00BFFF>{currentQuantity}</color><color=#FF0000>/{maxQuantity}</color>"
                );
        }
    }

    // ...existing code...
}
