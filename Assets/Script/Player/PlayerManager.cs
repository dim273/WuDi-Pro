using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerManager : MonoBehaviour, ISaveManager
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
            Debug.Log("û���㹻���");
            return false;
        }
        else 
            soul -= _cost;
        return true;
    }

    public void LoadData(GameData _data)
    {
        soul = _data.soul;
    }

    public void SaveData(ref GameData _data)
    {
        _data.soul = this.soul;
    }
}
