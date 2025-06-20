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

        itemInfo.SetActive(true);        // bật khung thông tin
    }

}
