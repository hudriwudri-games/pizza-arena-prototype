using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Oven : MonoBehaviour
{

    [SerializeField]
    [Range(0, 3)]
    private int playerIndex;

    [SerializeField]
    private Timer timer;


    private float targetTime = 9.0f;
    private float burnTime = 1.0f;
    private float finishTime = 3.0f;

    bool activeBaking = false;


    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(timer.GetLeftTime()<=0)
        {
            activeBaking = false;
        }
    }

    public bool IsPlayer(int id)
    {
        return playerIndex == id;
    }

    public int StartBaking(int ingredients)
    {
        if(ingredients >= 7)
        {
            activeBaking = true;
            timer.StartTimer(targetTime);
            return 7;
        }
        return 0;
    }

    public bool IsActive()
    {
        return activeBaking;
    }

    public int TakePizza()
    {
        if(timer.GetLeftTime() <= finishTime && timer.GetLeftTime() > burnTime && activeBaking)
        {
            StopTimer();
            //take 8 pizza slices (category 1)
            return 1;
        }
        if (timer.GetLeftTime() <= burnTime && activeBaking)
        {
            StopTimer();
            //take 8 burnt pizza slices (category 2)
            return 2;
        }
        //get no pizza (category 0)
        return 0;
    }

    void StopTimer()
    {
        activeBaking = false;
        timer.StopTimer();
    }
}
