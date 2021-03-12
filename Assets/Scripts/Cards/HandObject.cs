using System;
using System.Collections;
using System.Collections.Generic;
using Cards;
using UnityEngine;

public class HandObject : MonoBehaviour
{
    public bool CanPlay => service.IsPlayerTurn() && !service.GameEnded;

    [SerializeField] private Player player;
    [SerializeField] private Animator playerAnimator;
    [SerializeField] private float animationDelay;
    [SerializeField] private StackObject stackObject;
    [SerializeField] private LayerMask mask;
    [SerializeField] private float spread;
    [SerializeField] private float depthGap;
    [SerializeField] private float hoverDistance;
    [SerializeField] private GameObject cardPrefab;
    [SerializeField] private Card[] startingHand;

    private List<CardMaterialController> cards = new List<CardMaterialController>();
    private CardMaterialController hover;

    private bool hasChanged;
    private GameService service;

    private void Awake()
    {
        service = ServiceLocator.ServiceLocator.Get<GameService>();
    }

    private void Start()
    {
        foreach (Card card in startingHand)
            Add(card);
    }

    private void Update()
    {
        UpdateHover();

        if (hover && CanPlay && Input.GetMouseButtonDown(0))
            PlayCard(hover);
    }

    private void LateUpdate()
    {
        if (!hasChanged)
            return;
        
        AdjustPositions();
        hasChanged = false;
    }

    private void PlayCard(CardMaterialController card)
    {
        player.PlayCard(card.Card);
        player.RemoveCard(card.Card);
        Remove(card);
    }

    private void UpdateHover()
    {
        CardMaterialController card = GetMouseOverFirst();
        
        if (hover != card)
            hasChanged = true;

        hover = card;
    }

    private CardMaterialController GetMouseOverClosest()
    {
        if (cards.Count == 0)
            return null;
        
        RaycastHit[] hits = new RaycastHit[cards.Count];
        Physics.RaycastNonAlloc(Camera.main.ScreenPointToRay(Input.mousePosition), hits, Mathf.Infinity, mask);

        RaycastHit closest = hits[0];
        float minDistance = Mathf.Infinity;
        foreach (RaycastHit hit in hits)
        {
            if (!hit.transform || hit.distance >= minDistance)
                continue;

            minDistance = hit.distance;
            closest = hit;
        }
        
        return !closest.transform ? null : closest.transform.GetComponent<CardMaterialController>();
    }

    private CardMaterialController GetMouseOverFirst()
    {
        if (!Camera.main)
            return null;
        
        foreach (CardMaterialController card in cards)
        {
            if (card.Intersects(Camera.main.ScreenPointToRay(Input.mousePosition)))
                return card;
        }

        return null;
    }

    private void AdjustPositions()
    {
        float gap = cards.Count > 1
            ? spread / (cards.Count - 1f)
            : spread / 2f;
        
        for (int i = 0; i < cards.Count; i++)
        {
            Vector3 position = transform.position - transform.right * (spread / 2f);
            position += transform.right * (gap * i);

            if (cards[i] == hover)
                position += transform.up * hoverDistance;
            else
                position += transform.forward * ((i + 1) * depthGap);

            cards[i].transform.position = position;
        }
    }

    public void Add(Card card)
    {
        GameObject cardObject = Instantiate(cardPrefab, transform);
        cardObject.transform.localPosition = Vector3.zero;
        cardObject.transform.localRotation = Quaternion.identity;
        
        CardMaterialController controller = cardObject.GetComponent<CardMaterialController>();
        controller.Card = card;

        cards.Add(controller);

        hasChanged = true;
    }

    public void Remove(CardMaterialController card)
    {
        DestroyImmediate(card.gameObject);
        cards.Remove(card);

        StartCoroutine(PlaceOnStack(animationDelay, card.Card));
        playerAnimator.SetTrigger("Play");

        hasChanged = true;
    }

    private IEnumerator PlaceOnStack(float delay, Card card)
    {
        yield return new WaitForSeconds(delay);
        
        stackObject.Add(card);
    }
}
