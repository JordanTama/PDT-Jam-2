using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class Card : ScriptableObject
{
    public new string name;
    public Color color;
    public int value;
    public bool playable;
    public Action<Card> OnPlayed;

    public bool hasComboEffect;
    public int cardValueToActivateCombo;
    public int valueToAddOnComboActivation;

    public bool hasExpiry;
    public int turnsUntilCardExpires;
    public int valueDecay;
    public Action<Card> OnExpiry;
    
    public Content[] content;

    public bool hasMarkupEffect;
    public float markupMultiplier;

    public void PlayCard()
    {
        if (playable)
        {
            OnPlayed?.Invoke(this);
        }
    }

    public void StartTurn()
    {
        if (hasExpiry)
        {
            if (turnsUntilCardExpires == 0)
            {
                OnExpiry?.Invoke(this);
            }
            else
            {
                value -= valueDecay;
                turnsUntilCardExpires--;
            }
        }
    }

    [Serializable]
    public struct Content
    {
        public Material material;
        public float depth;
    }
}
