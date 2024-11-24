using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public static PlayerController instance;
    public MovementController movementController;
    public IHitPoint hitPoint;

    private void Awake()
    {

        instance = this;
        hitPoint = new HitPoint(10);
        movementController.immortal = (IImmortal)hitPoint;
    }
    private void Start()
    {

        
    }
    public void LoadLevel()
    {
        hitPoint.SetHealth(10);
        movementController.LoadMove();
        SkillManager.instance.LoadSkill();
    }
    private void Update()
    {
        if (!GameManager.instance.isEnd) {
            if (hitPoint.GetCurrentHealth() <= 0)
            {
                return;
            }
            else
            {
                movementController.Movement();
            }
        }
        if (Input.GetKey(KeyCode.T))
        {
            hitPoint.TakeDamage(1);
        }
    }
}
