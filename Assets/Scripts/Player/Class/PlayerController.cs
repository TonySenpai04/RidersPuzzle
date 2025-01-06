using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using Unity.VisualScripting;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public static PlayerController instance;
    public MovementController movementController;
    public IHitPoint hitPoint;
    public DataHero currentHero;
    private void Awake()
    {
        instance = this;
        hitPoint = new HitPoint(10);
        movementController.immortal = (IImmortal)hitPoint;
        float screenWidth = Camera.main.orthographicSize * 2 * 9f/16f; 
         float cellSize =(float) (screenWidth - 0.1 * (6 - 1)) / 6;
        transform.localScale= new Vector3(cellSize, cellSize, 1);
   
    }
    private void Start()
    {
        

    }
    public void SetCurrentData(DataHero hero)
    {
        currentHero = hero;
    }
    public void LoadLevel()
    {
        hitPoint.SetHealth(currentHero.hp);
        hitPoint.Heal(currentHero.hp);
        GetComponent<SpriteRenderer>().sprite = currentHero.heroImage;
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
 
    }
   
}
