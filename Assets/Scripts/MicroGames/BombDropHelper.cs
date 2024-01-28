using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;

public class BombDropHelper : MonoBehaviour
{
    public UnityEvent OnBombExplode;
    
    [Header("Throw Values")]
    [Min(0.0001f)]
    public float RotateSpeedIncrementMultiplier = 1;
    [Range(0, 1)]
    public float RotateSpeedDecreaseMultiplier = 0.5f;
    [Min(0.01f)]
    public float ThrowDistanceMultiplier = 1;
    [Range(0, 1)]
    public float UpwardAngle = 0.3f;

    [Header("Bomb Values")]
    [Min(1)]
    public float BombExplodeTimeRangeMin = 7;
    [Range(1, 10)]
    public float BombExplodeTimeRangeMax = 10;
    [Range(0.01f, 100)]
    public float BombExplosionForce = 1;
    [Range(0.1f, 5)]
    public float BombExplosionRadius = 2;
    [Range(0, 3)]
    public float BombExplosionUpwardModifier = 1;
    public LayerMask BombExplosionCheckLayers;

    [Header("References")]
    public Rigidbody Bomb;
    public Transform BombPivot;
    public GameObject Cursor;

    [HideInInspector]
    public BombDrop Instantiator;

    private bool isCharging = true;
    private float rotationSpeed = 0;
    private float detonationTime;
    private Collider[] hitPlayers = new Collider[4];

    void Start()
    {
        Bomb.isKinematic = true;
    }

    void Update()
    {
        if (isCharging && Gamepad.current.buttonSouth.wasPressedThisFrame)
            IncreaseRotationSpeed();
        else if (rotationSpeed > 0)
            DecreaseRotationSpeed();

        if (isCharging && Gamepad.current.buttonEast.wasPressedThisFrame)
            ThrowBomb();

        if (Time.time >= detonationTime)
            ExplodeBomb();

        if (isCharging)
            transform.Rotate(new(0, rotationSpeed, 0));
    }

    private void StartBombCountdown()
    {
        isCharging = true;
        detonationTime = Time.time + Random.Range(BombExplodeTimeRangeMin, BombExplodeTimeRangeMax);
    }

    private void IncreaseRotationSpeed() => rotationSpeed += 1 * RotateSpeedIncrementMultiplier;

    private void DecreaseRotationSpeed()
    {
        rotationSpeed -= 1 * RotateSpeedDecreaseMultiplier;

        if (rotationSpeed < 0)
            rotationSpeed = 0;
    }

    private void ThrowBomb()
    {
        isCharging = false;
        Bomb.isKinematic = false;
        
        BombPivot.SetParent(null, true);
        Bomb.AddForce(rotationSpeed * ThrowDistanceMultiplier * Vector3.Slerp(transform.forward, Vector3.up, UpwardAngle)); // TODO: Make throw work
        Debug.Log("Bomb thrown");
    }

    private void ExplodeBomb()
    {
        Bomb.isKinematic = true;

        Bomb.Sleep();
        OnBombExplode?.Invoke();

        for (int index = 0; 
             index < Physics.OverlapSphereNonAlloc(Bomb.transform.position, BombExplosionRadius, hitPlayers, BombExplosionCheckLayers); 
             index++)
        {            
            if (hitPlayers[index].TryGetComponent<Rigidbody>(out var rigidbody))
                rigidbody.AddExplosionForce(BombExplosionForce, BombPivot.position, BombExplosionRadius, BombExplosionUpwardModifier);
        }
    }
}
