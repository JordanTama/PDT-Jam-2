using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Patron : Dealer
{
    [SerializeField] private Animator animator;
    [SerializeField] private StackObject stackObject;
    [SerializeField] private float initialDelay;
    [SerializeField] private float cardDelay;
    private static readonly int Play = Animator.StringToHash("Play");

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
        StartCoroutine(Turn());
    }

    private IEnumerator Turn()
    {
        yield return new WaitForSeconds(initialDelay);
        
        animator.SetTrigger(Play);
        
        yield return new WaitForSeconds(cardDelay);
        
        Card randomCard = hand.cards[Random.Range(1, hand.cards.Count - 1)];
        stackObject.Add(randomCard);

        hand.PlayCard(randomCard);
    }
}
