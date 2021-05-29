using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class PlayerController : MonoBehaviour, Damageable
{
    [SerializeField] int startingHealth;
    [SerializeField] float speed;
    [SerializeField] float attackAreaDimensions;

    Rigidbody rb;

    int healthPoints;

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
        // calc direction the player looks in
        float lookHorizontal = Input.GetAxis("AimHorizontal");
        float lookVertical = Input.GetAxis("AimVertical");
        Vector3 lookDirection = new Vector3(lookHorizontal, 0f, lookVertical).normalized;

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
        Vector3 moveDirection = new Vector3(moveHorizontal, 0f, moveVertical).normalized;

        // calc the current angle of the stick
        // (to change speed depending on how much the stick is tilted)
        float stickAngle = Mathf.Sqrt(moveHorizontal * moveHorizontal + moveVertical * moveVertical);

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
}