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

    private Dictionary<GameObject, Card> cards = new Dictionary<GameObject, Card>();
    private List<GameObject> cardGameObjects = new List<GameObject>();

    private void Start()
    {
        player.OnDrawCard += CreateCard;
    }

    private void CreateCard(Card card)
    {
        GameObject newCardGameObject = Instantiate(cardPrefab, transform);

        cards.Add(newCardGameObject, card);

        cardGameObjects.Add(newCardGameObject);

        CardController newCardController = newCardGameObject.GetComponent<CardController>();

        newCardController.card = card;

        newCardController.OnCardPlayed += PlayCard;

        RepositionCards();
    }

    private void PlayCard(GameObject cardGameObject)
    {
        Destroy(cardGameObject);

        player.PlayCard(cards[cardGameObject]);

        cards.Remove(cardGameObject);

        cardGameObjects.Remove(cardGameObject);

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
}
