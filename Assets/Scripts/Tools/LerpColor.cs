using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LerpColor : MonoBehaviour
{
    public float LerpLength = 1f;

    float LerpTime = 0f;

    private Color ColorStart;
    public Color ColorTarget;
    public bool IsLerping = true;
    private bool LerpReverse = false;

    private Color NextColor = Color.white;

    public Image TargetObject;

    private void Start()
    {
        if(TargetObject == null)
            return;

        ColorStart = TargetObject.color;
    }

    void Update()
    {
        if(!IsLerping)
            return;

        LerpTime += Time.deltaTime;

        if(!LerpReverse)
            TargetObject.color = Color.Lerp(ColorStart, ColorTarget, LerpTime / LerpLength);
        else
            TargetObject.color = Color.Lerp(ColorTarget, ColorStart, LerpTime / LerpLength);

        if(LerpTime / LerpLength >= 1)
        {
            LerpTime = 0f;
            LerpReverse = !LerpReverse;
        }
    }
}
