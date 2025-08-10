using UnityEngine;
using TMPro;

[RequireComponent(typeof(TextMeshProUGUI))]
public class AutoWidthTMP : MonoBehaviour
{
    public float padding = 10f;

    private TextMeshProUGUI tmp;
    private RectTransform rectTransform;

    void Awake()
    {
        tmp = GetComponent<TextMeshProUGUI>();
        rectTransform = GetComponent<RectTransform>();
    }

    void Update()
    {
        float newWidth = tmp.preferredWidth + padding;
        rectTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, newWidth);
    }
}
