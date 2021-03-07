using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : Dealer
{
    protected override void Start()
    {
        dealerType = DealerType.Player;

        base.Start();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            EndTurn();
        }   
    }

    public override void StartTurn()
    {
        Debug.Log(dealerType + " takes their turn.");
    }
}
