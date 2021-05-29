using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerDummy : MonoBehaviour, Damageable
{
    [SerializeField] GameObject enemy;
    [SerializeField] bool attackEnemy;
    public void TakeDamage(int damageAmmount)
    {
        //print(gameObject.name + " took " + damageAmmount + " ammount of damage");
    }
    IEnumerator DamageEnemy()
    {
        while (enemy != null)
        {
            if(attackEnemy)
                enemy.GetComponent<Damageable>().TakeDamage(10);
            yield return new WaitForSeconds(1);
        }
    }

    private void Start()
    {
        StartCoroutine(DamageEnemy());
    }
}
