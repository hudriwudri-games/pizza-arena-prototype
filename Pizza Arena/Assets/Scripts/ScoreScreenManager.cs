using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;

public class ScoreScreenManager : MonoBehaviour
{
    [SerializeField] private GameObject scoreScreen;
    [SerializeField] private PlayerData[] playerData;
    [SerializeField] private TMP_Text scoreText1;
    [SerializeField] private TMP_Text scoreText2;
    [SerializeField] private GameObject winner1;
    [SerializeField] private GameObject winner2;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Show()
    {
        scoreText1.text = playerData[0].GetPoints().ToString();
        scoreText2.text = playerData[1].GetPoints().ToString();
        if(playerData[0].GetPoints()>playerData[1].GetPoints())
        {
            winner1.SetActive(true);
        }
        else if (playerData[0].GetPoints() > playerData[1].GetPoints())
        {
            winner2.SetActive(true);
        }
        else
        {
            winner1.SetActive(true);
            winner2.SetActive(true);
        }
        scoreScreen.SetActive(true);
    }

    public void Hide()
    {
        scoreScreen.SetActive(false);
    }
    public void Exit()
    {
        Application.Quit();
    }
    public void Restart()
    {
        Hide();
        SceneManager.LoadScene("PizzaArena");
    }
}
