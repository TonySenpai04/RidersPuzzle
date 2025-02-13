using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class LibraryController : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI heroTxt;
    [SerializeField] private TextMeshProUGUI objectTxt;
    [SerializeField] private GameObject menu;
    [SerializeField] private GameObject riderLibrary;
    [SerializeField] private GameObject objectLibrary;
    [SerializeField] private GameObject riderView;
    [SerializeField] private GameObject toturialView;
    [SerializeField] private GameObject objectview;
    [SerializeField] private HeroLibraryController heroLibraryController;
    private void Start()
    {
        heroTxt.text = HeroManager.instance.HeroOwnedQuantity()+"/11";
        objectTxt.text = HiddenObjectManager.instance.ObjectQuantity() + "/" + HiddenObjectManager.instance.ObjectQuantity();
    }
    private void OnEnable()
    {
        menu.SetActive(true);
        riderLibrary.SetActive(false);
        objectLibrary.SetActive(false);
        objectview.SetActive(false);
        toturialView.SetActive(false);
        riderView.SetActive(false);
        heroTxt.text = HeroManager.instance.HeroOwnedQuantity() + "/11" ;
    }
}
