using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PandoraBox : HiddenObject
{
    [Serializable]
    public class MovementEffect
    {
        public float probability; 
        public Action effect;

        public MovementEffect(float probability, Action effect)
        {
            this.probability = probability;
            this.effect = effect;
        }

        public void ExecuteEffect()
        {
            effect();
  
        }
    }

    private List<MovementEffect> effects;

    public  void Awake()
    {
        effects = new List<MovementEffect>
        {
            new MovementEffect(0.05f, ReturnToStart),
            new MovementEffect(0.05f, MoveToDestination),
            new MovementEffect(0.45f, RandomPowerUp),
            new MovementEffect(0.45f, RandomObstacle)
        };
    }
    public override void ActiveSkill()
    {
        
        ApplyEffects();
    }
    public void ApplyEffects()
    {
        float random = UnityEngine.Random.value; 
        float cumulativeProbability = 0f;
        List<MovementEffect> validEffects = new List<MovementEffect>(); 

        foreach (var effect in effects)
        {
            cumulativeProbability += effect.probability;
            if (random <= cumulativeProbability) 
            {
                validEffects.Add(effect); 
            }
        }

        if (validEffects.Count > 0)
        {
            int randomIndex = UnityEngine.Random.Range(0, validEffects.Count);
            validEffects[randomIndex].ExecuteEffect(); 
        }
    }

    private void ReturnToStart()
    {
        PlayerController.instance.movementController.MoveStartPoint();
        Destroy(this.gameObject);
    }

    private void MoveToDestination()
    {
        PlayerController.instance.movementController.MoveEndPoint();
        Destroy(this.gameObject);
    }

    private void RandomPowerUp()
    {

        HiddenObject objectRandom = Instantiate(HiddenObjectManager.instance.GetRandomPowerUp(),
            transform.position, Quaternion.identity);
        Debug.Log(objectRandom.name);
        objectRandom.ActiveSkill();
        Destroy(this.gameObject);

    }

    private void RandomObstacle()
    {
        HiddenObject objectRandom = Instantiate(HiddenObjectManager.instance.GetRandomObstacle(),
            transform.position, Quaternion.identity);
        Debug.Log(objectRandom.name);
        objectRandom.ActiveSkill();
        Destroy(this.gameObject);

    }
}
