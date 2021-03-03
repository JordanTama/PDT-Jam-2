using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[CreateAssetMenu]
public class CardData : ScriptableObject
{
    public List<CardController.EffectTypes> effects;
}
