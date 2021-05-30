using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemHandler : MonoBehaviour
{
    [SerializeField] PlayerData data;
    [SerializeField] int ingredientCount;

    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Item"))
        {
            data.AddIngredients(ingredientCount);
            Destroy(other.gameObject);
        }
    }
}
