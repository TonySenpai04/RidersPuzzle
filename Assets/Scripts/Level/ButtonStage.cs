using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
public class ButtonStage : MonoBehaviour
{
    [SerializeField] private Button button;
    [SerializeField] private Image Image;
    [SerializeField] private Sprite lockImage;
    [SerializeField] private Sprite UnlockImage;
    [SerializeField] private int levelIndex;
    [SerializeField] public bool isUnlocked;
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

    public void SetButtonState(bool isUnlocked)
    {
       this.isUnlocked = isUnlocked;
        Image.sprite = isUnlocked ? UnlockImage : lockImage;
        button.onClick.RemoveAllListeners(); 

        if (isUnlocked)
        {
            button.onClick.AddListener(OnButtonClick);
        }
        else
        {
            button.onClick.AddListener(() => Debug.Log($"Level {levelIndex} chưa được mở khóa!")); 
        }
    }
}