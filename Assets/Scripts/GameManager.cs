using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public UnityEvent OnStartGame;
    public UnityEvent OnRoundComplete;
    public UnityEvent OnTurnComplete;

    public PlayerInputManager PlayerInputManager;
    public MicroGame[] MicroGames;
    [HideInInspector]
    public int[] pointCollectors = {0, 0, 0, 0};

    [SerializeField]
    private PlayerInput[] players = new PlayerInput[4];

    [SerializeField]
    private int maxRounds;
    private int currentRound;
    private int currentTurn;
    private MicroGame activeMicroGame;

    void Awake()
    {

        DontDestroyOnLoad(this.gameObject);

    }

    public void Init(int maxRounds)
    {
        this.maxRounds = maxRounds;
    }

    public void StartGame()
    {
        if (maxRounds == 0)
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

    private void Start()
    {
        if (players.Length < 4)
            Debug.LogWarning("Less than 4 players have been referenced!");

        currentRound = 1;
        currentTurn = 1;

        Debug.Log("Starting game with Round 1");

        RunRound();
    }

    private void RunRound()
    {
        if (currentRound > maxRounds)
        {
            // Announce end of game
            Debug.Log("Game Ended!");
            SceneManager.LoadScene("EndScene-Hidde");
            return;
        }

        if (currentRound == maxRounds)
        {
            // Announce last round
            Debug.Log("This is the last round!");
        }

        currentTurn = 1;

        RunTurn();
    }

    private void RunTurn()
    {
        Debug.Log("Running turn " + currentTurn);

        ChooseMicroGame();
        StartMicroGame();
    }

    private void ChooseMicroGame()
    {
        Debug.Log("Choosing Micro Game...");

        int randomIndex = UnityEngine.Random.Range(0, MicroGames.Length);
        activeMicroGame = MicroGames[randomIndex];

        Debug.Log("Random index: " + randomIndex);

        // Effect for choosing MicroGame

        activeMicroGame.OnMicroGameComplete += OnMicroGameComplete;
    }

    private void StartMicroGame()
    {
        activeMicroGame.InitMicroGame(null, players[currentTurn - 1]);
    }

    private void OnMicroGameComplete(object sender, EventArgs e)
    {
        activeMicroGame.OnMicroGameComplete -= OnMicroGameComplete;
        activeMicroGame = null;

        EndTurn();
    }

    private void EndTurn()
    {
        if (currentTurn == players.Length)
        {
            EndRound();
            return;
        }

        currentTurn++;
        RunTurn();
    }

    private void EndRound()
    {
        currentTurn = 1;
        currentRound++;

        RunRound();
    }

    public bool RegisterPoint(Transform collector, Point point)
    {
        // int collectorIndex = collector.name[^1] - 1;

        // Debug.Log($"Player {collector.name} ({collectorIndex} collected {point.Value} points");

        // pointCollectors[collectorIndex] += point.Value;

        switch(collector.name)
        {
            case "Player 1":
                pointCollectors[0] += point.Value;
                break;
            case "Player 2":
                pointCollectors[1] += point.Value;
                break;
            case "Player 3":
                pointCollectors[2] += point.Value;
                break;
            case "Player 4":
                pointCollectors[3] += point.Value;
                break;
        }

        Destroy(point.gameObject);
        return true;
    }
}