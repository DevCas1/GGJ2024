using Unity.Mathematics;
using UnityEngine;
using UnityEngine.InputSystem;

public class HammerTime : MicroGame
{
    public GameObject HammerHelperPrefab;

    public override void InitMicroGame(MicroGame parent, PlayerInput player)
    {
        base.InitMicroGame(this, player);

        // Instantiate helper-objects
        HammerTimeHelper instance = Instantiate(HammerHelperPrefab, player.transform.position, quaternion.identity, player.transform).GetComponent<HammerTimeHelper>();

        // Set references and values
        instance.Instantiator = this;
    }

    // Added because of inheritance-requirement
    protected override void RunMicroGame(){}
}
