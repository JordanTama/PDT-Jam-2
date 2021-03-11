using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HandController : MonoBehaviour
{
    [SerializeField] private Player player;
    [SerializeField] private GameObject cardPrefab;
    [SerializeField] private Vector3 centrePosition;
    [SerializeField] private float distanceBetweenCards;
    [SerializeField] private float cardWidth;

    private List<GameObject> cardGameObjects = new List<GameObject>();
    private Dictionary<Card, GameObject> cards = new Dictionary<Card, GameObject>();
    private bool cardsPlayable;
    private List<Card> expiredCards = new List<Card>();

    private void Awake()
    {
        player.OnStartTurn += () => SetCardsPlayable(true);
        player.OnStartTurn += CardsStartTurn;
        player.OnEndTurn += () => SetCardsPlayable(false);
        player.OnDrawCard += CreateCard;
    }

    private void CreateCard(Card card)
    {
        GameObject newCardGameObject = Instantiate(cardPrefab, transform);
        CardController newCardController = newCardGameObject.GetComponent<CardController>();

        cardGameObjects.Add(newCardGameObject);
        cards.Add(card, newCardGameObject);

        newCardController.card = card;

        card.OnPlayed += PlayCard;
        card.playable = cardsPlayable;
        card.OnExpiry += thisCard => expiredCards.Add(thisCard);

        RepositionCards();
    }

    private void PlayCard(Card card)
    {
        player.PlayCard(card);

        RemoveCard(card);
    }

    private void RepositionCards()
    {
        for (int i = 0; i < cardGameObjects.Count; i++)
        {
            GameObject cardGameObject = cardGameObjects[i];

            float spacingOffset = distanceBetweenCards * i;

            float centeringOffset = (distanceBetweenCards / 2) * (cardGameObjects.Count - 1);

            cardGameObject.transform.position = transform.position + Vector3.right * (spacingOffset - centeringOffset);
        }
    }

    private void SetCardsPlayable(bool playable)
    {
        cardsPlayable = playable;

        foreach (KeyValuePair<Card, GameObject> item in cards)
        {
            item.Key.playable = playable;
        }
    }

    private void CardsStartTurn()
    {
        foreach (KeyValuePair<Card, GameObject> item in cards)
        {
            item.Key.StartTurn();
        }

        RemoveExpiredCards();
    }

    private void RemoveCard(Card card)
    {
        GameObject cardGameObject = cards[card];

        Destroy(cardGameObject);

        cardGameObjects.Remove(cardGameObject);
        cards.Remove(card);

        RepositionCards();
    }

    private void RemoveExpiredCards()
    {
        foreach (Card card in expiredCards)
        {
            RemoveCard(card);
        }

        expiredCards.Clear();
    }
}
