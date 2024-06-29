using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Productbase : MonoBehaviour
{
    public static Productbase Instance { get; private set; }
    private List<Product> products = new List<Product>();

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
        Product[] loadedProducts = Resources.LoadAll<Product>("ProductData");
        products.AddRange(loadedProducts);

        // Alternatively, we can just create them and add them here. 
        // products.Add(new Product { id = 1, productName = "Apple", icon = Resources.Load<Sprite>("Icons/Apple") });
    }

    public Product GetProductById(int id)
    {
        return products.Find(product => product.Id == id);
    }

    public List<Product> GetAllProducts()
    {
        return new List<Product>(products);
    }
}