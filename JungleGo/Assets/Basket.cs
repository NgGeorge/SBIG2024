using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Basket
{
    public List<Product> Products { get; private set; }

    public Basket()
    {
        Products = new List<Product>();
    }

    public void AddProduct(Product product)
    {
        Products.Add(product);
    }
}