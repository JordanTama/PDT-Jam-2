using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HalveDecorator : EffectDecorator
{
    public HalveDecorator(IEffect decoratableCard) : base(decoratableCard) { }

    public override int ApplyEffect(int currentValue)
    {
        Debug.Log("Halve");
        return base.ApplyEffect(currentValue) / 2;
    }
}
