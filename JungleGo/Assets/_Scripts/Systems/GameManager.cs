using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    public List<Customer> Customers { get; private set; }

    public InventoryManager Inventory{ get; private set; }

    internal List<Level> Levels { get; private set; }

    private int _currentLevelIndex = 0;

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

        Levels = new List<Level> 
        {
            new()
            {
                CustomerList = new string[] {
                    "Frodo Baggins",
                    "Bilbo Baggins",
                    "Gandalf",
                    "Samwise Gamgee",
                    "Merry (Meriadoc Brandybuck)",
                    "Pippin (Peregrin Took)"
                    },
                ProductIds = new List<int> { 1, 2, 3, 4, 5 },
                MaxProductCount = 10,
                DelayBetweenCustomerSec = 2,
            }
        };
    }

    public void Start()
    {
        Initialize();
        var level = Levels[_currentLevelIndex];

        Inventory.RegenerateStock(level.ProductIds, level.MaxProductCount);
        GenerateCustomers(level.CustomerList);

        StartCoroutine(CustomerStartLoop(level));
    }

    IEnumerator CustomerStartLoop(Level level)
    {
        var currentCustomerIndex = 0;
        while (true && !IsAllCustomersHaveFinished())
        {
            yield return new WaitForSeconds(level.DelayBetweenCustomerSec);

            if (currentCustomerIndex < Customers.Count)
            {
                Debug.Log($"Dispatching Customer {Customers[currentCustomerIndex].Name}");
                Customers[currentCustomerIndex].TravelToNextShelf(Inventory);
                currentCustomerIndex++;
            }
        }

        Debug.Log("Finished start routine");
    }

    private bool IsAllCustomersHaveFinished()
    {
        var isComplete = true;

        foreach (var customer in Customers)
        {
            isComplete = isComplete && customer.IsShoppingComplete;
        }

        return isComplete;
    }

    private void GenerateCustomers(string[] customersList)
    {
        for (int i = 0; i < customersList.Length ; i++)
        {
            Customer customer = new Customer((int)i + 1, customersList[i], Inventory);
            Customers.Add(customer);
        }
    }

    /// <summary>
    /// This function should be called in every update
    /// This is the funciton for moving all customers in every update. 
    /// Each customer will run in the order of their initilization.
    /// </summary>
    public void MoveAllCustomers()
    {
        // Print current board to the console.
        BoardManager.Instance.PrintBoard();

        // Move each customer 
        Customers.ForEach(customer => 
        {
            var path = PathFinder.Instance.AStarPathfind(customer.Position, customer.GetNextProductInList().Position);
            
            if (path != null && path.Count > 1)
            {
                customer.Move(path[1].Item1, path[1].Item2);
            }

            if (customer.Position == customer.GetNextProductInList().Position)
            {
                Debug.Log("Customer #" + customer.Id + " achived to the position!\n");
                // @Iain, feel free to invoke purchase here.
            }
        });
    }
}
