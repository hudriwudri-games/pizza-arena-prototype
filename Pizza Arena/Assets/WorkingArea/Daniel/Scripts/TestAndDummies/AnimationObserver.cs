using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimationObserver : Observer
{
    [SerializeField] Renderer renderer;
    // JUST FOR TESTING
    [SerializeField] Material walkMat, attackMat, idleMat, dyingMat;

    public override void Notify(Enemy sender)
    {
        switch (sender.GetState())
        {
            case Enemy.State.ATTACKINGMELEE:
                renderer.material = attackMat;
                break;
            case Enemy.State.WALKINGTOWARDSPLAYER:
                renderer.material = walkMat;
                break;
            case Enemy.State.IDLE:
                renderer.material = idleMat;
                break;
            case Enemy.State.DYING:
                renderer.material = dyingMat;
                break;
        }
    }
}
