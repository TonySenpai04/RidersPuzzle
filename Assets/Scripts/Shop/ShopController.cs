using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ShopController : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI goldTxt;
    private void Start()
    {
        goldTxt.text=GoldManager.instance.GetGold().ToString();
    }
}
