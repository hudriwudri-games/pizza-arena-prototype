using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class PlayerOvenInteraction : MonoBehaviour
{
    [SerializeField]
    private Text pizzaText;
    [SerializeField]
    private Text ingredientText;
    private int id = -1;
    private Oven activeOven = null;
    private bool isEnter;
    private int ingredient = 15;
    private int slices = 0;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(id == -1)
        {
            id = this.GetComponent<PlayerInput>().playerIndex;
        }

        if(activeOven != null && isEnter)
        {
            isEnter = false;
            if(activeOven.IsActive())
            {
               if(activeOven.TakePizza() == 1)
                {
                    slices += 8;
                    Debug.Log("slices " + slices);
                };
            }
            else
            {
                Debug.Log("START BAKING");
                ingredient -= activeOven.StartBaking(ingredient);
            }
        }
        pizzaText.text = slices.ToString();
        ingredientText.text = ingredient.ToString();

    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Oven"))
        {
            Oven oven = other.GetComponentInParent<Oven>();
            if (oven.IsPlayer(id))
            {
                activeOven = oven;
            }
        }
    }
    private void OnTriggerExit(Collider other)
    {
        Debug.Log(other.gameObject.name);
        if (other.gameObject.CompareTag("Oven"))
        {
                activeOven = null;            
        }
    }

    public void OnConfirm(InputAction.CallbackContext context)
    {
        if (context.started)
        {
            isEnter = true;
        }
    }
}
