using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Patron : Dealer
{
    protected override void Start()
    {
        dealerType = DealerType.Patron;

        base.Start();
    }
}
