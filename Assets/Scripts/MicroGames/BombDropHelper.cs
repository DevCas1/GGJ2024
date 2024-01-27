using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class BombDropHelper : MonoBehaviour
{
    
    [Header("Values")]
    [Min(0.01f)]
    public float RotateSpeedIncrementMultiplier = 1;
    [Range(0, 1)]
    public float RotateSpeedDecreaseMultiplier = 0.5f;
    [Min(0.01f)]
    public float ThrowDistanceMultiplier = 1;
    [Range(0, 1)]
    public float UpwardAngle = 0.5f;

    [Header("References")]
    public Rigidbody Bomb;
    public Transform BombPivot;
    public GameObject Cursor;

    [HideInInspector]
    public BombDrop Instantiator;

    private bool isCharging = true;
    private float rotationSpeed = 0;
    private Rigidbody[] smashedRigidbodies;

    // Start is called before the first frame update
    void Start()
    {
        rotationSpeed = 0;
        isCharging = true;
    }

    // Update is called once per frame
    void Update()
    {
        if (isCharging && Gamepad.current.buttonSouth.wasPressedThisFrame)
            IncreaseRotationSpeed();
        else
            DecreaseRotationSpeed();

        if (isCharging && Gamepad.current.buttonEast.wasPressedThisFrame)
            ThrowBomb();
    }

    private void IncreaseRotationSpeed() => rotationSpeed += 1 * RotateSpeedIncrementMultiplier;

    private void DecreaseRotationSpeed() => rotationSpeed -= 1 * RotateSpeedDecreaseMultiplier;

    private void ThrowBomb()
    {
        isCharging = false;
        
        BombPivot.SetParent(null, true);
        Bomb.AddForce(rotationSpeed * transform.forward);
    }
}
