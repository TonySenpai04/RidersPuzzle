using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ObjectLibrary : MonoBehaviour
{

    [SerializeField] private string id;
    [SerializeField] private Image obejctImage;
    [SerializeField] private TextMeshProUGUI txtID;
    [SerializeField] private TextMeshProUGUI txtID2;
 
    //[SerializeField] private HeroView heroView;

    public string Id { get => id; set => id = value; }

    private void Start()
    {

        UpdateVisibility();

    }

    public void SetObject(string id, Sprite objectSprite)
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
        }
        else
        {
            txtID.gameObject.SetActive(true);
            obejctImage.gameObject.SetActive(false);
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
}
