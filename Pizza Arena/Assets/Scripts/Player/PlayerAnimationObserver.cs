using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAnimationObserver : PlayerObserver
{
    [SerializeField] Renderer renderer;
    // JUST FOR TESTING
    [SerializeField] Material defaultMat, attackMat, damageMat;

    public override void Notify(PlayerController sender)
    {
        switch (sender.GetState())
        {
            case PlayerController.State.DEFAULT:
                renderer.material = defaultMat;
                break;
            case PlayerController.State.MELEEATTACK:
                renderer.material = attackMat;
                break;
            case PlayerController.State.DAMAGED:
                renderer.material = damageMat;
                break;
        }
    }
}
