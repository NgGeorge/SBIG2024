using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    public List<Customer> Customers { get; private set; }

    public InventoryManager Inventory{ get; private set; }

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            // Keeps the GameManager persistent across scenes.
            // This maybe needed in order to retain scoring info, etc.
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        } 
    }

    private void Initialize()
    {
        Customers = new List<Customer>();
        // TODO : Implement Important Stuff
        Inventory = new InventoryManager();
    }
    
    // TODO : Create level implementation? 

    private void GenerateCustomers(int numberOfCustomers)
    {
        for (int i = 0; i < numberOfCustomers; i++)
        {
            Customer customer = new Customer((i + 1).ToString());
            Customers.Add(customer);
        }
    }
}
