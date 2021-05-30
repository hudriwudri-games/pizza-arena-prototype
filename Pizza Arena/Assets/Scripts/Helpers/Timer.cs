using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Timer : MonoBehaviour
{
    [SerializeField]
    private Image timeBar = null;
    [SerializeField]
    private Text timeText = null;

    private float time = -1.0f;
    private float targetTime = -1.0f;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (time > 0.0f)
        {
            time -= Time.deltaTime;
        }

        if(timeBar!= null)
        {
            timeBar.fillAmount = (float)1 / targetTime * Mathf.Max(time, 0);
        }
        if (timeText != null)
        {
            int secondsTime = (int)time + 1; //ceiling number
            int minutes = secondsTime / 60;
            int seconds = secondsTime % 60;
            timeText.text = string.Format("{0:0}:{1:00}", minutes, seconds);
        }
    }

    public void StartTimer(float seconds)
    {
        time = seconds;
        targetTime = seconds;
    }
    public void StopTimer()
    {
        time = -1.0f;
        targetTime = -1.0f;
    }

    public float GetLeftTime()
    {
        return time;
    }

}
