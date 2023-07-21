using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[Serializable]
public class Stat
{
    [SerializeField]
    private BarScript bar;
    [SerializeField]
    private float maxValue;
    [SerializeField]
    private float currentValue;

    public float CurrentValue { get => currentValue; set { this.currentValue = Mathf.Clamp(value,0,MaxValue); bar.Value = currentValue; } }

    public float MaxValue { get => maxValue; set { maxValue = value; bar.MaxValue = maxValue; } }

    public void Initialize()
    {
        this.MaxValue = maxValue;
        this.CurrentValue = currentValue;
    }
}
