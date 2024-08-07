using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System;
using UnityEngine;
using UnityEngine.UIElements;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    public List<Customer> Customers { get; private set; }
    public delegate void CoroutineCallback();

    internal List<Level> Levels { get; private set; }

    private Level level;

    private int _currentLevelIndex = 0;
    private System.Random random = new System.Random();
    
    private GameObject[] _prefabArray;
    private GameObject endScreen;

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
        level = Levels[_currentLevelIndex];
        level.Initialize();
        Customers = level.Customers;
        Debug.Log($"Game Customers : {Customers.Count}");
        clipboard = FindObjectOfType<Clipboard>();
        clipboard.uiClipboard.rootVisualElement.style.display = DisplayStyle.None;
        basketUI = FindObjectOfType<BasketUI>();
        basketUI.uiBasket.rootVisualElement.style.display = DisplayStyle.None;
        endScreen = GameObject.Find("EndScreen");
        endScreen.SetActive(false);
    }

    public void StartGame()
    {
        Debug.Log("Start Game");
        GameObject.Find("MainMenu").SetActive(false);
        clipboard.uiClipboard.rootVisualElement.style.display = DisplayStyle.Flex;
        basketUI.uiBasket.rootVisualElement.style.display = DisplayStyle.Flex;
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
            if (currentCustomerIndex < Customers.Count)
            {
                Debug.Log($"Dispatching Customer {Customers[currentCustomerIndex].Name}");

                // TODO : This is where the customers should spawn
                GameObject customerPrefab = Instantiate(_prefabArray[random.Next(0, _prefabArray.Length)], _startPotision, Quaternion.identity);
                var DoorNoise = GameObject.Find("Door").GetComponent<AudioSource>();
                DoorNoise.Play();
                
                Component[] components = customerPrefab.GetComponents<Component>();
                
                foreach (Component component in components)
                {
                    Debug.Log("Components:" + component.GetType().Name);
                }

                var customer = Customers[currentCustomerIndex];
                CustomerHandler customerHandlerScript = customerPrefab.GetComponent<CustomerHandler>();
                customerHandlerScript.CustomerData = customer;
                customer.customerSprite = customerHandlerScript.GetComponent<SpriteRenderer>().sprite;
                Debug.Log($"Is customer sprite available? {customer.customerSprite == null}");

                if (currentCustomerIndex == 0)
                {
                    clipboard.Customers.Add(customer);
                    clipboard.Stock = InventoryManager.Instance.Stock;
                    clipboard.AddCustomer(customer);
                    clipboard.Initialize();
                }
                else 
                {
                    clipboard.AddCustomer(customer);
                }

                //Customers[currentCustomerIndex].TravelToNextShelf();
                currentCustomerIndex++;
            }

            yield return new WaitForSeconds(random.Next(Constants.MinCustomerDelaySec, level.DelayBetweenCustomerSec));
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
        var CustomerSales = CalculateSales();
        var PlayerInput = clipboard.CalculatePlayerTotal();
        endScreen.SetActive(true);
        EndScreen esController = endScreen.GetComponent<EndScreen>();
        esController.ShowEndScreen(PlayerInput, CustomerSales);
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
