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
    [Range(0.01f, 1000)]
    public float BombExplosionForce = 1;
    [Range(0.1f, 5)]
    public float BombExplosionRadius = 2;
    [Range(0, 3)]
    public float BombExplosionUpwardModifier = 1;
    public LayerMask BombExplosionCheckLayers;

    [Header("Bomb Blink Values")]
    public float SlowBlinkTimeTilExplosion = 4;
    public float SlowBlinkSpeed = 1;
    public float FastBlinkTimeTilExplosion = 2;
    public float FastBlinkSpeed = 3;

    [Header("References")]
    public Renderer Bomb;
    public Rigidbody BombRigidbody;
    public Transform BombPivot;
    public GameObject Cursor;

    [HideInInspector]
    public MicroGame Instantiator;

    private bool isCharging = true;
    private float rotationSpeed = 0;
    private float detonationTime;
    private int hitPlayersCount;
    private Collider[] hitPlayers = new Collider[4];
    private bool exploded;

    void Start()
    {
        BombRigidbody.isKinematic = true;
        BombRigidbody.GetComponent<Collider>().isTrigger = true;
        detonationTime = -1;
        StartBombCountdown();
    }

    void Update()
    {
        if (Gamepad.current != null && isCharging && Gamepad.current.buttonSouth.wasPressedThisFrame)
            IncreaseRotationSpeed();
        else if (rotationSpeed > 0)
            DecreaseRotationSpeed();

        if (Gamepad.current != null && isCharging && Gamepad.current.buttonEast.wasPressedThisFrame)
            ThrowBomb();

        if (detonationTime > 0)
        {
            if (Time.time >= detonationTime)
            {
                if (!exploded)
                    ExplodeBomb();
            }
            else
                UpdateBombBlink();
        }

        if (isCharging)
            transform.Rotate(new(0, rotationSpeed, 0));

        if (exploded)
            CheckForSleepingTargets();
    }

    private void StartBombCountdown()
    {
        isCharging = true;
        detonationTime = Time.time + Random.Range(BombExplodeTimeRangeMin, BombExplodeTimeRangeMax);
        Debug.Log($"Bomb explodes in {detonationTime - Time.time} seconds");
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
        BombRigidbody.isKinematic = false;
        BombRigidbody.GetComponent<Collider>().isTrigger = false;
        
        Cursor.SetActive(false);
        BombPivot.SetParent(null, true);
        BombRigidbody.AddForce(rotationSpeed * ThrowDistanceMultiplier * Vector3.Slerp(transform.forward, Vector3.up, UpwardAngle)); // TODO: Make throw work
        Debug.Log("Bomb thrown");
    }

    private void ExplodeBomb()
    {
        if (isCharging)
            ThrowBomb();

        Debug.Log("Bomb exploded");

        BombRigidbody.isKinematic = true;

        BombRigidbody.Sleep();
        OnBombExplode?.Invoke();

        for (int index = 0; 
             index < (hitPlayersCount = Physics.OverlapSphereNonAlloc(BombRigidbody.transform.position, BombExplosionRadius, hitPlayers, BombExplosionCheckLayers)); 
             index++)
        {            
            if (hitPlayers[index].TryGetComponent<Rigidbody>(out var rigidbody))
            {
                rigidbody.AddExplosionForce(BombExplosionForce, BombPivot.position, BombExplosionRadius, BombExplosionUpwardModifier);
                Debug.Log($"Bomb hit {rigidbody.transform.root.name}");
            }
                
        }

        exploded = true;

        Destroy(BombPivot.gameObject);
        Destroy(BombRigidbody.gameObject);
    }

    private void UpdateBombBlink()
    {
        float timeTilDetonation = detonationTime - Time.time;
        Color blinkColor = new Color(10, 0, 0, 1);

        if (timeTilDetonation < FastBlinkTimeTilExplosion)
            Bomb.material.SetColor("_EmissionColor", Color.Lerp(Color.black, blinkColor, Mathf.Sin(Time.time * FastBlinkSpeed)));
        else if (timeTilDetonation < SlowBlinkTimeTilExplosion)
            Bomb.material.SetColor("_EmissionColor", Color.Lerp(Color.black, blinkColor, Mathf.Sin(Time.time * SlowBlinkSpeed)));
    }

    private void CheckForSleepingTargets()
    {
        int activeRigidbodies = 0;

        for (int index = 0; index < hitPlayersCount; index++)
        {
            if (!hitPlayers[index].attachedRigidbody.IsSleeping())
                activeRigidbodies++;
        }

        if (activeRigidbodies == 0)
        {
            Instantiator.MicroGameComplete();
            Destroy(gameObject);
        }
    }
}
