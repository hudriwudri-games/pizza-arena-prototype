using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(Rigidbody))]
public class PlayerController : MonoBehaviour, Damageable
{
    public GameObject projectile;
    
    [SerializeField] float speed;
    [SerializeField] float blockSpeedMultiplier = 0.1f;
    // stats for melee attacks
    [SerializeField] float meleeAttackArea;
    [SerializeField] float meleeAttackRange;
    [SerializeField] int meeleAttackDamage;
    [SerializeField] float meleeAttackDuration;
    [SerializeField] float meleeAttackCooldown;
    [SerializeField] float invincibilityCooldown;
    [SerializeField] float aimAngle = 45;
    [SerializeField] PlayerData data;

    Rigidbody rb;
    DrawTrajectory aimPreview;
    Vector3 lookDirection;

    Vector2 movementInput;
    Vector2 lookInput;

    float shootLength;

    // helper variables for smoother turning
    float turnSmoothTime = 0.1f;
    float turnSmoothVelocity;

    bool canAttack = true;
    bool invincible = false;
    bool isAiming = false;
    bool isBlocking = false;
    bool canBlock = true;

    // just for testing 
    [Header("Testing")]
    public bool holdToBlock = true;
    public bool toggleDashOnOff = true;
    public bool toggleBlockOnOff = true;

    // updating "animations" (colors)
    List<PlayerObserver> observers;
    State thisState = State.DEFAULT;

