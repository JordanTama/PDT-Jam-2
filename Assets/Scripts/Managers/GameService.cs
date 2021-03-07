﻿using System;
using ServiceLocator;
using UnityEngine;

public class GameService : IService
{
    public enum Role
    {
        Buyer,
        Seller
    }

    public Action onStartGame;
    public Action onStartPlayerTurn;
    public Action onStartPatronTurn;
    public Action onEndGame;

    public bool playerStarts { get; set; } = true;
    public int maxTurnsPerDealer { get; set; } = 3;

    private Dealer player;
    private Dealer patron;
    private Dealer currentDealer;
    private int turnsTaken = 0;

    public void RegisterDealer(Dealer.DealerType dealerType, Dealer dealer)
    {
        switch (dealerType)
        {
            case Dealer.DealerType.Player:
                if (player == null)
                {
                    player = dealer;
                    Debug.Log("Player registered.");
                } else
                {
                    Debug.Log("Player already registered.");
                }
                
                break;
            case Dealer.DealerType.Patron:
                if (patron == null)
                {
                    patron = dealer;
                    Debug.Log("Patron registered.");
                }
                else
                {
                    Debug.Log("Patron already registered.");
                }

                break;
            default:
                throw new ArgumentOutOfRangeException();
        }

        if (player != null && patron != null)
        {
            StartGame();
        }
    }

    private void StartGame()
    {
        if (playerStarts)
        {
            player.role = Role.Seller;
            patron.role = Role.Buyer;

            currentDealer = player;
        }
        else
        {
            player.role = Role.Buyer;
            patron.role = Role.Seller;

            currentDealer = patron;
        }

        Debug.Log("Game started. Current dealer is " + currentDealer);

        currentDealer.StartTurn();
    }

    public void EndTurn()
    {
        currentDealer = currentDealer == player ? patron : player;

        turnsTaken++;

        if (turnsTaken >= maxTurnsPerDealer * 2)
        {
            EndGame();
        } else
        {
            currentDealer.StartTurn();
        }
    }

    private void EndGame()
    {
        Debug.Log("Game ends.");

        onEndGame?.Invoke();
    }
}
