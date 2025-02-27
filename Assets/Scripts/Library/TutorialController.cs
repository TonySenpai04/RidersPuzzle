using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TutorialController : MonoBehaviour
{
    [SerializeField] private List<GameObject> images;
    [SerializeField] private List<Image> circles; 
    [SerializeField] private Sprite circleEmpty;  
    [SerializeField] private Sprite circleFilled; 
    [SerializeField] private int currentIndex = 0;
    [SerializeField] private Button next;
    [SerializeField] private Button prev;
    [SerializeField] public Button close;
    [SerializeField] private GameObject menuLib;
    
    private Vector2 startTouchPosition;
    private Vector2 endTouchPosition;
    private bool isSwiping = false;
    private float swipeThreshold = 50f;
    [SerializeField] private ISwipeDetector swipeDetector;
    void Start()
    {
        swipeDetector= new SwipeDetector(NextImage, PreviousImage);
        UpdateUI();

        prev.onClick.AddListener(() => PreviousImage());
        next.onClick.AddListener(() => NextImage());
        close.onClick.AddListener(() => Close());
    }
    void Update()
    {
        swipeDetector.DetectSwipe();
    }


    public void Close()
    {
        
         this.gameObject.SetActive(false);
         menuLib.SetActive(true);
        
    }
    public void CloseFirst()
    {

        this.gameObject.SetActive(false);


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
    private void OnEnable()
    {
        currentIndex = 0;
        UpdateUI();
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
