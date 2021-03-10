using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TableController : MonoBehaviour
{
    private List<Card> pile = new List<Card>();

    public void PlayCard(Dealer dealer, Card card)
    {
        pile.Add(card);
    }
}
