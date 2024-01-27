using System;
using UnityEngine;

public class TurnManager : MonoBehaviour
{
    public event EventHandler OnStartGame;
    public event EventHandler OnTurnComplete;

    private int maxTurns = 0;
    private int currentTurn;

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
    }

    private void RunTurn()
    {
        if (currentTurn >= maxTurns)
        {
            // Announce end of game
        }

        // Announce turn

        // Choose "micro game" for this turn
        // Run chosen "micro game"

        currentTurn++;
        OnTurnComplete.Invoke(this, null);
    }
}