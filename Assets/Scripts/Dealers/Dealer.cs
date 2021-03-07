using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Dealer : MonoBehaviour
{
    public enum DealerType
    {
        Player,
        Patron
    }
    
    public GameService.Role role { get; set; }

    protected DealerType dealerType;
    protected GameService gameService;

    protected virtual void Start()
    {
        gameService = ServiceLocator.ServiceLocator.Get<GameService>();

        gameService.RegisterDealer(dealerType, this);
    }

    public virtual void StartTurn()
    {
        Debug.Log(dealerType + " takes their turn.");

        EndTurn();
    }

    protected void EndTurn()
    {
        Debug.Log(dealerType + " ends their turn.");

        gameService.EndTurn();
    }
}
