using System;
using DG.Tweening;
using UnityEngine;
using UnityEngine.InputSystem;

public class HammerTimeHelper : MonoBehaviour
{
    [Range(1, 359), Header("Values")]
    public float MaxHammerRotation = 135;
    [Range(0, 10)]
    public float MaxChargeTime = 3;
    [Min(0.01f)]
    public float ChargeStrengthMultiplier = 1;
    [Range(0, 1)]
    public float UpwardAngle = 0.5f;

    [Header("References")]
    public Transform HammerPivot;
    public GameObject Arrow;
    
    [HideInInspector]
    public HammerTime Instantiator;

    private bool canRotate = true;
    private bool smashCharging = false;
    private float chargeStartTime;
    private Rigidbody smashedRigidbody;

    private void Start()
    {
        canRotate = true;
        smashCharging = false;
    }

    private void Update()
    {
        RotateHammer();

        if (!smashCharging && Gamepad.current.buttonSouth.wasPressedThisFrame)
            SmashStart();

        if (smashCharging && !Gamepad.current.buttonSouth.wasReleasedThisFrame)
            EndSmash();

        if (!canRotate && !smashCharging)
            CheckSmashedRigidbody();
    }

    private void RotateHammer()
    {
        Vector2 input = Gamepad.current?.leftStick?.ReadValue() ?? Vector2.zero;

        if (canRotate && input.sqrMagnitude > 0)
            transform.forward = new(input.x, 0, input.y);
    }

    private void SmashStart()
    {
        smashCharging = true;
        chargeStartTime = Time.time;
        HammerPivot.DOLocalRotate(new(MaxHammerRotation, 0, 0), MaxChargeTime);
    }

    private void EndSmash()
    {
        HammerPivot.DOKill();
        canRotate = false;
        smashCharging = false;

        float chargeTime = Time.time - chargeStartTime;

        if (chargeTime > MaxChargeTime)
            chargeTime = MaxChargeTime;

        Arrow.SetActive(false);
        HammerPivot.DOLocalRotate(Vector3.zero, 0.15f, RotateMode.FastBeyond360).OnComplete(() =>
        {
            smashedRigidbody = GetComponentInParent<Rigidbody>();
            smashedRigidbody.AddForce(ChargeStrengthMultiplier * chargeTime * Vector3.Slerp(transform.forward, Vector3.up, UpwardAngle));

            Destroy(gameObject);
        });
    }

    private void CheckSmashedRigidbody()
    {
        if (smashedRigidbody.IsSleeping())
        {
            if (Instantiator != null)
                Instantiator.MicroGameComplete();
            else
                Debug.LogWarning("HammerTimeHelper was not instantiated through HammerTime MicroGame, or the reference was cleared before parent.MicroGameComplete() could be called!");
        }
    }
}