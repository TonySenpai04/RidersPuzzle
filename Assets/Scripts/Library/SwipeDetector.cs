using System;
using UnityEngine;

public class SwipeDetector : ISwipeDetector
{
    public Action OnSwipeLeft;
    public Action OnSwipeRight;
    private Vector2 startTouchPosition;
    private Vector2 endTouchPosition;
    private bool isSwiping = false;
    private float swipeThreshold = 50f;

    public SwipeDetector(  Action OnSwipeRight, Action  OnSwipeLeft)
    {
        this.OnSwipeLeft = OnSwipeLeft;  
        this.OnSwipeRight = OnSwipeRight;
    }


    public void DetectSwipe()
    {
        if (Input.touchCount > 0)
        {
            Touch touch = Input.GetTouch(0);
            if (touch.phase == TouchPhase.Began)
            {
                startTouchPosition = touch.position;
            }
            else if (touch.phase == TouchPhase.Ended)
            {
                endTouchPosition = touch.position;
                HandleSwipe();
            }
        }
    }

    private void HandleSwipe()
    {

        float deltaX = endTouchPosition.x - startTouchPosition.x;
        if (Mathf.Abs(deltaX) > swipeThreshold)
        {
            if (deltaX < 0)
                OnSwipeRight?.Invoke(); 
            else
                OnSwipeLeft?.Invoke();
        }
    }
}
