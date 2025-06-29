using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface INewBoughtHero 
{
    public void AddNewHero(int id)
    {
       
    }

    public void RemoveHero(int id)
    {
        
    }

    public bool IsNewHero(int id)
    {
        return false;
    }

    public bool AllSeen()
    {
      return false;
    }

    private void SaveData()
    {
        
    }

    private void LoadData()
    {
        
    }
}
