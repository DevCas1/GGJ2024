using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UiHover : MonoBehaviour
{
    private RectTransform rTransform;
    private float baseYoffset;
    public float hoverAmplitude = 1.0f;
    public float hoverSpeed = 1.0f;

    void Awake()
    {
        rTransform = GetComponent<RectTransform>();
        baseYoffset = rTransform.localPosition.y;
    }

    void Update()
    {
        Vector3 pos = rTransform.localPosition;
        pos.y = hoverAmplitude * Mathf.Cos(Time.time * hoverSpeed) + baseYoffset;
        rTransform.localPosition = pos;
    }
}
