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

    [Header("异常状态颜色")]
    [SerializeField] private Color[] chillColor;
    [SerializeField] private Color[] igniteColor;
    [SerializeField] private Color[] shockColor;

    void Start()
    {
        sr = GetComponent<SpriteRenderer>();

        origineMat = sr.material;
    }
    private IEnumerator FlashFX()
    {
        sr.material = hitMat;
        Color curColor = sr.color;
        sr.color = Color.white;
        yield return new WaitForSeconds(flashDuration);
        sr.color = curColor;
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
    private void CancelColorChange()
    {
        CancelInvoke();     //取消该 MonoBehaviour 上的所有 Invoke 调用。
        sr.color = Color.white;
    }

    public void ShockFxFor(float _second)
    {
        InvokeRepeating("ShockColor", 0, .3f);
        Invoke("CancelColorChange", _second);
    }
    public void ChillFxFor(float _second)
    {
        InvokeRepeating("ChillColor", 0, .3f);
        Invoke("CancelColorChange", _second);
    }
    public void IgniteFxFor(float _second)
    {
        InvokeRepeating("IgniteColor", 0, .3f);
        Invoke("CancelColorChange", _second);
    }
    private void ShockColor()
    {
        if (sr.color != shockColor[0])
            sr.color = shockColor[0];
        else
            sr.color = shockColor[1];
    }
    private void ChillColor()
    {
        if(sr.color != chillColor[0])
            sr.color = chillColor[0];
        else
            sr.color = chillColor[1];
    }
    private void IgniteColor()
    {
        if(sr.color != igniteColor[0])
            sr.color = igniteColor[0];
        else
            sr.color = igniteColor[1];
    }
}
