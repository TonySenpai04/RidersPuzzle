using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class MazeEffect : MonoBehaviour
{
    public  string Name;
    public  string Description;
    public virtual void ApplyEffect() {
        Debug.Log("Apply effect");

    }
}
