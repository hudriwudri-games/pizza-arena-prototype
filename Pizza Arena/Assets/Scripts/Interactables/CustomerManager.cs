using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CustomerManager : MonoBehaviour
{

    [SerializeField] private GameObject customers;

    private Customer[] customerList;
    private int[] freeCustomers;
    private int freeCustomersAmount = 0;
    private float[] openOrdersPerPlayer;
    private const int maxPlayerCount = 4;
    private int currPlayerCount = 2;

    void Start()
    {
        customerList = new Customer[customers.transform.childCount];
        freeCustomers = new int[customerList.Length];
        openOrdersPerPlayer = new float[maxPlayerCount];

        int i = 0;
        foreach(Transform customer in customers.transform)
        {
            customerList[i] = customer.gameObject.GetComponent<Customer>();
            ++i;
        }

        InvokeRepeating("UpdateOrders", 2.0f, 5.0f);
    }

    void Update()
    {
    }

    private void UpdateOrders()
    {

        for (int i = 0; i < currPlayerCount; ++i)
        {
            openOrdersPerPlayer[i] = 0;
        }
        
        // find who needs new orders
        for (int i = 0; i < customerList.Length; ++i)
        {
            if (customerList[i].IsActive())
            {
                ++openOrdersPerPlayer[customerList[i].GetOrderPlayerId()];
            }
        }
        // start new needed orders
        for (int i = 0; i < currPlayerCount; ++i)
        {
            FindFreeCustomers();
            if(freeCustomersAmount > 0)
            {
                int currCustomerId = freeCustomers[Random.Range(0, freeCustomersAmount)];
                Customer currCustomer = customerList[currCustomerId];
                if (openOrdersPerPlayer[i] <= 0)
                {
                    ++openOrdersPerPlayer[i];
                    currCustomer.StartOrder(i);
                }
                else if (Random.Range(0, 100) < 10)
                {
                    ++openOrdersPerPlayer[i];
                    currCustomer.StartOrder(i);
                }
            }
        }
    }

    private void FindFreeCustomers()
    {
        for (int i = 0; i < customerList.Length; ++i)
        {
            freeCustomers[i] = -1;
        }
        freeCustomersAmount = 0;

        for (int i = 0; i < customerList.Length; ++i)
        {
            if (!customerList[i].IsActive())
            {
                freeCustomers[freeCustomersAmount] = i;
                ++freeCustomersAmount;
            }
        }
    }
}
