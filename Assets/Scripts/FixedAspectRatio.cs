using UnityEngine;

[RequireComponent(typeof(Camera))]
public class FixedHeightDisplay : MonoBehaviour
{
    public float targetAspect = 9f / 16f; // Tỷ lệ 9:16 (1080x1920)

    void Start()
    {
        Camera camera = GetComponent<Camera>();

        // Tính toán tỷ lệ hiện tại
        float windowAspect = (float)Screen.width / Screen.height;

        // Tính toán tỷ lệ scale
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
        else // Màn hình vừa hoặc thấp hơn tỷ lệ (game giữ nguyên chiều dài)
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
