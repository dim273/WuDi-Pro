using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BackgroundFollow : MonoBehaviour
{
    [SerializeField] private float parallaxEffect;
    private GameObject cam;
    private float xPostion;
    private float length;

    void Start()
    {
        cam = GameObject.Find("Main Camera");
        xPostion = transform.position.x;
    }

    
    void Update()
    {
        float diatanceToMove = cam.transform.position.x * parallaxEffect;
        float distanceMoved = cam.transform.position.x * (1 - parallaxEffect);

        transform.position = new Vector3(xPostion + diatanceToMove, transform.position.y);

        if (distanceMoved > length + xPostion)
            xPostion = xPostion + length;
        else if(distanceMoved < length - xPostion)
            xPostion = xPostion - length;

    }
}
