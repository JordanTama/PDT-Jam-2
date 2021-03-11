using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hand
{
    public Action<Card> OnRemoveCard;
    public Action<Card> OnDrawCard;

    public List<Card> cards = new List<Card>();
    private List<Card> expiredCards = new List<Card>();

    private Dealer dealer;
    private List<Card> deck;
    private bool cardsPlayable;

    public Hand(Dealer dealer, List<Card> deck)
    {
        this.dealer = dealer;
        this.deck = deck;

        dealer.OnStartTurn += StartTurn;
        dealer.OnEndTurn += EndTurn;
    }

    private void StartTurn()
    {
        SetCardsPlayable(true);

        CardsStartTurn();
    }

    private void EndTurn()
    {
        SetCardsPlayable(false);

        RemoveExpiredCards();
    }

    public void DrawCard()
    {
        if (deck.Count > 0)
        {
            Card drawnCard = deck[0];

            deck.Remove(drawnCard);
            cards.Add(drawnCard);

            drawnCard.OnPlayed += PlayCard;
            drawnCard.playable = cardsPlayable;
            drawnCard.OnExpiry += expiredCards.Add;

            OnDrawCard?.Invoke(drawnCard);
        }
    }

    private void SetCardsPlayable(bool playable)
    {
        cardsPlayable = playable;

        foreach (Card card in cards)
        {
            card.playable = playable;
        }
    }

    private void PlayCard(Card card)
    {
        RemoveCard(card);

        dealer.PlayCard(card);   
    }

    private void RemoveCard(Card card)
    {
        cards.Remove(card);

        OnRemoveCard?.Invoke(card);
    }

    private void RemoveExpiredCards()
    {
        foreach (Card card in expiredCards)
        {
            if (cards.Contains(card))
            {
                RemoveCard(card);
            }
        }

        expiredCards.Clear();
    }

    private void CardsStartTurn()
    {
        foreach (Card card in cards)
        {
            card.StartTurn();
        }
    }
}
