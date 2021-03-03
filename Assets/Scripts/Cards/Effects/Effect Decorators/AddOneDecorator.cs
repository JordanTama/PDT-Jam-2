using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AddOneDecorator : EffectDecorator
{
    public AddOneDecorator(IEffect decoratableCard) : base(decoratableCard) { }

    public override int ApplyEffect(int currentValue)
    {
        Debug.Log("Add one");
        return base.ApplyEffect(currentValue) + 1;
    }
}
