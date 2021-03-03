using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CardController : MonoBehaviour
{
    [SerializeField] private CardData data;

    private IEffect effect = new BaseEffect();

    private void Start()
    {
        foreach (EffectTypes effectData in data.effects) {
            switch (effectData)
            {
                case EffectTypes.AddOne:
                    Debug.Log("Create add one decorator");
                    effect = new AddOneDecorator(effect);
                    break;
                case EffectTypes.Halve:
                    Debug.Log("Create halve decorator");
                    effect = new HalveDecorator(effect);
                    break;
                //case EffectTypes.Add:
                //    Debug.Log("Create add decorator");
                //    effect = new AddDecorator(effect, valueToAdd);
                //    break;
                default:
                    throw new System.Exception("No such effect: " + effectData);
            }
            
        }

        Debug.Log(effect.ApplyEffect(10));
    }

    public enum EffectTypes
    {
        AddOne,
        Halve,
        //Add
    }
}
