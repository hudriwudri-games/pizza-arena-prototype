using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Enemy : MonoBehaviour
{
    List<Observer> observers;
    State thisState = State.IDLE;

    protected void Start()
    {
        observers = new List<Observer>(GetComponents<Observer>());
        //print(observers.Count);
    }
    public enum State
    {
        IDLE,
        WALKINGTOWARDSPLAYER,
        ATTACKINGMELEE,
        DYING
    }

    protected void NotifyObservers(State newState)
    {
        thisState = newState;
        foreach(Observer thisObserver in observers)
        {
            thisObserver.Notify(this);
        }
    }

    public State GetState()
    {
        return thisState;
    }
}
