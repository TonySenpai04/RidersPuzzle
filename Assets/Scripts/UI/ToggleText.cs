using UnityEngine;
using UnityEngine.UI;
using TMPro; 

public class ToggleText : MonoBehaviour
{
    public Button toggleButton;
    public TextMeshProUGUI targetText; 

    private bool isTextVisible = false;

    void Start()
    {
        toggleButton.onClick.AddListener(ToggleTextVisibility);
        targetText.gameObject.SetActive(isTextVisible);
    }

    void ToggleTextVisibility()
    {
        isTextVisible = !isTextVisible;
        targetText.gameObject.SetActive(isTextVisible);
    }
}
