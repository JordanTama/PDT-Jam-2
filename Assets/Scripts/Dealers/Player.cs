using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : Dealer
{
    [SerializeField] private HandObject handObject;
    protected override void Start()
    {
        dealerType = DealerType.Player;

        base.Start();
        
        hand.OnDrawCard += card => handObject.Add(card);
    }

    protected virtual void DrawCards()
    {
        while (hand.cards.Count < dealerSettings.cardsInHand && deck.Count > 0)
            hand.DrawCard();
    }
    
    public override void StartTurn()
    {
        Debug.Log(dealerType + " takes their turn.");

        DrawCards();

        OnStartTurn?.Invoke();
    }
}
