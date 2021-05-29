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
    private Image timer;


    private float targetTime = 9.0f;
    private float burnTime = 1.0f;
    private float finishTime = 3.0f;

    private float currTargetTime = -1.0f;
    bool activeBaking = false;


    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(currTargetTime > 0.0f)
        {
            currTargetTime -= Time.deltaTime;
        }

       
        timer.fillAmount = 1 / targetTime * Mathf.Max(currTargetTime, 0);
    }

    public bool IsPlayer(int id)
    {
        return playerIndex == id;
    }

    public int StartBaking(int ingredients)
    {
        if(ingredients >= 7)
        {
            Debug.Log("Bake");
            activeBaking = true;
            currTargetTime = targetTime;
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
        if (currTargetTime <= finishTime && currTargetTime > burnTime && activeBaking)
        {
            StopTimer();
            //take 8 pizza slices
            return 1;
        }
        if (currTargetTime <= burnTime && activeBaking)
        {
            StopTimer();
            //take 8 burnt pizza slices
            return 2;
        }
        //get no pizza
        return 0;
    }

    void StopTimer()
    {
        activeBaking = false;
        currTargetTime = -1.0f;
    }
}
