using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.InputSystem;

public class PlayerData : MonoBehaviour
{
    [Header("HUD")]
    [SerializeField] Text pointsText;
    [SerializeField] Text slicesText;
    [SerializeField] Text ingredientsText;
    [SerializeField] Image healthBar;

    [Header("Debugging")]
    [SerializeField] [Range(0, 100)] private int health = 100;
    [SerializeField] [Range(0, 100)] private int maxHealth = 100;
    [SerializeField] [Range(0, 100)] private int slices = 15;
    [SerializeField] [Range(0, 100)] private int maxSlices = 99;
    [SerializeField] [Range(0, 100)] private int ingredients = 15;
    [SerializeField] [Range(0, 100)] private int maxIngredients = 99;
    [SerializeField] [Range(0, 100)] private int points = 0;

    [SerializeField] bool continuallyUpdateHUD = false;

    private int playerId;

    // Start is called before the first frame update
    void Start()
    {
        playerId = gameObject.GetComponent<PlayerInput>().playerIndex;
        UpdateHUD();
    }

    private void Update()
    {
        if(continuallyUpdateHUD)
        {
            UpdateHUD();
        }
    }

    public void AddHealth(int value)
    {
        health += value;
        health = Mathf.Min(health, maxHealth);
        healthBar.fillAmount = (float)1 / maxHealth * health;
    }

    public void RemoveHealth(int value)
    {
        health -= value;
        health = Mathf.Max(health, 0);
        healthBar.fillAmount = (float)1 / maxHealth * health;
    }
    public int GetHealth()
    {
        return health;
    }

    public void AddPizzaSlice(int amount)
    {
        slices += amount;
        slices = Mathf.Min(slices, maxSlices);
        slicesText.text = slices.ToString();
    }
    public void RemovePizzaSlice(int amount)
    {
        slices -= amount;
        slices = Mathf.Max(slices, 0);
        slicesText.text = slices.ToString();
    }
    public int GetPizzaSliceAmount()
    {
        return slices;
    }
    public void AddIngredients(int amount)
    {
        ingredients += amount;
        ingredients = Mathf.Min(ingredients, maxIngredients);
        ingredientsText.text = ingredients.ToString();
    }
    public void RemoveIngredients(int amount)
    {
        ingredients -= amount;
        ingredients = Mathf.Max(ingredients, 0);
        ingredientsText.text = ingredients.ToString();
    }
    public int GetIngredientsAmount()
    {
        return ingredients;
    }
    public void AddPoints(int amount)
    {
        points += amount;
        pointsText.text = points.ToString();
    }
    public int GetPoints()
    {
        return points;
    }
    public void ResetPlayer()
    {
        health = 100;
        slices = 0;
        ingredients = 0;
        UpdateHUD();
    }

    private void UpdateHUD()
    {
        healthBar.fillAmount = (float)1 / maxHealth * health;
        Debug.Log(health);
        slicesText.text = slices.ToString();
        ingredientsText.text = ingredients.ToString();
        pointsText.text = points.ToString();
    }

    public int GetPlayerId()
    {
        return playerId;
    }
}
