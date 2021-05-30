using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelManager : MonoBehaviour
{
    static private LevelManager instance;

    [SerializeField] private Timer timer;
    [SerializeField] private PlayerData[] playerData;
    [SerializeField] private ScoreScreenManager scoreScreen;

    //private float roundTime = 60 * 5;
    private float roundTime = 5;

    public static LevelManager GetLevelManager()
    {
        return instance;
    }

    private void Awake()
    {
        // we need this class to only exist once, destroy any copies
        if (instance != null && instance != this)
        {
            Debug.LogWarning("destroying copy of LevelManager");
            Destroy(this.gameObject);
        }
        else if (instance == null)
        {
            instance = this;
        }
    }
    void Start()
    {
        timer.StartTimer(roundTime);
    }

    // Update is called once per frame
    void Update()
    {
        if(timer.GetLeftTime() <= 0)
        {
            scoreScreen.Show();
        }
    }

    public void GivePointsToPlayer(int playerId, int points)
    {
        playerData[playerId].AddPoints(points);
    }
}
