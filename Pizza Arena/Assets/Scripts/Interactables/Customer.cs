using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Customer : MonoBehaviour
{
    [SerializeField] private Timer timer;
    [SerializeField] private GameObject HUD;
    [SerializeField] private Text orderSizeText;
    [SerializeField] private Image orderImage;
    private int currOrderPlayerId = -1;
    private int currOrderSize = 0;
    private float angryTime = 10.0f;
    private int happyPoints = 5;
    private int hangryPoints = 3;
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
        switch(currOrderPlayerId)
        {
            case 0:
                orderImage.color = Color.red;
                break;
            case 1:
                orderImage.color = Color.blue;
                break;
            case 2:
                orderImage.color = Color.green;
                break;
            case 3:
                orderImage.color = Color.yellow;
                break;
        }
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
        Debug.Log("xxx" + IsActive() + " " + collision.gameObject.GetComponent<Projectile>().GetPlayerId() + " " + currOrderPlayerId);
        if (IsActive() && collision.gameObject.GetComponent<Projectile>().GetPlayerId() == currOrderPlayerId)
        {
            --currOrderSize;
            orderSizeText.text = currOrderSize.ToString();

            if (currOrderSize <= 0)
            {
                timer.StopTimer();
            }

            if (timer.GetLeftTime() < angryTime)
            {
                LevelManager.GetLevelManager().GivePointsToPlayer(currOrderPlayerId, hangryPoints);
            }
            else
            {
                LevelManager.GetLevelManager().GivePointsToPlayer(currOrderPlayerId, happyPoints);
            }
        }
    }
}
