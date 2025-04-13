using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;
public class SliderController : MonoBehaviour
{

    public bool isRunning = true;
    public Slider slider;
    public float speed = 0.1f;
    float time = 0f;
    public GameObject btnStart;
    public GameObject panelTransition;
  


    void Start()
    {
      //  slider.gameObject.SetActive(true);
        btnStart.SetActive(false);
       
        isRunning = true;
    }
   
    void Update()
    {
        if (isRunning && slider.gameObject.activeSelf)
        {
            UpdateSliderValue();
            CheckSliderCompletion();
        }
    }
    public void ShowSlider()
    {
        slider.gameObject.SetActive(true);
    }

    // Hàm này sẽ được gọi từ VersionChecker để ẩn thanh slider
    public void HideSlider()
    {
        slider.gameObject.SetActive(false);
    }
    void UpdateSliderValue()
    {
        // Nếu chưa lấy xong thời gian => cho chạy tối đa tới 99%
        float maxValue = TimeManager.Instance != null && !TimeManager.Instance.IsTimeFetched ? 0.75f : 1f;

        time += Time.deltaTime * speed;
        time = Mathf.Min(time, maxValue); // không vượt quá maxValue
        slider.value = time;
    }




    void CheckSliderCompletion()
    {
        if (time >= 1f && TimeManager.Instance != null && TimeManager.Instance.IsTimeFetched)
        {
            CompleteSlider();
        }
    }

    void CompleteSlider()
    {
        btnStart.SetActive(true);
        slider.gameObject.SetActive(false);
    }

}