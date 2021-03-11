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

    private Hand hand;

    private void Start()
    {
        hand = player.hand;

        hand.OnRemoveCard += card => RemoveCard(card);
        hand.OnDrawCard += card => CreateCard(card);
    }

    private void CreateCard(Card card)
    {
        GameObject newCardGameObject = Instantiate(cardPrefab, transform);
        CardController newCardController = newCardGameObject.GetComponent<CardController>();

        cardGameObjects.Add(newCardGameObject);
        cards.Add(card, newCardGameObject);

        newCardController.card = card;

        RepositionCards();
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

    private void RemoveCard(Card card)
    {
        GameObject cardGameObject = cards[card];

        Destroy(cardGameObject);

        cardGameObjects.Remove(cardGameObject);
        cards.Remove(card);

        RepositionCards();
    }
}
