using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

// Does this need to inherit MonoBehavior to control things like movements?
public class Customer
{
    public int Id { get; private set; }

    public string Name { get; private set; }

    public List<Product> ShoppingList { get; private set;}

    public Basket Basket { get; private set; }

    private int _currentProductIndex = 0;

    public bool IsShoppingComplete = false;
    
    public (int, int) Position { get; private set; } 

    public Sprite customerSprite { get; set; }

    public Customer(int id, string name)
    {
        Id = id;
        Name = name;
        Basket = new Basket();
        Position = (0, 0);
    }

    public void GenerateShoppingList()
    {
        ShoppingList = new List<Product>();
        // TODO: Generate random number of items within a set boundary 
        // Consider whether that should be static or if we want it the
        // boundary to vary based on something like difficulty, etc.
        var maxProducts = InventoryManager.Instance.Stock.Count;
        var numProducts = Random.Range(1, maxProducts + 1);

        for (int i = 0; i < numProducts; i++)
        {
            var product = InventoryManager.Instance.GetRandomProductFromInventory();
            if (!ShoppingList.Any(p => p.Id == product.Id))
            {
                ShoppingList.Add(product);
                Debug.Log($"Customer {Name}: Added {product.ProductName} to shopping list");
            }
            
        }

        Debug.Log($"Customer {Name}: Created shopping list");
    }

    public void TravelToNextShelf()
    {
        if (_currentProductIndex >= ShoppingList.Count)
        {
            FinishShopping();
            return;
        }

        // placeholder for now, until Yigit gives direction on path finding
        Debug.Log($"Customer {Name}: Travelling to next shelf");

        // Temporary: hard code visiting shelf to debug simulation
        OnShelfVisited();
    }

    public void OnShelfVisited()
    {
        if (_currentProductIndex >= ShoppingList.Count) 
        {
            FinishShopping();
            return;
        }

        _currentProductIndex++;

        PurchaseNextProduct();
    }

    private void PurchaseNextProduct()
    {
        if (_currentProductIndex < ShoppingList.Count)
        {
            var product = ShoppingList[_currentProductIndex];

            Debug.Log($"Customer {Name}: attempting to purchase product {product.ProductName}");

            // if there is enough product left to purchase
            if (InventoryManager.Instance.PurchaseProduct(product))
            {
                Debug.Log($"Customer {Name}: purchased product {product.ProductName}");
            }
        }
        else
        {
            Debug.Log($"Customer {Name}: Shopping list complete");
            FinishShopping();
        }
    }

    public Product GetNextProductInList()
    {
        if (_currentProductIndex < ShoppingList.Count )
        {
            return ShoppingList[_currentProductIndex];
        }
        else
        {
            return null;
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

    public int GetCurrentProductIndex()
    {
        return _currentProductIndex;
    }
}