using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    [SerializeField] int attackDamage;
    [SerializeField] float lifeTime;
    private int playerId;

    float timer;

    void Awake()
    {
        timer = 0f;
    }

    // Update is called once per frame
    void Update()
    {
        timer += Time.deltaTime;

        if (timer >= lifeTime)
        {
            //Debug.Log("Dead");
            Destroy(gameObject);
        }
    }

    void OnCollisionEnter(Collision other)
    {
        if (other.gameObject.CompareTag("Enemy"))
        {
            Damageable enemy = other.gameObject.GetComponent<Damageable>();
            if (enemy != null)
            {
                enemy.TakeDamage(attackDamage);
            }
            else
            {
                Debug.LogError(other.gameObject.name + " doesn't have a Damageable component, add one or remove the Enemy tag");
            }
        }
        Destroy(gameObject);
    }

    public void SetPlayerId(int id)
    {
        playerId = id;
    }

    public int GetPlayerId()
    {
        return playerId;
    }
}
