using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UpgadeUIController : MonoBehaviour
{
    [SerializeField] private GameObject btnGroup;
    [SerializeField] private GameObject UpgradeView;
    [SerializeField] private GameObject enhaceView;
    void Start()
    {
        
    }
    private void OnEnable()
    {
        btnGroup.gameObject.SetActive(true);
        UpgradeView.gameObject.SetActive(false);
        enhaceView.gameObject.SetActive(false);
    }

  
    void Update()
    {
        
    }
}
