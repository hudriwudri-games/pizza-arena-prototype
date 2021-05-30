using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelManager : MonoBehaviour
{
    static private LevelManager instance;

    [SerializeField] private Timer timer;
    [SerializeField] private PlayerData[] playerData;
    [SerializeField] private ScoreScreenManager scoreScreen;

    [SerializeField] private GameObject spawners;

    private float roundTime = 60 * 5;
    //private float roundTime = 5;
    private bool running = true;

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
        if(running && timer.GetLeftTime() <= 0)
        {
            StopGame();
            scoreScreen.Show();
        }
    }

    public void GivePointsToPlayer(int playerId, int points)
    {
        playerData[playerId].AddPoints(points);
    }

    private void StopGame()
    {
        running = false;
        GameObject[] enemies = GameObject.FindGameObjectsWithTag("Enemy");
        foreach(GameObject enemy in enemies)
        {
            Destroy(enemy);
        }
        Destroy(spawners);
        //TODO: delete all monsters and spawner, delete character input
    }
}
