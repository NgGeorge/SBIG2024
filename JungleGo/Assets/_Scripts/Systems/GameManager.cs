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
    
    private GameObject[] _prefabArray;

    private Vector3 _startPotision;
    public Vector3 EndPosition;

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
        _startPotision = GameObject.FindGameObjectsWithTag("Start")[0].transform.position;
        EndPosition = GameObject.FindGameObjectsWithTag("Exit")[0].transform.position;
        LoadPrefabs();

        StartCoroutine(StartLevel(level, OnLevelComplete));
    }

    IEnumerator StartLevel(Level level, CoroutineCallback callback)
    {
        var currentCustomerIndex = 0;
        while (!IsAllCustomersHaveFinished())
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

                // TODO : This is where the customers should spawn
                GameObject customerPrefab = Instantiate(_prefabArray[random.Next(0, _prefabArray.Length)], _startPotision, Quaternion.identity);
                
                Component[] components = customerPrefab.GetComponents<Component>();
                
                foreach (Component component in components)
                {
                    Debug.Log("Components:" + component.GetType().Name);
                }

                CustomerHandler customerHandlerScript = customerPrefab.GetComponent<CustomerHandler>();
                customerHandlerScript.CustomerData = Customers[currentCustomerIndex];

                //Customers[currentCustomerIndex].TravelToNextShelf();
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

    void LoadPrefabs()
    {
        var prefabFolderPath = "Prefabs/Customers";
        // Load all prefabs in the specified folder
        _prefabArray = Resources.LoadAll<GameObject>(prefabFolderPath);
        if (_prefabArray.Length > 0)
        {
            Debug.Log($"{_prefabArray.Length} prefabs loaded from {prefabFolderPath}.");
        }
        else
        {
            Debug.LogWarning("No prefabs found in the specified folder.");
        }
    }
}
