using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(Rigidbody))]
public class PlayerController : MonoBehaviour, Damageable
{
    public GameObject projectile;
    
    [SerializeField] float speed;
    // stats for melee attacks
    [SerializeField] float meleeAttackArea;
    [SerializeField] float meleeAttackRange;
    [SerializeField] int meeleAttackDamage;
    [SerializeField] GameObject aimPreview;
    [SerializeField] PlayerData data;

    Rigidbody rb;


    Vector2 movementInput;
    Vector2 lookInput;

    // helper variables for smoother turning
    float turnSmoothTime = 0.1f;
    float turnSmoothVelocity;

    bool isAiming = false;


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
            float targetAngle = Mathf.Atan2(lookDirection.x, lookDirection.z) * Mathf.Rad2Deg;
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

    /// <summary>
    /// Attack functions
    /// </summary>

    void ShortRangeAttack()
    {
        RaycastHit[] hits;
        hits = Physics.BoxCastAll(transform.position + transform.forward * meleeAttackRange, new Vector3(meleeAttackArea, meleeAttackArea, meleeAttackArea), transform.forward, Quaternion.identity, 0);
        foreach (RaycastHit hit in hits)
        {
            if (hit.collider.gameObject.CompareTag("Enemy"))
            {
                Damageable enemy = hit.collider.gameObject.GetComponent<Damageable>();
                if (enemy != null)
                {
                    enemy.TakeDamage(meeleAttackDamage);
                }
                else
                {
                    Debug.LogError(hit.collider.gameObject.name + " doesn't have a Damageable component, add one or remove the Enemy tag");
                }
            }
        }
    }

    //Draw the BoxCast as a gizmo to show where it currently is testing. Click the Gizmos button to see this
    void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireCube(transform.position + transform.forward * meleeAttackRange, new Vector3(meleeAttackArea, meleeAttackArea, meleeAttackArea));
    }

    void LongRangeAttack()
    {
        if (data.GetPizzaSliceAmount() > 0)
        {
            GameObject bullet = Instantiate(projectile, transform.position + transform.forward * meleeAttackRange, Quaternion.identity) as GameObject;
            bullet.GetComponent<Rigidbody>().AddForce(transform.forward * 500);
            data.RemovePizzaSlice(1);
        }
    }

    /// <summary>
    /// Health functions
    /// </summary>

    public void TakeDamage(int damageAmount)
    {
        data.RemoveHealth(damageAmount);
        if(data.GetHealth()<=0)
        {
            data.ResetPlayer();
            ResetPlayer();
        }
    }

    public void ResetPlayer()
    {
        // TODO: loose Items - done?
        rb.velocity = Vector3.zero;
        transform.position = Vector3.zero;
        transform.rotation = Quaternion.Euler(Vector3.zero);
    }

    public int GetHealth()
    {
        return data.GetHealth();
    }

    /// <summary>
    /// Input Functions
    /// </summary>
    /// 

    public void OnMove(InputAction.CallbackContext context)
    {
        movementInput = context.ReadValue<Vector2>();
    }

    public void OnAimRotate(InputAction.CallbackContext context)
    {
        lookInput = context.ReadValue<Vector2>();
    }

    public void OnAttack(InputAction.CallbackContext context)
    {
        if (context.started)
        {
            if (isAiming)
            {
                LongRangeAttack();
            }
            else
            {
                ShortRangeAttack();
            }  
        }
    }

    public void OnAim(InputAction.CallbackContext context)
    {
        if (context.started)
        {
            isAiming = true;
            aimPreview.SetActive(true);
        }

        if (context.canceled)
        {
            isAiming = false;
            aimPreview.SetActive(false);
        }
    }
}