using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventoryManager : MonoBehaviour 
{
    public Dictionary<Product, int> Stock { get; private set; }
    public InventoryManager()
    {
        Stock = generateStock();
    }

    private Dictionary<Product, int> generateStock()
    {
        var newStock = new Dictionary<Product, int>();
        // TODO : Implement me

        return newStock;
    }

    public void regenerateStock()
    {
        Stock = generateStock();
    }
}
