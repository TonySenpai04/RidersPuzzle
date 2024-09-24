using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public static PlayerController instance;
    public MovementController movementController;
    public IHitPoint hitPoint;
    private void Start()
    {
        instance = this;
        hitPoint = new HitPoint(5);
    }
   
}
