using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelManager : MonoBehaviour
{
    [SerializeField] private Timer timer;

    private float roundTime = 60 * 5;
    // Start is called before the first frame update
    void Start()
    {
        timer.StartTimer(roundTime);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
