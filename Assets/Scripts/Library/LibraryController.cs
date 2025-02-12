using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class LibraryController : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI heroTxt;
    [SerializeField] private TextMeshProUGUI objectTxt;
    [SerializeField] private GameObject menu;
    [SerializeField] private HeroLibraryController heroLibraryController;
    private void Start()
    {
        heroTxt.text = HeroManager.instance.HeroOwnedQuantity()+"/11";
        objectTxt.text = HiddenObjectManager.instance.ObjectQuantity() + "/" + HiddenObjectManager.instance.ObjectQuantity();
    }
    private void OnEnable()
    {
        menu.SetActive(true);
        heroTxt.text = HeroManager.instance.HeroOwnedQuantity() + "/11" ;
    }
}
