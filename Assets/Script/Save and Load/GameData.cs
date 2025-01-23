using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[System.Serializable]

public class GameData
{
    public int soul;
    public SerializableDictionary<string, int> inventory;
    public GameData()
    {
        this.soul = 0;
        inventory = new SerializableDictionary<string, int>();
    }
}
