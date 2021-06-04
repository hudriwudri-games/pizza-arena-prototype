using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DummyEnemy : MonoBehaviour, Damageable
{
    int hp = 5;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public int TakeDamage(int damageAmmount)
    {
        hp -= damageAmmount;
        //Debug.Log(hp);
        if (hp <= 0)
        {
            Despawn();
        }
        return 10;
    }

    public void Despawn()
    {
        Destroy(gameObject);
    }
}
