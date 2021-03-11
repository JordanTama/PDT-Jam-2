using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CardController : MonoBehaviour
{    
    public Card card;

    private new MeshRenderer renderer;

    private void Start()
    {
        renderer = GetComponent<MeshRenderer>();

        renderer.material.SetColor("_BaseColor", card.color);
    }

    private void OnMouseOver()
    {
        if (Input.GetMouseButtonDown(0))
        {
             card.PlayCard();
        }
    }
}
