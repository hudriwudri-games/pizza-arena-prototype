using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Customer : MonoBehaviour
{
    [SerializeField] private Timer timer;
    [SerializeField] private GameObject HUD;
    [SerializeField] private Text orderSizeText;
    private int currOrderPlayerId = -1;
    private int currOrderSize = 0;
    private float angryTime = 10.0f;
    private int points = 0;
    private int satisfiedPoints = 5;
    private int angryPoints = 3;
    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        if(timer.GetLeftTime()<=0 || currOrderSize <= 0)
        {
            HUD.SetActive(false);
            currOrderSize = 0;
        }
    }

    public void StartOrder(int playerId)
    {
        timer.StartTimer(30);
        currOrderPlayerId = playerId;
        HUD.SetActive(true);
        currOrderSize = Random.Range(1, 9);
        orderSizeText.text = currOrderSize.ToString();
    }

    public bool IsActive()
    {
        return timer.GetLeftTime() > 0;
    }

    public int GetOrderPlayerId()
    {
        return currOrderPlayerId;
    }
    private void OnCollisionEnter(Collision collision)
    {
        --currOrderSize;

        if (currOrderSize <= 0)
        {
            timer.StopTimer();
        }

        if (timer.GetLeftTime() < angryTime)
        {
            points += angryPoints;
        }
        else
        {
            points += satisfiedPoints;
        }
    }

    public int GetPoints(int playerId)
    {
        if(playerId == currOrderPlayerId)
        {
            int playerPoints = points;
            points = 0;
            return playerPoints;
        }
        return 0;
    }

}
