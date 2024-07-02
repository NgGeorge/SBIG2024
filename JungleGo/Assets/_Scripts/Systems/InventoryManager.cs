using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class InventoryManager : MonoBehaviour 
{
    private const int DEFAULT_MAX_PRODUCT_COUNT = 10;
    public Dictionary<Product, int> Stock { get; private set; }
    public InventoryManager()
    {
        Stock = GenerateStock();
    }

    private Dictionary<Product, int> GenerateStock(List<int> productIds = null, int maxProductCount = DEFAULT_MAX_PRODUCT_COUNT)
    {
        var newStock = new Dictionary<Product, int>();

        if (productIds != null)
        {
            var db = ProductDatabase.Instance;

            foreach (var id in productIds)
            {
                var product = db.GetProductById(id);
                if (product != null)
                {
                    var count = (int)System.Math.Ceiling(Random.value * maxProductCount); // ensure there is at least 1 product
                    newStock.Add(product, count);
                    Debug.Log($"Inventory: Added {count} {product.ProductName}s to inventory");
                }
            }
        }

        return newStock;
    }

    public void RegenerateStock(List<int> productIds = default, int maxProductCount = DEFAULT_MAX_PRODUCT_COUNT)
    {
        Stock = GenerateStock(productIds, maxProductCount);
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

    public Product GetRandomProduct()
    {
        var index = Random.Range(0, Stock.Count);
        return Stock.Keys.ElementAt<Product>(index);
    }
}
