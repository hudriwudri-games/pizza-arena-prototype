using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemHandler : MonoBehaviour
{
    [SerializeField] PlayerData data;
    [SerializeField] int ingredientCount;

    void OnCollisionEnter(Collision other)
    {
        if (other.gameObject.CompareTag("Pickup"))
        {
            data.AddIngredients(ingredientCount);
            Destroy(other.gameObject);
        }
    }
}
