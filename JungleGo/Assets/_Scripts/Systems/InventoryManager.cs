using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class InventoryManager : MonoBehaviour 
{
    public static InventoryManager Instance { get; private set; }

    public Dictionary<Product, int> Stock { get; private set; }
    private List<int> uniqueProductIds { get;set; }
    private System.Random random { get; set; }

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            // Persist this object across scenes
            DontDestroyOnLoad(gameObject);
            random = new System.Random();
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void GenerateStock(int maxProductCount = Constants.MaxUniqueProducts)
    {
        var newStock = new Dictionary<Product, int>();
        uniqueProductIds = new List<int>();

        for (int i = 0; i < Constants.MaxUniqueProducts; i++) {
            var product = PickProduct();
            var count = (int)System.Math.Ceiling(UnityEngine.Random.value * maxProductCount); // ensure there is at least 1 product
            newStock.Add(product, count);
            Debug.Log($"Inventory: Added {count} {product.ProductName}s to inventory");
        }

        Stock = newStock;
    }

    public bool PurchaseProduct(Product product, int numPurchased = 1)
    {
        Debug.Log($"Inventory: Customer attempting to purchase {numPurchased} {product.ProductName}'s from inventory");

        if (Stock.TryGetValue(product, out var count) && count > numPurchased) 
        {
            Debug.Log($"Inventory: Customer succeeded to purchase {numPurchased} {product.ProductName}'s from inventory");
            Stock[product] = count - 1;
            return true;
        }

        Debug.Log($"Inventory: Customer failed to purchase {numPurchased} {product.ProductName}'s from inventory");
        // TODO: what happens if the product doesn't exist (highly unlikely edge case)
        // TODO: what happens if not enough product is in stock? 
        return false;
    }

    /// <summary>
    /// Select a random unique product from our database if possible, otherwise it'll refresh the list of products and reuse them
    /// </summary>
    private Product PickProduct()
    {
        // Refresh products list if we run out
        if (uniqueProductIds.Count <= 0) {
            for (int i = 0; i < ProductDatabase.Instance.ProductCount; i++) {
                uniqueProductIds.Add(i + 1);
            }
        }

        int index = random.Next(0, uniqueProductIds.Count);
        var id = uniqueProductIds[index];
        uniqueProductIds.RemoveAt(index);

        return ProductDatabase.Instance.GetProductById(id);
    }

    public Product GetRandomProductFromInventory()
    {
        var index = UnityEngine.Random.Range(0, Stock.Count);
        return Stock.Keys.ElementAt<Product>(index);
    }
}
