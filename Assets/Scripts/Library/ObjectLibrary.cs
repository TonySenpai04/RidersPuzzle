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
        
       txtID.gameObject.SetActive(false);
       obejctImage.gameObject.SetActive(true);

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
        
       

    }
}
