using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseEffect : IEffect
{
    public int ApplyEffect(int currentValue)
    {
        Debug.Log("base value of " + currentValue);
        return currentValue;
    }
}
