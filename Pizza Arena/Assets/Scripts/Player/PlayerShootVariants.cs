using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerShootVariants : MonoBehaviour
{
    float length;
    double startPress;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void OnAimRotate(InputAction.CallbackContext context)
    {
        length = StickLength(context);
        Debug.Log("length: " + length);
    }
    public void OnAttack(InputAction.CallbackContext context)
    {
       
        length = (float)(context.startTime - startPress)/2;
        if (context.ReadValue<float>() < 0.01f)
        {
            startPress = context.time;
        }
        Debug.Log("attack: " + (context.startTime- startPress ) + "length: " + length);
    }
    private float SameLength()
    {
        return 1;
    }

    private float StickLength(InputAction.CallbackContext context)
    {
        return context.ReadValue<Vector2>().magnitude;
    }
}
