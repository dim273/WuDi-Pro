using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EntityFX : MonoBehaviour
{
    private SpriteRenderer sr;

    [Header("Flash FX")]
    [SerializeField] private float flashDuration;
    [SerializeField] private Material hitMat;

    private Material origineMat;

    void Start()
    {
        sr = GetComponent<SpriteRenderer>();
        origineMat = sr.material;
    }
    private IEnumerator FlashFX()
    {
        sr.material = hitMat;
        yield return new WaitForSeconds(flashDuration);
        sr.material = origineMat;
    }
    private void RedColorBlink()
    {
        if(sr.color != Color.white)
        {
        sr.color = Color.white; 
        }
        else
        {
            sr.color = Color.red;
        }
    }
    private void CancelRedBlink()
    {
        CancelInvoke();
        sr.color = Color.white;
    }
}
