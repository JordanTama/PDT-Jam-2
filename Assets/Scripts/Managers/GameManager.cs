using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    [SerializeField] private bool playerStarts;
    [SerializeField] private int maxTurnsPerDealer;

    private GameService gameService;

    private void Awake()
    {
        gameService = ServiceLocator.ServiceLocator.Get<GameService>();

        gameService.playerStarts = playerStarts;
        gameService.maxTurnsPerDealer = maxTurnsPerDealer;
    }
}
