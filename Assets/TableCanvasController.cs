using System;
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class TableCanvasController : MonoBehaviour
{
    [SerializeField] private float restartTime;
    [SerializeField] private Text stack;
    [SerializeField] private Text player;
    [SerializeField] private Text opponent;
    [SerializeField] private Text message;

    [SerializeField] private TableController table;
    [SerializeField] private GameService gameService;

    private void Awake()
    {
        gameService = ServiceLocator.ServiceLocator.Get<GameService>();
    }

    private void Start()
    {
        message.gameObject.SetActive(false);
        
        player.text = table.Player.targetValue.ToString("D3");
        opponent.text = table.Patron.targetValue.ToString("D3");
        
        table.OnChangeValue += (value) => stack.text = value.ToString("D3");
        gameService.onEndGame += () => ShowEndText();
    }

    private void ShowEndText()
    {
        message.gameObject.SetActive(true);
        message.text = gameService.winningReason;
        StartCoroutine(Restart());
    }

    private IEnumerator Restart()
    {
        yield return new WaitForSeconds(restartTime);

        ServiceLocator.ServiceLocator.Initialize();
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}
