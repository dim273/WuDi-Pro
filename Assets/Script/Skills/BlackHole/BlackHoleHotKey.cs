using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class BlackHoleHotKey : MonoBehaviour
{
    private KeyCode myHotKey;
    private TextMeshProUGUI myText;

    private Transform myTransform;
    private BlackHoleManager myManager;

    public void SetupHotKey(KeyCode _myKeyCode, Transform _myTransform, BlackHoleManager _myManager)
    {
        myText = GetComponentInChildren<TextMeshProUGUI>();
        myTransform = _myTransform;
        myHotKey = _myKeyCode; 
        myManager = _myManager;
        myText.text = _myKeyCode.ToString();  
    }
    private void Update()
    {
        if (Input.GetKeyDown(myHotKey))
        {
            myManager.AddEnemyToList(myTransform);
            myText.color = Color.clear;
        }
    }
}
