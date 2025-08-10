using UnityEngine;
using DG.Tweening;
using UnityEngine.UI;

public class CardFusionUI : MonoBehaviour
{
    public RectTransform[] cards;        // 5 UI Image card
    public RectTransform fusionTarget;   // Vị trí hợp nhất
    public Image fusionResult;           // UI Image kết quả
    public ParticleSystem fusionFX;      // FX flash khi hợp nhất
    public GameObject selectHero;
    public float upDistance = 50f;
    public float upTime = 0.4f;
    public float moveToCenterTime = 0.6f;
    public float rotateSpeed = 360f;

    private Vector2[] originalPositions; // Lưu vị trí ban đầu

    void Awake()
    {
        // Lưu lại vị trí ban đầu của các card
        originalPositions = new Vector2[cards.Length];
        for (int i = 0; i < cards.Length; i++)
        {
            originalPositions[i] = cards[i].anchoredPosition;
        }
    }

    public void StartFusion()
    {
        int completed = 0;

        foreach (RectTransform card in cards)
        {
            Vector2 originalPos = card.anchoredPosition;
            Vector2 upPos = originalPos + Vector2.up * upDistance;
            //  float randomRotate = Random.Range(-rotateSpeed, rotateSpeed);
            float fusionRotate = 150;
            // Bay lên + xoay nhẹ
            card.DOAnchorPos(upPos, upTime).SetEase(Ease.OutQuad);
            card.DORotate(new Vector3(0, 0, fusionRotate), upTime, RotateMode.FastBeyond360);

            // Bay vào giữa
            Sequence seq = DOTween.Sequence();
            seq.AppendInterval(upTime)
               .Append(card.DOAnchorPos(fusionTarget.anchoredPosition, moveToCenterTime).SetEase(Ease.InQuad))
               .Join(card.DORotate(Vector3.zero, moveToCenterTime))
               .OnComplete(() =>
               {
                   completed++;
                   if (completed == cards.Length)
                   {
                       PlayFusionEffect();
                   }
               });
        }
    }

    void PlayFusionEffect()
    {
        // FX flash
        if (fusionFX != null) fusionFX.Play();

        // Ẩn các card và reset vị trí về ban đầu
        for (int i = 0; i < cards.Length; i++)
        {
           // cards[i].gameObject.SetActive(false);
            cards[i].anchoredPosition = originalPositions[i]; // Trả về vị trí ban đầu
            cards[i].rotation = Quaternion.identity;          // Reset xoay
        }

        // Hiện card kết quả
        selectHero.SetActive(false);
        fusionResult.gameObject.SetActive(true);
        fusionResult.rectTransform.localScale = Vector3.zero;
        fusionResult.rectTransform.DOScale(Vector3.one, 0.4f).SetEase(Ease.OutBack)
            .OnComplete(() =>
            {
                // Rung nhẹ
                fusionResult.rectTransform.DOShakePosition(0.2f, 5f, 10, 90, false, true);
            });
       
    }

    public void ResetCards()
    {
        // Hàm này để hiện lại các card nếu muốn chạy lại animation
        foreach (var card in cards)
            card.gameObject.SetActive(true);

        fusionResult.gameObject.SetActive(false);
    }
}
