using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    public event System.Action OnReachedEndOfLevel;

    public float moveSpeed = 7;
    public float smoothMoveTime = .1f;
    public float turnSpeed = 8;

    float angle;
    float smoothInputMagnitude;
    float smoothMoveVelocity;
    Vector3 velocity;

    Rigidbody rb;
    bool disabled;

    private void Start() {
        rb = GetComponent<Rigidbody>();
        Guard.OnGuardHasSpottedPlayer += Disable;
    }

    private void Update() {
        Vector3 inputDirection = Vector3.zero;
        if (!disabled)
            inputDirection = new Vector3(Input.GetAxisRaw("Horizontal"), 0, Input.GetAxisRaw("Vertical")).normalized;
        float inputMagnitude = inputDirection.magnitude;
        smoothInputMagnitude = Mathf.SmoothDamp(smoothInputMagnitude, inputMagnitude, ref smoothMoveVelocity, smoothMoveTime);

        float targetAngle = Mathf.Atan2(inputDirection.x, inputDirection.z) * Mathf.Rad2Deg;
        angle = Mathf.LerpAngle(angle, targetAngle, turnSpeed * Time.deltaTime * inputMagnitude);

        velocity = transform.forward * moveSpeed * smoothInputMagnitude;
    }

    private void FixedUpdate() {
        rb.MoveRotation(Quaternion.Euler(Vector3.up * angle));
        rb.MovePosition(rb.position + velocity * Time.deltaTime);
    }

    private void OnTriggerEnter(Collider other) {
        if (other.CompareTag(Tags.Finish)) {
            Disable();
            if (OnReachedEndOfLevel != null)
                OnReachedEndOfLevel();
        }
    }

    private void Disable() {
        disabled = true;
    }

    private void OnDestroy() {
        Guard.OnGuardHasSpottedPlayer -= Disable;
    }
}
