using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class HardLockedScrollRect : ScrollRect
{
    public override void OnInitializePotentialDrag(PointerEventData eventData)
    {
        base.OnInitializePotentialDrag(eventData);

        // Nếu content không đủ để scroll → tắt inertia để không có trượt
        if (content.rect.height <= viewport.rect.height)
        {
            inertia = false;
        }
    }

    public override void OnDrag(PointerEventData eventData)
    {
        // Không đủ nội dung → không cho kéo
        if (content.rect.height <= viewport.rect.height)
        {
            return;
        }

        // Nếu đang ở top và kéo xuống → chặn
        if (vertical && verticalNormalizedPosition >= 0.999f && eventData.delta.y < 0)
        {
            return;
        }

        // Nếu đang ở bottom và kéo lên → chặn
        if (vertical && verticalNormalizedPosition <= 0.001f && eventData.delta.y > 0)
        {
            return;
        }

        base.OnDrag(eventData);
    }
}
