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

    protected DealerType dealerType;
    protected GameService gameService;
    protected List<Card> deck = new List<Card>();
    public Hand hand;

    private void Awake()
    {
        startDeck.cards.ForEach(card => deck.Add(Instantiate(card)));

        hand = new Hand(this, deck);
    }

    protected virtual void Start()
    {
        gameService = ServiceLocator.ServiceLocator.Get<GameService>();

        gameService.RegisterDealer(dealerType, this);
    }

    protected void DrawCards()
    {
        while (hand.cards.Count < dealerSettings.cardsInHand && deck.Count > 0)
        {
            hand.DrawCard();
        }
    }

    public void PlayCard(Card card)
    {
        tableController.PlayCard(this, card);

        Debug.Log(dealerType + " plays " + card.name);

        EndTurn();
    }

    public virtual void StartTurn()
    {
        Debug.Log(dealerType + " takes their turn.");

        DrawCards();

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
