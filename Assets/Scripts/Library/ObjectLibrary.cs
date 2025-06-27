using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ObjectLibrary : MonoBehaviour
{

    [SerializeField] private int id;
    [SerializeField] private Image obejctImage;
    [SerializeField] private TextMeshProUGUI txtID;
    [SerializeField] private TextMeshProUGUI txtID2;
    [SerializeField] private GameObject notiRedDot;
    //[SerializeField] private HeroView heroView;

    public int Id { get => id; set => id = value; }

    private void Start()
    {

        UpdateVisibility();

    }

    public void SetObject(int id, Sprite objectSprite)
    {
        this.Id = id;
        this.obejctImage.sprite = objectSprite;
        this.txtID.text = id.ToString();
        this.txtID2.text = id.ToString();
        txtID.gameObject.SetActive(false);
    }
    private void OnEnable()
    {

        UpdateVisibility();

    }
    public void UpdateVisibility()
    {
        var hiddenObject = HiddenObjectManager.instance.GetById(this.Id);
        if (hiddenObject != null && hiddenObject.isSeen)
        {

            txtID.gameObject.SetActive(false);
            obejctImage.gameObject.SetActive(true);
            obejctImage.sprite = hiddenObject.sprite;
            notiRedDot.SetActive(hiddenObject.isFirstSeen);
        }
        else
        {
            txtID.gameObject.SetActive(true);
            obejctImage.gameObject.SetActive(false);
            notiRedDot.SetActive(false);
        }
    }
    public void SetSeenObject(bool isSeen)
    {
        var hiddenObject = HiddenObjectManager.instance.GetById(this.Id);
        if (hiddenObject != null )
        {
            hiddenObject.isSeen = isSeen;
        }
    }
    public void OnClick()
    {
        var hiddenObject = HiddenObjectManager.instance.GetById(this.Id);
        if (hiddenObject != null && hiddenObject.isFirstSeen)
        {
            hiddenObject.isFirstSeen = false;
            notiRedDot.SetActive(false);
        }
        bool anyFirstSeen = false;
        foreach (var hdObject in HiddenObjectManager.instance.AllObjects)
        {
            if (hdObject != null && hdObject.isFirstSeen)
            {
                anyFirstSeen = true;
                break;
            }
        }

        if (!anyFirstSeen)
        {
            NotiManager.instance.ClearNotiRedDot("object");
        }
    }

}
