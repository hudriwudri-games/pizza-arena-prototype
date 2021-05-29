using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(Rigidbody))]
public class PlayerController : MonoBehaviour, Damageable
{
    [SerializeField] int startingHealth;
    [SerializeField] float speed;
    [SerializeField] float attackAreaDimensions;

    Rigidbody rb;

    int healthPoints;

    Vector2 movementInput;
    Vector2 lookInput;


    // helper variables for smoother turning
    float turnSmoothTime = 0.1f;
    float turnSmoothVelocity;

    void Awake()
    {
        healthPoints = startingHealth;
    }

    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    void Update()
    {
        // get direction the player looks in
        Vector3 lookDirection = new Vector3(lookInput.x, 0f, lookInput.y).normalized;

        // if there is input change, apply smoothing and rotate player to desired angle
        if (lookDirection.magnitude >= 0.1f)
        {
            float targetAngle = Mathf.Atan2(-lookDirection.x, -lookDirection.z) * Mathf.Rad2Deg;
            float angle = Mathf.SmoothDampAngle(transform.eulerAngles.y, targetAngle, ref turnSmoothVelocity, turnSmoothTime);
            transform.rotation = Quaternion.Euler(0f, angle, 0f);
        }
    }

    void FixedUpdate()
    {
        // get direction player will move in
        Vector3 moveDirection = new Vector3(movementInput.x, 0f, movementInput.y).normalized;

        // calc the current angle of the stick
        // (to change speed depending on how much the stick is tilted)
        float stickAngle = Mathf.Sqrt(movementInput.x * movementInput.x + movementInput.y * movementInput.y);

        rb.velocity = moveDirection * stickAngle * speed;
    }

    public void TakeDamage(int damageAmmount)
    {
        healthPoints -= damageAmmount;
        if (healthPoints <= 0)
        {
            ResetPlayer();
        }
    }

    public int getHealth() 
    {
        return healthPoints;
    }

    public void ResetPlayer()
    {
        // TODO: loose Items
        rb.velocity = Vector3.zero;
        transform.position = Vector3.zero;
        transform.rotation = Quaternion.Euler(Vector3.zero);
    }

    /// <summary>
    /// Input Functions
    /// </summary>
    /// 

    public void OnMove(InputAction.CallbackContext context)
    {
        Debug.Log("move son");
        movementInput = context.ReadValue<Vector2>();
    }

    public void OnAimRotate(InputAction.CallbackContext context)
    {
        lookInput = context.ReadValue<Vector2>();
    }
}