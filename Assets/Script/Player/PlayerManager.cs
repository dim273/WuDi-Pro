using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerManager : MonoBehaviour
{
    public static PlayerManager instance;  
    public Player player;

    public int soul;

    private void Awake()
    {
        if(instance != null)
        {
            Destroy(instance.gameObject);
        }
        else 
            instance = this;
    }

    public bool HaveEnoughSoul(int _cost)
    {
        if(_cost > soul)
        {
            Debug.Log("Ã»ÓÐ×ã¹»Áé»ê");
            return false;
        }
        else 
            soul -= _cost;
        return true;
    }
}
