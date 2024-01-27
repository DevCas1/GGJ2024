using System;
using UnityEngine;
using UnityEngine.InputSystem;

public abstract class MicroGame : MonoBehaviour
{
    public event EventHandler OnMicroGameComplete;

    protected PlayerInput[] players;

    public virtual void InitMicroGame(ref PlayerInput[] players)
    {
        this.players = players;
    }

    protected abstract void RunMicroGame();

    protected void MicroGameComplete()
    {
        OnMicroGameComplete.Invoke(this, null);
    }
}