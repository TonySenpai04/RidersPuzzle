using UnityEngine;

[RequireComponent(typeof(Canvas))]
public class FixedHeightUICamera : MonoBehaviour
{
    public float targetAspect = 9f / 16f; // Tỷ lệ gốc 1080x1920

    void Start()
    {
        Canvas canvas = GetComponent<Canvas>();
        Camera camera = canvas.worldCamera;

        float windowAspect = (float)Screen.width / Screen.height;
        float scaleWidth = windowAspect / targetAspect;

        if (scaleWidth < 1.0f) // Màn hình quá cao (phần thừa trên và dưới)
        {
            Rect rect = camera.rect;

            rect.width = 1.0f;
            rect.height = scaleWidth;
            rect.x = 0;
            rect.y = (1.0f - scaleWidth) / 2.0f;

            camera.rect = rect;
        }
        else // Màn hình rộng hơn hoặc bằng tỷ lệ gốc
        {
            Rect rect = camera.rect;

            rect.width = 1.0f;
            rect.height = 1.0f;
            rect.x = 0;
            rect.y = 0;

            camera.rect = rect;
        }
    }
}
