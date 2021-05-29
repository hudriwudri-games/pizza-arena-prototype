using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerDummy : MonoBehaviour, Damageable
{
    public void TakeDamage(int damageAmmount)
    {
        print(gameObject.name + " took " + damageAmmount + " ammount of damage");
    }
}
