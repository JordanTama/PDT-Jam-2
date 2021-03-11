using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Patron : Dealer
{
    protected override void Start()
    {
        dealerType = DealerType.Patron;

        base.Start();
    }

    public override void StartTurn()
    {
        Debug.Log(dealerType + " takes their turn.");

        DrawCards();

        OnStartTurn?.Invoke();

        // Play a random card
        Card randomCard = hand.cards[Random.Range(1, hand.cards.Count - 1)];

        hand.PlayCard(randomCard);
    }
}
