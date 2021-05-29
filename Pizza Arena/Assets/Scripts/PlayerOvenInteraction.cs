using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerOvenInteraction : MonoBehaviour
{
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
                ingredient -= activeOven.StartBaking(ingredient);
            }
        }
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "oven" || other.gameObject.name == "Oven (1)" || other.gameObject.name == "Oven")
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
        if (other.tag == "oven" || other.gameObject.name == "Oven (1)" || other.gameObject.name == "Oven")
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
