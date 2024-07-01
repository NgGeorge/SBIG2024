using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Basket
{
    public List<Product> Products { get; private set; }
    public decimal Total = 0.0M;
    public Basket()
    {
        Products = new List<Product>();
    }

    public void AddProduct(Product product, int numProducts = 1)
    {
        Debug.Log($"Basket: Added {numProducts} {product.ProductName}'s to basket");
        Products.Add(product);
        Total += product.Price;
        Debug.Log($"Basket: New basket total is {Total:C}");
    }
}