using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class CenterTextIcon : MonoBehaviour
{
    public Image icon; // Icon dạng UI Image
    public TextMeshProUGUI textMeshPro; // Text dạng UI
    public float spacing = 10f; // Khoảng cách giữa icon và text

    void Update()
    {
        if (icon == null || textMeshPro == null) return;

        // Lấy kích thước text
        float textWidth = textMeshPro.preferredWidth;
        float iconWidth = icon.rectTransform.sizeDelta.x;

        // Căn chỉnh icon và text
        icon.rectTransform.anchoredPosition = new Vector2(-textWidth / 2 - spacing, 0);
        textMeshPro.rectTransform.anchoredPosition = new Vector2(iconWidth / 2, 0);
    }
}
