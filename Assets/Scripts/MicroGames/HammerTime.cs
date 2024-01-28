using UnityEngine;
using Unity.Mathematics;
using UnityEngine.InputSystem;

public class HammerTime : MicroGame
{
    public HammerTimeHelper HammerHelperPrefab;

    public override void InitMicroGame(MicroGame parent, PlayerInput player)
    {
        base.InitMicroGame(this, player);

        // Instantiate helper-objects
        GameObject instance = Instantiate(HammerHelperPrefab, player.transform.position, quaternion.identity, player.transform).gameObject;
        HammerTimeHelper helper = instance.GetComponent<HammerTimeHelper>();

        // Set references and values
        helper.Instantiator = this;
    }

    // Added because of inheritance-requirement
    protected override void RunMicroGame(){}
}
