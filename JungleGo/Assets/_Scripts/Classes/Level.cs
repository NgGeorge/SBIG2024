﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

public class Level
{
    public List<Customer> Customers { get; set; }
    public int DelayBetweenCustomerSec;
    private List<string> uniqueNameList { get; set; }
    private Random random { get; set; }
   
    
    public Level (int difficulty = 1) 
    {
        difficulty = Math.Max(difficulty, Constants.DifficultyCap);
        uniqueNameList = Constants.CustomerNames.ToList();
        random = new Random();
        Customers = GenerateCustomers(difficulty);

        // Min limit incase we want to adjust the max delay or difficulty cap later
        DelayBetweenCustomerSec = Math.Min(Constants.MinCustomerDelaySec, Constants.MaxCustomerDelaySec - difficulty);
    }

    public void Initialize() 
    {
        InventoryManager.Instance.GenerateStock();     
        foreach (var customer in Customers) {
            customer.GenerateShoppingList();
        }
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
        int customerLimit = Constants.MinCustomers + (difficulty * Constants.CustomerCountDiffMod); //1; // todo: Constants.MinCustomers + (difficulty * Constants.CustomerCountDiffMod);
        for (int i = 0; i < customerLimit; i++) {
            var name = PickCustomerName();
            Customer newCustomer = new Customer(i + 1, name);
            cList.Add(newCustomer);
        }

        return cList;
    }
}

