using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TableController : MonoBehaviour
{
    public Action<int> OnChangeValue;

    private int Value
    {
        get
        {
            return value;
        }
        set
        {
            this.value = value;

            OnChangeValue?.Invoke(value);
        }
    }

    private int value;

    private List<Card> pile = new List<Card>();

    public void PlayCard(Dealer dealer, Card card)
    {
        pile.Add(card);

        Value = CalculateValue();
    }

    public int CalculateValue()
    {
        int sum = 0;

        pile.ForEach(card => sum += card.value);

        return sum;
    }
}
