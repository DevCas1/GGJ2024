using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class GameManager : MonoBehaviour
{
    public event EventHandler OnStartGame;
    public event EventHandler OnTurnComplete;

    public PlayerInputManager PlayerInputManager;
    public MicroGame[] MicroGames;

    private int maxTurns = 0;
    private int currentTurn;
    private PlayerInput[] players;
    private int playerCount => players.Length;
    private MicroGame activeMicroGame;

    public void Init(int maxTurns)
    {
        this.maxTurns = maxTurns;
    }


    public void StartGame()
    {
        if (maxTurns == 0)
        {
            Debug.LogError("Can't start the game before calling TurnManager.Init()!");
            return;
        }

        if (MicroGames.Length < 1)
        {
            Debug.LogError("Can't start without any microgames set!");
            return;
        }

        players = FindObjectsOfType<PlayerInput>();

        if (players.Length < 1 )
        {
            Debug.LogError("Game cannot be played with fewer than 1 player!");
            return;
        }
    }

    private void RunTurn()
    {
        if (currentTurn >= maxTurns)
        {
            // Announce end of game
        }

        // Announce turn


        ChooseMicroGame();
        StartMicroGame();

        currentTurn++;
        OnTurnComplete.Invoke(this, null);
    }

    private void ChooseMicroGame()
    {
        activeMicroGame = MicroGames[0];
        activeMicroGame.OnMicroGameComplete += OnMicroGameComplete;
    }

    private void StartMicroGame()
    {
        activeMicroGame.InitMicroGame(ref players);
    }

    private void OnMicroGameComplete(object sender, EventArgs e)
    {
        activeMicroGame.OnMicroGameComplete -= OnMicroGameComplete;

        activeMicroGame = null;
    }
}