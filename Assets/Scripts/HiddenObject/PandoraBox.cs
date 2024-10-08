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
            new MovementEffect(0.2f, ReturnToStart),
            new MovementEffect(0.0001f, MoveToDestination),
            new MovementEffect(0.05f, MoveForward),
            new MovementEffect(0.05f, MoveBackward)
        };
    }
    public override void ActiveSkill()
    {
        
        ApplyEffects();
    }
    public void ApplyEffects()
    {
        effects.Sort((a, b) => b.probability.CompareTo(a.probability));

        float random = UnityEngine.Random.value;

        foreach (var effect in effects)
        {
            if (random <= effect.probability) 
            {
                effect.ExecuteEffect(); 
                break; 
            }
        }
    }

    private void ReturnToStart()
    {
        PlayerController.instance.movementController.MoveStartPoint();
        Debug.Log("Trở lại điểm xuất phát");
    }

    private void MoveToDestination()
    {
        PlayerController.instance.movementController.MoveEndPoint();
        Debug.Log("Đi tới điểm đích");
    }

    private void MoveForward()
    {
        PlayerController.instance.movementController.MoveForward(1);
        Debug.Log("Tiến về phía trước 1 ô");
    }

    private void MoveBackward()
    {
        PlayerController.instance.movementController.UndoLastMove(1);
        Debug.Log("Lùi lại phía sau 1 ô");
    }
}
