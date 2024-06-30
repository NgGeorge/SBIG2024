using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Product", menuName = "ScriptableObjects/Product", order = 1)]
public class Product : ScriptableObject
{
    public int Id;
    public string ProductName;
    public Sprite Icon;
    public decimal Price;
}