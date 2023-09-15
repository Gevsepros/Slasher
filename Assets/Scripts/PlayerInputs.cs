using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
public class PlayerInputs : MonoBehaviour
{
    [SerializeField] float speedMultiplier = 20f;
    [SerializeField] Camera playerCam;
    [SerializeField] float runningMultiplier = 2f;
    Rigidbody rb;
    Animator animator;
    Vector3 moveDir = Vector3.zero;
    Vector2 moveInputValue;
    bool isRunning = false;

    void Awake()
    {
        rb = GetComponent<Rigidbody>();
        animator = GetComponent<Animator>();
    }
    void Update()
    {
        moveDir += moveInputValue.x * GetCameraRight(playerCam) * speedMultiplier * (isRunning ? runningMultiplier : 1);
        moveDir += moveInputValue.y * GetCameraForward(playerCam) * speedMultiplier * (isRunning ? runningMultiplier : 1);

        rb.AddForce(moveDir, ForceMode.Impulse);
        animator.SetFloat("Speed", moveDir.sqrMagnitude);
        LookAt();
        moveDir = Vector3.zero;
    }

    Vector3 GetCameraForward(Camera cam)
    {
        Vector3 forward = cam.transform.forward;
        forward.y = 0f;
        return forward.normalized;
    }

    Vector3 GetCameraRight(Camera cam)
    {
        Vector3 right = cam.transform.right;
        right.y = 0f;
        return right.normalized;
    }

    void OnMove(InputValue value)  
    {
        moveInputValue = value.Get<Vector2>().normalized;
        if (moveInputValue == Vector2.zero) isRunning = false; 
    }

    void OnRun()
    {
        isRunning = true;
    }
    void LookAt()
    {
        Vector3 direction = rb.velocity;
        direction.y = 0f;
        if (moveDir.sqrMagnitude > 0.1f && direction.sqrMagnitude > 0.1f)
        {
            rb.rotation = Quaternion.LookRotation(direction, Vector3.up);
        }
        else 
        {
            rb.angularVelocity = Vector3.zero;
        }
    }
}
