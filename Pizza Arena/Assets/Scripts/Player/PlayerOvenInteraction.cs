using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class PlayerOvenInteraction : MonoBehaviour
{
    [SerializeField] private PlayerData data;

    private int id = -1;
    private Oven activeOven = null;
    private bool isEnter;

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
                    data.AddPizzaSlice(8);
                };
            }
            else
            {
                int lostIngredients = activeOven.StartBaking(data.GetIngredientsAmount());
                data.RemoveIngredients(lostIngredients);
            }
        }

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
