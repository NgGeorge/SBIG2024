using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProductDatabase : MonoBehaviour
{
    public static ProductDatabase Instance { get; private set; }
    private List<Product> products = new List<Product>();
    public int ProductCount { get { return products.Count; } }

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            // Persist this object across scenes
            DontDestroyOnLoad(gameObject);
            Initialize();
        }
        else
        {
            Destroy(gameObject);
        }
    }

    // Initialize the product database
    private void Initialize()
    {
        // Load all Product assets from the Resources folder
        // These products can be generated as .assets folder in Unity
        // Product[] loadedProducts = Resources.LoadAll<Product>("ProductData");
        // products.AddRange(loadedProducts);

        // Alternatively, we can just create them and add them here. 
        //products.Add(new Product { Id = 1, ProductName = "Apple", Icon = Resources.Load<Sprite>("Icons/Apple") });
        products.Add(new Product { Id = 1, ProductName = "Apple", Price = 2.0M });
        products.Add(new Product { Id = 2, ProductName = "Orange", Price = 3.0M });
        products.Add(new Product { Id = 3, ProductName = "TV", Price = 500.0M });
        products.Add(new Product { Id = 4, ProductName = "Guitar", Price = 200.0M });
        products.Add(new Product { Id = 5, ProductName = "Pie", Price = 20.0M });
    }

    public Product GetProductById(int id)
    {
        return products.Find(product => product.Id == id);
    }

    public List<Product> GetAllProducts()
    {
        return new List<Product>(products);
    }

    public Product GetRandomProduct()
    {
        var index = Random.Range(0, products.Count);
        return products[index];
    }
}