using System;
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

    public override void StartTurn()
    {
        Debug.Log(dealerType + " takes their turn.");
    }
}
