using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ObjectView : MonoBehaviour
{
    [SerializeField] private int id;
    [SerializeField] private Image objectImage;
    [SerializeField] private TextMeshProUGUI txtName;
    private ISwipeDetector swipeDetector;
    [SerializeField] private List<HiddenObject> seenObjects;
    [SerializeField] private int currentIndex;


    void Start()
    {
        swipeDetector = new SwipeDetector(NextObject, PreviousObject);
        seenObjects = HiddenObjectManager.instance.GetSeenObject();
    }
    private void Update()
    {
        swipeDetector.DetectSwipe();
    }
    private void OnEnable()
    {
        seenObjects.Clear();
        seenObjects = HiddenObjectManager.instance.GetSeenObject();
    }
    public void SetObject(int id, Sprite objectImage,string name)
    {
        this.id = id;
        this.objectImage.sprite= objectImage;
        txtName.text = id + " " +name.ToUpper() ;
        currentIndex = seenObjects.FindIndex(o => o.id == id);

    }
    private void LoadObject(int index)
    {
        if (index >= 0 && index < seenObjects.Count)
        {
            HiddenObject obj = seenObjects[index];
            SetObject(obj.id, obj.sprite, obj.name);
            BackgroundManager.instance.SetObjectBg(id);
            ApplyText.instance.UpdateObjectInfo(id);
        }
    }
    public void NextObject()
    {
        if (currentIndex < seenObjects.Count - 1)
        {
            currentIndex++;
            LoadObject(currentIndex);
        }
    }
    public void PreviousObject()
    {
        if (currentIndex > 0)
        {
            currentIndex--;
            LoadObject(currentIndex);
        }
    }

}
