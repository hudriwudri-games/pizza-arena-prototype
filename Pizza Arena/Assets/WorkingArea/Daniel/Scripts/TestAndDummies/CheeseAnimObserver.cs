using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheeseAnimObserver : Observer
{
    [SerializeField] Renderer renderer;
    // JUST FOR TESTING
    [SerializeField] Material walkMat, attackMat, idleMat, dyingMat, searchMat;

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
            case Enemy.State.SEARCHINGPLAYER:
                renderer.material = searchMat;
                break;
        }
    }
}
