using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class EffectDecorator : IEffect
{
    private IEffect decoratableCard { get; }

    protected EffectDecorator(IEffect decoratableCard)
    {
        this.decoratableCard = decoratableCard;
    }

    public virtual int ApplyEffect(int currentValue)
    {
        return decoratableCard.ApplyEffect(currentValue);
    }
}
