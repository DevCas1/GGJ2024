using Unity.Mathematics;
using UnityEngine.InputSystem;

public class BombDrop : MicroGame
{
    public BombDropHelper BombHelperPrefab;

    public override void InitMicroGame(MicroGame parent, PlayerInput player)
    {
        base.InitMicroGame(this, player);

        // Instantiate helper-objects
        BombDropHelper instance = Instantiate(BombHelperPrefab, player.transform.position, quaternion.identity, player.transform).GetComponent<BombDropHelper>();

        // Set references and values
        instance.Instantiator = this;
    }

    // Added because of inheritance-requirement
    protected override void RunMicroGame(){}
}