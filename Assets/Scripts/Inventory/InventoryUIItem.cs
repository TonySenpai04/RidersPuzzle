using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class InventoryUIItem : MonoBehaviour
{
    [SerializeField] Image iconImg;
    [SerializeField] TMP_Text qtyTxt;
    [SerializeField] Button button;
    public (int, int) Key { get; private set; }
    public void Init((int, int) key, Sprite icon, int qty, Action<(int, int)> onClick)
    {
        Key = key;
        SetIcon(icon);
        SetQuantity(qty);
        button.onClick.AddListener(() => onClick?.Invoke(Key));
    }
    public void SetIcon(Sprite s) => iconImg.sprite = s;

    public void SetQuantity(int qty)
    {
        qtyTxt.text = qty >= 1_000_000 ? $"{(qty / 1_000_000f):0.#}M"
                 : qty >= 1_000 ? $"{(qty / 1_000f):0.#}K"
                 : qty.ToString();

        qtyTxt.enabled = qty > 1;   // ẩn số lượng nếu =1
    }
}
