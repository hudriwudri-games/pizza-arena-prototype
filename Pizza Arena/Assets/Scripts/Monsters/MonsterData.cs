using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MonsterData : MonoBehaviour
{

    [Header("HUD")]
    [SerializeField] Image healthBar;

    [Header("Debugging")]
    [SerializeField] [Range(0, 100)] private int health = 10;
    [SerializeField] [Range(0, 100)] private int startHealth = 10;

    [SerializeField] bool continuallyUpdateHUD = false;

    private void Start()
    {
        UpdateHUD();
    }
    private void Update()
    {
        if(continuallyUpdateHUD)
        {
            UpdateHUD();
        }
    }
    public void RemoveHealth(int value)
    {
        health -= value;
        health = Mathf.Max(health, 0);
        healthBar.fillAmount = (float)1 / startHealth * health;
    }
    public int GetHealth()
    {
        return health;
    }
    private void UpdateHUD()
    {
        healthBar.fillAmount = (float)1 / startHealth * health;
    }
}
