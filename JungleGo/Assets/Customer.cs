using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

// Does this need to inherit MonoBehavior to control things like movements?
public class Customer
{
    public string Name { get; private set; }
    public List<Product> ShoppingList { get; private set;}
    public Basket Basket { get; private set; }

    private int _currentProductIndex = 0;

    public bool IsShoppingComplete = false;

    // TODO: Figure out collision and behavioral properties

    public Customer(string name)
    {
        Name = name;
        Basket = new Basket();
        ShoppingList = GenerateShoppingList(); 
    }

    private List<Product> GenerateShoppingList()
    {
        ShoppingList = new List<Product>();
        // TODO: Generate random number of items within a set boundary 
        // Consider whether that should be static or if we want it the
        // boundary to vary based on something like difficulty, etc.
        var maxProducts = ProductDatabase.Instance.ProductCount;
        var numProducts = Random.Range(1, maxProducts + 1);

        for (int i = 0; i < maxProducts; i++)
        {
            var product = ProductDatabase.Instance.GetRandomProduct();
            if (!ShoppingList.Any(p => p.Id == product.Id))
            {
                ShoppingList.Add(product);
                Debug.Log($"Customer {Name}: Added {product.ProductName} to shopping list");
            }
            
        }

        Debug.Log($"Customer {Name}: Created shopping list");

        return ShoppingList;
    }

    public void TravelToNextShelf(InventoryManager inventoryManager)
    {
        if (_currentProductIndex >= ShoppingList.Count)
        {
            FinishShopping();
            return;
        }

        // placeholder for now, until Yigit gives direction on path finding
        Debug.Log($"Customer {Name}: Travelling to next shelf");

        // Temporary: hard code visiting shelf to debug simulation
        OnShelfVisited(inventoryManager);
    }

    public void OnShelfVisited(InventoryManager inventoryManager)
    {
        if (_currentProductIndex >= ShoppingList.Count) 
        {
            FinishShopping();
            return;
        }

        _currentProductIndex++;

        if (ShouldPurchase())
        {
            PurchaseNextProduct(inventoryManager);
            TravelToNextShelf(inventoryManager);
        }
        else
        {
            Debug.Log($"Customer {Name}: Decided not to puchase the item");
            TravelToNextShelf(inventoryManager);
        }
    }

    private void PurchaseNextProduct(InventoryManager inventoryManager)
    {
        if (_currentProductIndex < ShoppingList.Count)
        {
            var product = ShoppingList[_currentProductIndex];

            Debug.Log($"Customer {Name}: attempting to purchase product {product.ProductName}");

            // if there is enough product left to purchase
            if (inventoryManager.PurchaseProduct(product))
            {
                Debug.Log($"Customer {Name}: purchased product {product.ProductName}");
                Basket.AddProduct(product);
            }
        }
        else
        {
            Debug.Log($"Customer {Name}: Shopping list complete");
            FinishShopping();
        }
    }

    private void FinishShopping()
    {
        // TODO: add any finishing tasks for the customer before leaving store
        this.IsShoppingComplete = true;

        Debug.Log($"Customer {Name}: Finished shopping");
    }

    // we could have fun with this with customer attribute
    // e.g. if the person really likes apples
    // for now flip a coin
    private bool ShouldPurchase()
    {
        Debug.Log($"Customer {Name}: Deciding to purchase from shelf");
        return Random.value > .5;
    }
}