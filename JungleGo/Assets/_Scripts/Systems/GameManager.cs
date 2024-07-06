using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    public List<Customer> Customers { get; private set; }

    internal List<Level> Levels { get; private set; }

    private int currentLevelIndex = 0;
    private System.Random random = new System.Random();

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);

            // Let's do dynamic level generation
            Levels = new List<Level>();
            for (int i = 1; i <= Constants.DifficultyCap; i++) {
                Levels.Add(new Level(i));
                Debug.Log($"Level Generation Initiated, Difficulty {i}");
            }
        }
        else
        {
            Destroy(gameObject);
        } 
    }

    public void Start()
    {
        // Given the limited time, let's reduce scope and randomly select a level
        // for a dynamic game experience. 
        currentLevelIndex = random.Next(0, Levels.Count);
        var level = Levels[currentLevelIndex];
        Debug.Log($"Initializing level {currentLevelIndex}");
        level.Initialize();
        Customers = level.Customers;

        StartCoroutine(CustomerStartLoop(level));
    }

    IEnumerator CustomerStartLoop(Level level)
    {
        var currentCustomerIndex = 0;
        while (true && !IsAllCustomersHaveFinished())
        {
            yield return new WaitForSeconds(random.Next(Constants.MinCustomerDelaySec, level.DelayBetweenCustomerSec));

            if (currentCustomerIndex < Customers.Count)
            {
                Debug.Log($"Dispatching Customer {Customers[currentCustomerIndex].Name}");
                Customers[currentCustomerIndex].TravelToNextShelf();
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
