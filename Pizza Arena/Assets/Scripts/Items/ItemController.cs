using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class ItemController : MonoBehaviour
{
    [SerializeField] float impulseMagnitude;
    [SerializeField] IngredientType type;
    Rigidbody rb;

    [System.Serializable]
    public enum IngredientType
    {
        DOUGH
    }

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
        Vector3 direction = new Vector3(Random.Range(0.0f, 1.0f), 1.0f, Random.Range(0.0f, 1.0f)).normalized;
        rb.AddForce(direction * impulseMagnitude, ForceMode.Impulse);
    }

    public IngredientType GetIngredientType()
    {
        return type;
    }
}
