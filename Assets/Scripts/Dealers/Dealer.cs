using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Dealer : MonoBehaviour
{
    [SerializeField] protected DealerSettings dealerSettings;
    [SerializeField] protected Deck startDeck;
    [SerializeField] protected TableController tableController;
    
    public int targetValue;

    public enum DealerType
    {
        Player,
        Patron
    }
    
    public GameService.Role role { get; set; }
    public Action OnStartTurn;
    public Action OnEndTurn;
    public Action<Card> OnDrawCard;

    protected DealerType dealerType;
    protected GameService gameService;
    protected List<Card> deck = new List<Card>();
    protected List<Card> hand = new List<Card>();

    protected virtual void Start()
    {
        gameService = ServiceLocator.ServiceLocator.Get<GameService>();

        gameService.RegisterDealer(dealerType, this);

        startDeck.cards.ForEach(card => deck.Add(Instantiate(card)));

        while (hand.Count < dealerSettings.cardsInHand)
        {
            DrawCard();
        }
    }

    protected void DrawCard()
    {
        if (deck.Count > 0)
        {
            Card drawnCard = deck[0];
            hand.Add(drawnCard);
            deck.Remove(drawnCard);

            Debug.Log(dealerType + " draws " + drawnCard.name);

            OnDrawCard?.Invoke(drawnCard);
        } else
        {
            Debug.Log(dealerType + " has no cards left to draw");
        }
    }

    public void PlayCard(Card card)
    {
        hand.Remove(card);

        tableController.PlayCard(this, card);

        Debug.Log(dealerType + " plays " + card.name);

        DrawCard();

        EndTurn();
    }

    public virtual void StartTurn()
    {
        Debug.Log(dealerType + " takes their turn.");

        OnStartTurn?.Invoke();

        EndTurn();
    }

    protected void EndTurn()
    {
        Debug.Log(dealerType + " ends their turn.");

        OnEndTurn?.Invoke();

        gameService.EndTurn();
    }
}
