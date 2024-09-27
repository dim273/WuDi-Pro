using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerManager : MonoBehaviour
{
    public static PlayerManager instance;  
    public GameObject player;
    private void Awake()
    {
        if(instance != null)
        {
            Destroy(instance.gameObject);
        }
        else 
            instance = this;
    }
}
