using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ToturialController : MonoBehaviour
{
    [SerializeField] private List<GameObject> images;
    [SerializeField] private List<Image> circles; 
    [SerializeField] private Sprite circleEmpty;  
    [SerializeField] private Sprite circleFilled; 
    [SerializeField] private int currentIndex = 0;
    [SerializeField] private Button next;
    [SerializeField] private Button prev;

    void Start()
    {
        UpdateUI();
        prev.onClick.AddListener(() => PreviousImage());
        next.onClick.AddListener(() => NextImage());
    }

    public void NextImage()
    {
        if (currentIndex < images.Count - 1) 
        {
            currentIndex++;
            UpdateUI();
        }
    }

    public void PreviousImage()
    {
        if (currentIndex > 0) 
        {
            currentIndex--;
            UpdateUI();
        }
    }

    void UpdateUI()
    {
        for (int i = 0; i < images.Count; i++)
        {
            images[i].SetActive(i == currentIndex);
        }

        for (int i = 0; i < circles.Count; i++)
        {
            circles[i].sprite = (i == currentIndex) ? circleFilled : circleEmpty;
        }
        prev.gameObject.SetActive(currentIndex > 0);
        next.gameObject.SetActive(currentIndex < images.Count - 1);
    }
}
