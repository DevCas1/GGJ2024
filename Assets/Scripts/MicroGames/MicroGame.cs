using System;
using UnityEngine;
using UnityEngine.InputSystem;

public abstract class MicroGame : MonoBehaviour
{
    public event EventHandler OnMicroGameComplete;

    protected int PlayerCount
    {
        get => PlayerCount;

        private set { PlayerCount = value; }
    }

    public virtual void InitMicroGame(PlayerInput[] players)
    {
        PlayerCount = players.Length;
    }

    protected abstract void RunMicroGame();

    protected void MicroGameComplete()
    {
        OnMicroGameComplete.Invoke(this, null);
    }
}