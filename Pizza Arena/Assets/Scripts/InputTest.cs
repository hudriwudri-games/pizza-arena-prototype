using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class InputTest : MonoBehaviour
{
    public void OnMove(InputAction.CallbackContext context)
    {
            Debug.Log(this.gameObject.name + ": Move " + context.ReadValue<Vector2>());
    }
    public void OnAttack(InputAction.CallbackContext context)
    {
        if(context.started)
        {
            Debug.Log(this.gameObject.name + ": Attack");
        }        
    }
    public void OnAim(InputAction.CallbackContext context)
    {
        if (context.started)
        {
            Debug.Log(this.gameObject.name + ": AimState");
        }
        if (context.canceled)
        {
            Debug.Log(this.gameObject.name + ": AimState ended");
        }
    }
    public void OnAimRotate(InputAction.CallbackContext context)
    {
            Debug.Log(this.gameObject.name + ": AimRotate " + context.ReadValue<Vector2>());

    }
    public void OnConfirm(InputAction.CallbackContext context)
    {
        if (context.started)
        {
            Debug.Log(this.gameObject.name + ": Confirm");
        }
    }
    public void OnCancel(InputAction.CallbackContext context)
    {
        if (context.started)
        {
            Debug.Log(this.gameObject.name + ": Cancel");
        }
    }

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
      /*  if(Input.GetButtonDown("Attack"))
        {
            Debug.Log("Attack");
        }
        if (Input.GetButtonDown("Submit"))
        {
            Debug.Log("Submit");
        }
        if (Input.GetButtonDown("Cancel"))
        {
            Debug.Log("Cancel");
        }
        if (Input.GetButtonDown("Aim"))
        {
            Debug.Log("Aim");
        }
        if (Input.GetAxis("Aim") > 0.1f || Input.GetAxis("Aim") < -0.1f)
        {
            Debug.Log("Aim" + Input.GetAxis("Aim"));
        }
        if (Input.GetButtonDown("Shoot"))
        {
            Debug.Log("Shoot");
        }
        if (Input.GetAxis("Shoot") > 0.1f || Input.GetAxis("Shoot") < -0.1f)
        {
            Debug.Log("Shoot" + Input.GetAxis("Shoot"));
        }
        if (Input.GetAxis("Horizontal")>0.1f || Input.GetAxis("Horizontal") < -0.1f)
        {
            Debug.Log("Horizontal" + Input.GetAxis("Horizontal"));
        }
        if (Input.GetAxis("Vertical") > 0.1f || Input.GetAxis("Vertical") < -0.1f)
        {
            Debug.Log("Vertical" + Input.GetAxis("Vertical"));
        }
        if (Input.GetAxis("AimHorizontal") > 0.1f || Input.GetAxis("AimHorizontal") < -0.1f)
        {
            Debug.Log("AimHorizontal" + Input.GetAxis("AimHorizontal"));
        }
        if (Input.GetAxis("AimVertical") > 0.1f || Input.GetAxis("AimVertical") < -0.1f)
        {
            Debug.Log("AimVertical" + Input.GetAxis("AimVertical"));
        }*/
    }
}
