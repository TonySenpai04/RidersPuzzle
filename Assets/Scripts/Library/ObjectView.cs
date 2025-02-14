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

    void Start()
    {

    }
    public void SetObject(int id, Sprite objectImage,string name)
    {
        this.id = id;
        this.objectImage.sprite= objectImage;
        txtName.text = id + " " +name.ToUpper() ;

    }
 
}
