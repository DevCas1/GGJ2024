using Unity.Mathematics;
using UnityEngine;
using UnityEngine.InputSystem;

public class HammerTime : MicroGame
{
    public GameObject HammerPrefab;

    public override void InitMicroGame(PlayerInput[] players)
    {
        // Instantiate helper-objects
        for (int index = 0; index < PlayerCount; index ++)
        {
            Instantiate(HammerPrefab, players[PlayerCount].transform.position, quaternion.identity);
        }
        
        // Set references and values
        
        RunMicroGame();
    }

    protected override void RunMicroGame()
    {
        throw new System.NotImplementedException();
    }
}
