using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

public class Level
{
    public List<Customer> Customers { get; set; }
    public List<Product> Products { get; set; }
    public int DelayBetweenCustomerSec;
    private List<string> uniqueNameList { get; set; }
    private List<int> uniqueProductIds { get;set; }
    private Random random { get; set; }
   
    
    public Level (int difficulty = 1) {
        difficulty = Math.Max(difficulty, Constants.DifficultyCap);
        Products = new List<Product>();
        uniqueNameList = Constants.CustomerNames.ToList();
        uniqueProductIds = new List<int>();
        random = new Random();
        Customers = GenerateCustomers(difficulty);
        Products = GenerateProductList();

        // Min limit incase we want to adjust the max delay or difficulty cap later
        DelayBetweenCustomerSec = Math.Min(Constants.MinCustomerDelaySec, Constants.MaxCustomerDelaySec - difficulty);
    }

    /// <summary>
    /// For selecting unique customer names, but will reuse names if we generate more names than we have defined. 
    /// </summary>
    private string PickCustomerName()
    {
        // Refresh names list if we run out
        if (uniqueNameList.Count <= 0) {
            uniqueNameList = Constants.CustomerNames.ToList();
        }

        int index = random.Next(0, uniqueNameList.Count);
        var name = uniqueNameList[index];
        uniqueNameList.RemoveAt(index);

        return name;
    }

    /// <summary>
    /// Generate a list of customers 
    /// </summary>
    private List<Customer> GenerateCustomers(int difficulty)
    {
        var cList = new List<Customer>();
        // When setting this value, consider that the player has to swap between pages.
        // Setting the limit too high can make it literally impossible to play
        int customerLimit = Constants.MinCustomers + (difficulty * Constants.CustomerCountDiffMod);
        for (int i = 0; i < customerLimit; i++) {
            var name = PickCustomerName();
            Customer newCustomer = new Customer(i + 1, name);
            Customers.Add(newCustomer);
        }

        return cList;
    }

    /// <summary>
    /// Select a random unique product from our database if possible, otherwise it'll refresh the list of products and reuse them
    /// </summary>
    private Product PickProduct()
    {
        // Refresh products list if we run out
        if (uniqueProductIds.Count <= 0) {
            uniqueProductIds.Clear();
            for (int i = 0; i < ProductDatabase.Instance.ProductCount; i++) {
                uniqueProductIds.Add(i + 1);
            }
        }

        int index = random.Next(0, uniqueProductIds.Count);
        var id = uniqueProductIds[index];
        uniqueProductIds.RemoveAt(index);

        return ProductDatabase.Instance.GetProductById(id);

    }

    /// <summary>
    /// For generating a list of products
    /// </summary>
    private List<Product> GenerateProductList()
    {
        var pList = new List<Product>();
        for (int i = 0; i < Constants.MaxUniqueProducts; i++) {
            pList.Add(PickProduct());
        }

        return pList;
    } 

}

