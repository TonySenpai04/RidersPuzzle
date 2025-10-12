using UnityEngine;
using UnityEngine.EventSystems;

public class CloseOnBackgroundClick : MonoBehaviour, IPointerClickHandler
{
    public GameObject panelToHide;

    public void OnPointerClick(PointerEventData eventData)
    {
        if (eventData.pointerEnter == gameObject)
        {
            panelToHide.SetActive(false);
        }
    }
}
