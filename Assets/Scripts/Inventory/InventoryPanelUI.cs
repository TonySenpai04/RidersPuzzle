using TMPro;
using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

public class InventoryPanelUI : MonoBehaviour
{
    [SerializeField] Transform contentRoot;    
    [SerializeField] GameObject itemCellPrefab;
    [SerializeField] GameObject itemInfo;
    [SerializeField] Image itemInfoImg;
    // cache để update nhanh không tạo‑xoá mỗi lần
    readonly Dictionary<(int, int), InventoryUIItem> lookup = new();
    [SerializeField] List<InventoryUIItem> items;
    [SerializeField] TextMeshProUGUI nameItemTxt;
    [SerializeField] TextMeshProUGUI desItemTxt;
    void Start()
    {
        BuildResourceCells();                                  
        
    }
    private void OnEnable()
    {
        if (items.Count <= 0)
            return;
        RefreshAll();
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
    void BuildResourceCells()
    {
        var rm = ResourceManager.Instance;

        foreach (var info in rm.resourceInfos)
        {
            var key = info.GetKey();
            int qty = rm.GetQuantity(key.Item1, key.Item2);
            if (qty <= 0) qty = 0;         // vẫn tạo ô, lát nữa SetActive

            var go = Instantiate(itemCellPrefab, contentRoot);
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
        // Tìm metadata của resource vừa click
        var info = ResourceManager.Instance.resourceInfos
                   .Find(r => r.resourceType == key.Item1 &&
                              r.resourceId == key.Item2);

        if (info == null) return;        // đề phòng lỗi cấu hình

        itemInfoImg.sprite = info.icon;  // gán icon
                                         // Nếu muốn đổi cả tên, thêm TMP_Text và gán info.resourceName ở đây
                                         // itemInfoName.text = info.resourceName;

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
            desItemTxt.text= LocalizationManager.instance.GetLocalizedText(usageExpKey, heroName);
        }
        else if(key.Item1 == 3)
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

}
