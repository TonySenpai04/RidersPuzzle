using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UpgadeUIController : MonoBehaviour
{
    [SerializeField] private GameObject btnGroup;
    [SerializeField] private GameObject UpgradeView;
    [SerializeField] private GameObject enhaceView;
    [SerializeField] private GameObject inventoryView;
    void Start()
    {
        
    }
    private void OnEnable()
    {
        btnGroup.SetActive(true);
        UpgradeView.SetActive(false);
        enhaceView.SetActive(false);
        inventoryView.SetActive(false);
    }

  
    void Update()
    {
        
    }
}
