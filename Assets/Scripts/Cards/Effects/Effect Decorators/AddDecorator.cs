using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class AddDecorator : EffectDecorator
{
    private int valueToAdd;

    public AddDecorator(IEffect decoratableCard, int valueToAdd) : base(decoratableCard) {
        this.valueToAdd = valueToAdd;
    }

    public override int ApplyEffect(int currentValue)
    {
        Debug.Log("Add " + valueToAdd);
        return base.ApplyEffect(currentValue) + valueToAdd;
    }
}
