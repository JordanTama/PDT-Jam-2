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
        // Process Card Effects
        ComboEffect(card);
        MarkupEffect(card);

        pile.Add(card);

        Value += card.value;
    }

    private void ComboEffect(Card card)
    {
        if (!card.hasComboEffect) return;

        if (pile.Count == 0) return;

        if (pile[pile.Count - 1].value != card.cardValueToActivateCombo) return;

        Value += card.valueToAddOnComboActivation;
    }

    private void MarkupEffect(Card card)
    {
        if (!card.hasMarkupEffect) return;

        if (pile.Count == 0) return;

        Value += (int) Math.Floor(card.markupMultiplier * pile[pile.Count - 1].value * 2);
    }
}
