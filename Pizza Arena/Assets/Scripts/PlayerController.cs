using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class PlayerController : MonoBehaviour
{
    Rigidbody rb;

    Vector3 moveDirection;
    Vector3 lookDirection;

    public float speed = 6f;

    // helper variables for smoother turning
    float turnSmoothTime = 0.1f;
    float turnSmoothVelocity;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    void Update()
    {
        // calc direction the player looks in
        float lookHorizontal = Input.GetAxis("AimHorizontal");
        float lookVertical = Input.GetAxis("AimVertical");
        lookDirection = new Vector3(lookHorizontal, 0f, lookVertical).normalized;

        // if there is input change, apply smoothing and rotate player to desired angle
        if (lookDirection.magnitude >= 0.1f)
        {
            float targetAngle = Mathf.Atan2(lookDirection.x, lookDirection.z) * Mathf.Rad2Deg;
            float angle = Mathf.SmoothDampAngle(transform.eulerAngles.y, -targetAngle, ref turnSmoothVelocity, turnSmoothTime);
            transform.rotation = Quaternion.Euler(0f, angle, 0f);
        }
    }

    void FixedUpdate()
    {
        // calc direction player will move in
        float moveHorizontal = Input.GetAxis("Horizontal");
        float moveVertical = Input.GetAxis("Vertical");
        moveDirection = new Vector3(moveHorizontal, 0f, moveVertical).normalized;

        // calc the current angle of the stick
        // (to change speed depending on how much the stick is tilted)
        float stickAngle = Mathf.Sqrt(moveHorizontal * moveHorizontal + moveVertical * moveVertical);

        rb.velocity = moveDirection * stickAngle * speed;
    }
}