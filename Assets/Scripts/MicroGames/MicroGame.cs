using System;
using UnityEngine;
using UnityEngine.InputSystem;

public abstract class MicroGame : MonoBehaviour
{
    public event EventHandler OnMicroGameComplete;

    protected PlayerInput player;

    public virtual void InitMicroGame(MicroGame parent, PlayerInput player)
    {
        parent = this;
        this.player = player;
    }

    protected abstract void RunMicroGame();

    public virtual void MicroGameComplete()
    {
        OnMicroGameComplete.Invoke(this, null);
    }
}