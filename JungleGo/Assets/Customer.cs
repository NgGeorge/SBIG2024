using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Does this need to inherit MonoBehavior to control things like movements?
public class Customer
{
    public string Name { get; private set; }
    public List<Product> ShoppingList { get; private set;}
    public Basket Basket { get; private set; }

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
        Product product = new Product();
        ShoppingList.Add(product);

        return ShoppingList;
    }
}