    public enum State
    {
        DEFAULT,
        MELEEATTACK,
        LONGRANGEATTACK,
        COOLDOWN,
        DAMAGED,
        BLOCKING
    }

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        observers = new List<PlayerObserver>(GetComponents<PlayerObserver>());
        aimPreview = GetComponent<DrawTrajectory>();
    }

    void Update()
    {
        // get direction the player looks in
        lookDirection = new Vector3(lookInput.x, 0f, lookInput.y).normalized;

        // if there is input change, apply smoothing and rotate player to desired angle
        if (lookDirection.magnitude >= 0.1f)
        {
            float targetAngle = Mathf.Atan2(lookDirection.x, lookDirection.z) * Mathf.Rad2Deg;
            float angle = Mathf.SmoothDampAngle(transform.eulerAngles.y, targetAngle, ref turnSmoothVelocity, turnSmoothTime);
            transform.rotation = Quaternion.Euler(0f, angle, 0f);
        }

        if (isAiming)
        {
            aimPreview.DrawLineTrajectory((Quaternion.AngleAxis(-aimAngle, transform.right) * transform.forward) * CalcShootingForce(shootLength), transform.position + transform.forward * meleeAttackRange);
        }
    }

    void FixedUpdate()
    {
        // get direction player will move in
        Vector3 moveDirection = new Vector3(movementInput.x, 0f, movementInput.y).normalized;

        // calc the current angle of the stick
        // (to change speed depending on how much the stick is tilted)
        float stickAngle = Mathf.Sqrt(movementInput.x * movementInput.x + movementInput.y * movementInput.y);

        //rb.velocity = moveDirection * stickAngle * speed;
        if (isBlocking && holdToBlock)
        {
            rb.AddForce(speed * stickAngle * moveDirection * blockSpeedMultiplier);
        }
        else 
        {
            rb.AddForce(speed * stickAngle * moveDirection);
        }  
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
            if (hit.collider.gameObject.CompareTag("Player"))
            {
                if (hit.collider.gameObject != gameObject)
                {
                    if (!hit.collider.gameObject.GetComponent<PlayerController>().isBlocking)
                    {
                        Rigidbody otherPlayer = hit.collider.gameObject.GetComponent<Rigidbody>();
                        Vector3 pushDirection = transform.forward;
                        otherPlayer.AddForce(pushDirection * data.GetPushForce(), ForceMode.Impulse);
                    }
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

    IEnumerator MeleeAttackRoutine()
    {
        while (true)
        {
            NotifyObservers(State.MELEEATTACK);
            ShortRangeAttack();
            yield return new WaitForSeconds(meleeAttackDuration);
            NotifyObservers(State.COOLDOWN);
            yield return new WaitForSeconds(meleeAttackCooldown);
            NotifyObservers(State.DEFAULT);
            canAttack = true;
            yield break;
        }
    }

    void StartProjectile(float shotForce) 
    {
        if (data.GetPizzaSliceAmount() > 0)
        {
            NotifyObservers(State.LONGRANGEATTACK);
            // Spawn Bullet
            GameObject bullet = Instantiate(projectile, transform.position + transform.forward * meleeAttackRange, Quaternion.identity) as GameObject;
            // Apply Force at given angle
            bullet.GetComponent<Rigidbody>().AddForce((Quaternion.AngleAxis(-aimAngle, transform.right) * transform.forward) * CalcShootingForce(shotForce));
            // give bullet id of player (needed for point calc)
            bullet.GetComponent<Projectile>().SetPlayerId(data.GetPlayerId());
            data.RemovePizzaSlice(1);
            NotifyObservers(State.DEFAULT);
        }
    }

    private float CalcShootingForce(float shotForce) 
    {
        float range = Mathf.Lerp(data.GetMinShootingForce(), data.GetMaxShootingForce(), shotForce);
        return range;
    }

    private void Dodge()
    {
        Vector3 moveDirection = new Vector3(movementInput.x, 0f, movementInput.y).normalized;
        rb.AddForce(moveDirection * data.GetDodgeForce(), ForceMode.Impulse);
    }

    public bool IsBlocking()
    {
        return isBlocking;
    }

    IEnumerator BlockFast()
    {
        canBlock = false;
        isBlocking = true;
        NotifyObservers(State.BLOCKING);
        yield return new WaitForSeconds(1);
        isBlocking = false;
        NotifyObservers(State.DEFAULT);
        yield return new WaitForSeconds(1);
        canBlock = true;
    }

    /// <summary>
    /// Health functions
    /// </summary>

    IEnumerator TakeDamageRoutine(int damageAmount)
    {
        while (true)
        {
            NotifyObservers(State.DAMAGED);
            data.RemoveHealth(damageAmount);
            if (data.GetHealth() <= 0)
            {
                ResetPlayer();
                yield break;
            }
            yield return new WaitForSeconds(invincibilityCooldown);
            NotifyObservers(State.DEFAULT);
            invincible = false;
            yield break;
        }
    }

    public int TakeDamage(int damageAmount)
    {
        int newHealth = data.GetHealth() - damageAmount;
        if (invincible == false && isBlocking == false)
        {
            invincible = true;
            StartCoroutine(TakeDamageRoutine(damageAmount));
        }
        return newHealth;
    }

    public void ResetPlayer()
    {
        // TODO: loose Items - done?
        NotifyObservers(State.DEFAULT);
        rb.velocity = Vector3.zero;
        transform.position = new Vector3 (0.0f, 1.0f, 0.0f);
        transform.rotation = Quaternion.Euler(Vector3.zero);
        invincible = false;
        data.ResetPlayer();
    }

    /// <summary>
    /// Update State
    /// </summary>

    protected void NotifyObservers(State newState)
    {
        thisState = newState;
        foreach (PlayerObserver thisObserver in observers)
        {
            thisObserver.Notify(this);
        }
    }

    public State GetState()
    {
        return thisState;
    }

    #region inputControl
    /// <summary>
    /// Input Functions
    /// </summary>

    public void OnMove(InputAction.CallbackContext context)
    {
        movementInput = context.ReadValue<Vector2>();
    }

    public void OnAimRotate(InputAction.CallbackContext context)
    {
        lookInput = context.ReadValue<Vector2>();
        shootLength = context.ReadValue<Vector2>().magnitude;
    }

    public void OnAttack(InputAction.CallbackContext context)
    {
        if (context.started)
        {
            if (!isAiming && canAttack && !isBlocking)
            {
                canAttack = false;
                StartCoroutine(MeleeAttackRoutine());
            }
            else if (isAiming)
            {
                StartProjectile(shootLength);
            }  
        }
    }

    public void OnDodge(InputAction.CallbackContext context)
    {
        if (context.started && toggleDashOnOff)
        {
            Dodge();
        }
    }

    public void OnBlock(InputAction.CallbackContext context)
    {
        if (toggleBlockOnOff)
        {
            if (holdToBlock)
            {
                if (context.started)
                {
                    isBlocking = true;
                    NotifyObservers(State.BLOCKING);
                }
                if (context.canceled)
                {
                    isBlocking = false;
                    NotifyObservers(State.DEFAULT);
                }
            }
            else
            {
                if (canBlock)
                {
                    StartCoroutine(BlockFast());
                }
            }
        }
    }

    public void OnAim(InputAction.CallbackContext context)
    {
        if (context.started)
        {
            isAiming = true;
        }

        if (context.canceled)
        {
            isAiming = false;
            aimPreview.HideLine();
        }
    }
#endregion
}