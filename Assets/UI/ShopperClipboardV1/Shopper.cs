using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shopper
{
    public string name;
    public List<string> productsMasterList = new();

    public Shopper(string name)
    {
        this.name = name;
        this.productsMasterList.Add("milk");
        this.productsMasterList.Add("cookies");
        this.productsMasterList.Add("nuts");
        this.productsMasterList.Add("juice");
        this.productsMasterList.Add("goat");
        this.productsMasterList.Add("fruit");
    }
}
