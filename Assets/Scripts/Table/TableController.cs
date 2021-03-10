using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TableController : MonoBehaviour
{
    [SerializeField] private Player player;
    [SerializeField] private Patron patron;

    public Action<int> OnChangeValue;

    private GameService gameService;

    private int Value
    {
        get
        {
            return value;
        }
        set
        {
            this.value = value;

            OnChangeValue?.Invoke(Value);
            
            if (Value >= player.targetValue)
            {
                gameService.EndGame("Player reached target value.");
            } else if (Value <= patron.targetValue)
            {
                gameService.EndGame("Patron reached target value.");
            }
        }
    }

    private int value;

    private List<Card> pile = new List<Card>();

    private void Start()
    {
        gameService = ServiceLocator.ServiceLocator.Get<GameService>();
    }

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
