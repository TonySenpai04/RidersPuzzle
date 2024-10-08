using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class HealthTxt : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI healthTxt;
    private void FixedUpdate()
    {
        healthTxt.text=PlayerController.instance.hitPoint.GetCurrentHealth().ToString();
    }
}
