using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
public class ButtonStage : MonoBehaviour
{
    [SerializeField] private Button button;
    [SerializeField] private Image lockImage; // Ảnh khóa hiển thị

    private int levelIndex;
    private System.Action<int> onClick;

    public void Initialize(int index, System.Action<int> clickCallback, bool isUnlocked)
    {
        levelIndex = index;
        onClick = clickCallback;
        SetButtonState(isUnlocked);
        button.onClick.AddListener(OnButtonClick);
    }

    private void OnButtonClick()
    {
        onClick?.Invoke(levelIndex);
    }

    // Cập nhật trạng thái button
    public void SetButtonState(bool isUnlocked)
    {
        button.interactable = isUnlocked; // Vô hiệu hóa button nếu chưa mở khóa
        lockImage.gameObject.SetActive(!isUnlocked); // Hiển thị hình khóa nếu chưa mở khóa
    }
}