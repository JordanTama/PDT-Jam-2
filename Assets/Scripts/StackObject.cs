using System.Collections.Generic;
using Cards;
using UnityEngine;

public class StackObject : MonoBehaviour
{
    [SerializeField] private GameObject cardPrefab;
    [SerializeField] private float gap;
    private readonly List<CardMaterialController> cards = new List<CardMaterialController>();

    
    
    public void Add(Card card)
    {
        GameObject cardObject = Instantiate(cardPrefab, transform);
        cardObject.transform.localPosition = Vector3.zero;
        cardObject.transform.localRotation = Quaternion.identity;
        cardObject.transform.position -= transform.forward * (gap * cards.Count);
        
        CardMaterialController controller = cardObject.GetComponent<CardMaterialController>();
        controller.Card = card;

        cards.Add(controller);
    }
}