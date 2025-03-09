using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class DropdownPanelController : MonoBehaviour
{
    public RectTransform imageContainer; // Hình ảnh chứa button
    public Button[] buttons; // Danh sách button
    private bool isShrunk = false; // Trạng thái hiện tại
    private Vector2 originalSize; // Kích thước ban đầu
    private Vector2 shrunkSize; // Kích thước sau khi thu nhỏ
    private Vector3 originalPos; // Vị trí ban đầu
    private Vector3 shrunkPos; // Vị trí sau khi thu nhỏ (co lên trên)
    private Button activeButton; // Button được click

    private void Start()
    {
        // Lưu kích thước & vị trí ban đầu
        originalSize = imageContainer.sizeDelta;
        shrunkSize = new Vector2(originalSize.x, originalSize.y / 3);
        originalPos = imageContainer.anchoredPosition;
        shrunkPos = originalPos + new Vector3(0, (originalSize.y - shrunkSize.y)/2, 0);
        Debug.Log(originalPos);

        // Gán sự kiện click cho từng button
        foreach (Button btn in buttons)
        {
            btn.onClick.AddListener(() => ToggleImageSize(btn));
        }
        ToggleImageSize(buttons[0]);
    }

    private void ToggleImageSize(Button clickedButton)
    {
        if (isShrunk && activeButton == clickedButton) // Nếu đang thu nhỏ và nhấn lại button đó
        {
            StartCoroutine(ExpandImage());
            activeButton = null;
        }
        else // Nếu chưa thu nhỏ hoặc nhấn vào button khác
        {
            activeButton = clickedButton;
            MoveButtonToTop(clickedButton); // Đưa button lên trên cùng
            StartCoroutine(ShrinkImage());
        }
        isShrunk = !isShrunk;
    }

    private IEnumerator ShrinkImage()
    {
        float duration = 0.3f;
        float elapsed = 0f;

        // Ẩn tất cả button TRỪ button vừa nhấn
        foreach (Button btn in buttons)
        {
            if (btn != activeButton)
                btn.gameObject.SetActive(false);
        }

        while (elapsed < duration)
        {
            elapsed += Time.deltaTime;
            float t = elapsed / duration;
            imageContainer.sizeDelta = Vector2.Lerp(originalSize, shrunkSize, t);
            imageContainer.anchoredPosition = Vector3.Lerp(originalPos, shrunkPos, t);
            yield return null;
        }

        imageContainer.sizeDelta = shrunkSize;
        imageContainer.anchoredPosition = shrunkPos;
    }

    private IEnumerator ExpandImage()
    {
        float duration = 0.3f;
        float elapsed = 0f;

        // Hiện lại tất cả button
        foreach (Button btn in buttons)
        {
            btn.gameObject.SetActive(true);
        }

        while (elapsed < duration)
        {
            elapsed += Time.deltaTime;
            float t = elapsed / duration;
            imageContainer.sizeDelta = Vector2.Lerp(shrunkSize, originalSize, t);
            imageContainer.anchoredPosition = Vector3.Lerp(shrunkPos, originalPos, t);
            yield return null;
        }

        imageContainer.sizeDelta = originalSize;
        imageContainer.anchoredPosition = originalPos;
    }

    private void MoveButtonToTop(Button clickedButton)
{
    // Tìm vị trí của button được click trong danh sách
    int clickedIndex = System.Array.IndexOf(buttons, clickedButton);

    if (clickedIndex > 0) // Chỉ đổi nếu không phải button đầu
    {
        // Lấy button đầu tiên (cũ)
        Button firstButton = buttons[0];

        // Hoán đổi vị trí trong danh sách
        buttons[0] = clickedButton;
        buttons[clickedIndex] = firstButton;

        // Lấy RectTransform của button đầu & button click
        RectTransform clickedTransform = clickedButton.GetComponent<RectTransform>();
        RectTransform firstTransform = firstButton.GetComponent<RectTransform>();

        // Lưu lại vị trí cũ của button đầu
        Vector2 firstOldPos = firstTransform.anchoredPosition;
        Vector2 clickedOldPos = clickedTransform.anchoredPosition;

        // Đổi vị trí trong Hierarchy
        clickedTransform.SetSiblingIndex(0);
        firstTransform.SetSiblingIndex(clickedIndex);

        // Đổi vị trí RectTransform để đảm bảo hiển thị đúng
        clickedTransform.anchoredPosition = firstOldPos;
        firstTransform.anchoredPosition = clickedOldPos;
    }
}


}
