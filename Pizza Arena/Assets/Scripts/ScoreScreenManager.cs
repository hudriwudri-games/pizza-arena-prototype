using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ScoreScreenManager : MonoBehaviour
{
    [SerializeField] private GameObject scoreScreen;
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
