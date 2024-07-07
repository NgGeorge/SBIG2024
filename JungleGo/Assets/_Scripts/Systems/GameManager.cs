using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    public List<Customer> Customers { get; private set; }
    public delegate void CoroutineCallback();

    internal List<Level> Levels { get; private set; }

    private int _currentLevelIndex = 0;
    private System.Random random = new System.Random();

    private Clipboard clipboard;
    private BasketUI basketUI;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);

            // Let's do dynamic level generation
            Levels = new List<Level>();
            for (int i = 1; i <= Constants.DifficultyCap; i++) 
            {
                Levels.Add(new Level(i));
            }
        }
        else
        {
            Destroy(gameObject);
        } 
    }

    public void Start()
    {
        // Given the limited time, let's reduce scope and maybe randomly select a level
        // for a dynamic game experience. 
        var level = Levels[_currentLevelIndex];
        level.Initialize();
        Customers = level.Customers;
        Debug.Log($"Game Customers : {Customers.Count}");
        clipboard = FindObjectOfType<Clipboard>();
        basketUI = FindObjectOfType<BasketUI>();
        var basket = new Basket();
        basket.AddProduct(ProductDatabase.Instance.GetProductById(1), 1);
        basket.AddProduct(ProductDatabase.Instance.GetProductById(2), 3);
        basket.AddProduct(ProductDatabase.Instance.GetProductById(3), 1);
        basket.AddProduct(ProductDatabase.Instance.GetProductById(4), 4);
        basketUI.OpenBasket(basket);

        StartCoroutine(StartLevel(level, OnLevelComplete));
    }

    IEnumerator StartLevel(Level level, CoroutineCallback callback)
    {
        var currentCustomerIndex = 0;
        while (true && !IsAllCustomersHaveFinished())
        {
            yield return new WaitForSeconds(random.Next(Constants.MinCustomerDelaySec, level.DelayBetweenCustomerSec));

            if (currentCustomerIndex < Customers.Count)
            {
                Debug.Log($"Dispatching Customer {Customers[currentCustomerIndex].Name}");
                if (currentCustomerIndex == 0)
                {
                    clipboard.Customers.Add(Customers.First());
                    clipboard.Stock = InventoryManager.Instance.Stock;
                    clipboard.AddCustomer(Customers[currentCustomerIndex]);
                    clipboard.Initialize();
                }
                else 
                {
                    clipboard.AddCustomer(Customers[currentCustomerIndex]);
                }
                Customers[currentCustomerIndex].TravelToNextShelf();
                currentCustomerIndex++;
            }
        }

        Debug.Log("Finished start routine");

        if (callback != null) {
            callback();
        }
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

    private void OnLevelComplete()
    {
        var hasWon = false;
        var CustomerSales = CalculateSales();
        var PlayerInput = clipboard.CalculatePlayerTotal();

        if ((CustomerSales == 0.0M) && (PlayerInput != 0.0M)) {
            Debug.Log("Loss for simple scenario.");
        } 
        else
        {
            var result = (PlayerInput / CustomerSales) * 100;
            Debug.Log($"Customer sales were ${CustomerSales.ToString("0.00")}");
            Debug.Log($"PlayerInput was ${PlayerInput.ToString("0.00")}");
            if (result >= (100 + Constants.WinThreshold)) 
            {
                // TODO : Show player charged too much money
                Debug.Log($"Result is {result.ToString("0.00")}%, lost because the player charged too much money.");
            }
            else if (result <= (100 - Constants.WinThreshold)) 
            {
                // TODO : Show player lost the company too much money
                Debug.Log($"Result is {result.ToString("0.00")}%, lost because the player lost too much money.");
            } else if (result == 100) {
                hasWon = true;
                Debug.Log("Won with a Perfect Score");
            } else {
                hasWon = true;
                Debug.Log($"Result is {result.ToString("0.00")}%, won within the margin of error +/- {Constants.WinThreshold}%.");
            }
        }
    }

    private decimal CalculateSales()
    {
        decimal total = 0.0M;
        foreach (var customer in Customers) 
        {
           total += customer.Basket.Total; 
        }

        return total;
    }
}
