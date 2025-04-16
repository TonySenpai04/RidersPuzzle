using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class SkillFushionButton:MonoBehaviour
{
    [SerializeField] private int index;
    [SerializeField] private Button button;
    [SerializeField] private TextMeshProUGUI skillName;
    [SerializeField] private GameObject highlightBorder;
    public void SetData(int index, string name, System.Action<int> onClickAction)
    {
        this.index = index;
        this.skillName.text= name;
        button.onClick.AddListener(() => onClickAction(index));
        SetHighlight(false);
    }
    public void SetHighlight(bool isOn)
    {
        if (highlightBorder != null)
        {
            highlightBorder.SetActive(isOn);
        }
    }

    public int GetID() => index;
}