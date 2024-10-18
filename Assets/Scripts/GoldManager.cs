using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GoldManager : MonoBehaviour
{
    public int currentGold;
    public static GoldManager instance;
    private void Start()
    {
        instance = this;
    }

    public void AddGold(int amount)
    {
        if (amount > 0)
        {
            currentGold += amount;
        }
        else
        {
            Console.WriteLine("Số vàng thêm phải lớn hơn 0.");
        }
    }

    public void SpendGold(int amount)
    {
        if (amount > 0 && currentGold >= amount)
        {
            currentGold -= amount;
 
           
        }
        if(currentGold < 0)
        {
            currentGold = 0;
        }
 
    }

    public int GetGold()
    {
        return currentGold;
    }

    public void ResetGold(int newGoldAmount)
    {
        currentGold = newGoldAmount;
    }
}
