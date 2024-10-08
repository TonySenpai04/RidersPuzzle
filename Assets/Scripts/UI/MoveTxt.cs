using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class MoveTxt : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI moveTxt;
    private void FixedUpdate()
    {
        moveTxt.text = PlayerController.instance.movementController.numberOfMoves.GetCurrentMove().ToString();
    }
}

