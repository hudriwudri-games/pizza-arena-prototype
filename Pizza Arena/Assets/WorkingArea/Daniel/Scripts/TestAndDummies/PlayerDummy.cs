using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerDummy : MonoBehaviour, Damageable
{
    [SerializeField] GameObject enemy;
    [SerializeField] int damage;
    public int TakeDamage(int damageAmmount)
    {
        //print(gameObject.name + " took " + damageAmmount + " ammount of damage");
        return 10;
    }
    IEnumerator DamageEnemy()
    {
        while (enemy != null)
        {
            enemy.GetComponent<Damageable>().TakeDamage(damage);
            yield return new WaitForSeconds(1);
        }
    }

    private void Start()
    {
        StartCoroutine(DamageEnemy());
    }
}
