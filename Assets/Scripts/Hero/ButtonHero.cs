using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class ButtonHero:MonoBehaviour
{
    public int Index { get; private set; }
    [SerializeField] private Image heroIcon;
    [SerializeField] private Image lockImage; // Hình ảnh khóa
    [SerializeField] private Image infoImage; // Hình ảnh thông tin
    [SerializeField] private Image selectedImage; // Hình ảnh đã chọn
    private Button button;
    [SerializeField] private Button infoBtn;
    public bool isUnlocked { get; private set; }

    private void Awake()
    {
        button = GetComponent<Button>();
    }

    public void Initialize(int index, System.Action<int> onClickAction, bool isUnlocked, bool isSelected,Sprite heroIcon)
    {
        Index = index;
        this.heroIcon.sprite=heroIcon;
        UpdateButtonState(isUnlocked, isSelected);
        infoBtn.onClick.AddListener(() => HeroInfoManager.instance.ShowInfo(this.Index, infoBtn.GetComponent<RectTransform>()));
        infoBtn.onClick.AddListener(() => SoundManager.instance.PlaySFX("Click Sound"));
        button.onClick.AddListener(() => onClickAction(Index));
        button.onClick.AddListener(() => SoundManager.instance.PlaySFX("Click Sound"));
    }

    public void UpdateButtonState(bool isUnlocked, bool isSelected)
    {
        this.isUnlocked = isUnlocked;
        lockImage.gameObject.SetActive(!isUnlocked); 
        infoImage.gameObject.SetActive(isUnlocked);
        selectedImage.gameObject.SetActive(isSelected && isUnlocked);
         
    }
}