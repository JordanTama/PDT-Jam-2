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
    private Dictionary<GameObject, Card> cards = new Dictionary<GameObject, Card>();
    private Dictionary<GameObject, CardController> cardControllers = new Dictionary<GameObject, CardController>();
    private bool cardsPlayable;

    private void Awake()
    {
        player.OnStartTurn += () => SetCardsPlayable(true);
        player.OnEndTurn += () => SetCardsPlayable(false);
        player.OnDrawCard += CreateCard;
    }

    private void CreateCard(Card card)
    {
        GameObject newCardGameObject = Instantiate(cardPrefab, transform);
        CardController newCardController = newCardGameObject.GetComponent<CardController>();

        cardGameObjects.Add(newCardGameObject);
        cards.Add(newCardGameObject, card);
        cardControllers.Add(newCardGameObject, newCardController);

        newCardController.card = card;
        newCardController.OnCardPlayed += PlayCard;
        newCardController.playable = cardsPlayable;

        RepositionCards();
    }

    private void PlayCard(GameObject cardGameObject)
    {
        Destroy(cardGameObject);

        player.PlayCard(cards[cardGameObject]);

        cardGameObjects.Remove(cardGameObject);
        cards.Remove(cardGameObject);
        cardControllers.Remove(cardGameObject);

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

    private void SetCardsPlayable(bool playable)
    {
        cardsPlayable = playable;

        foreach (KeyValuePair<GameObject, CardController> item in cardControllers)
        {
            item.Value.playable = playable;
        }
    }
}
