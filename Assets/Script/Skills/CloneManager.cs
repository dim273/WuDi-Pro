using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CloneManager : MonoBehaviour
{
    private SpriteRenderer sr;
    [SerializeField] private float colorLosingSpeed;
    private float cloneTimer;
    private void Awake()
    {
        sr = GetComponent<SpriteRenderer>();
    }
    private void Start()
    {
        
    }

    private void Update()
    {
        cloneTimer -= Time.deltaTime;
        if(cloneTimer < 0)
        {
            sr.color = new Color(1,1,1,sr.color.a - (Time.deltaTime * colorLosingSpeed));
        }
    }
    public void SetupClone(Transform _cloneTransform, float _cloneDuration)
    {
        cloneTimer = _cloneDuration; 
        transform.position = _cloneTransform.position;
    }
}
