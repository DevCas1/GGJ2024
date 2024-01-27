using Unity.Mathematics;
using UnityEngine;
using UnityEngine.InputSystem;

public class HammerTime : MicroGame
{
    public GameObject HammerHelperPrefab;

    public override void InitMicroGame(ref PlayerInput[] players)
    {
        // Set references and values

        // Instantiate helper-objects
        for (int index = 0; index < players.Length; index ++)
        {
            var helper = Instantiate(HammerHelperPrefab, players[index].transform.position, quaternion.identity).GetComponent<HammerTimeHelper>();
            helper.playerInput = players[index];
            // players[index].onActionTriggered += helper.Test;
        }
        
        RunMicroGame();
    }

    protected override void RunMicroGame()
    {
        throw new System.NotImplementedException();
    }
}
