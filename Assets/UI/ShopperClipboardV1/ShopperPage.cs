using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.UIElements;

public class ShopperPage
{
    public Shopper shopper;
    //public Button button;
    public VisualElement shopperPageRoot;
    
    public ShopperPage(Shopper shopper, VisualTreeAsset template)
    {
        TemplateContainer itemButtonContainer = template.Instantiate();
        this.shopper = shopper;

        //button = itemButtonContainer.Q<Button>();
        //this.button.text = shopper.name;
        this.shopperPageRoot = itemButtonContainer.Q("ShopperPageRoot");
        //List<Button> buttons = itemButtonContainer.Children().OfType<Button>().ToList();
    }

    //V1: for each product in shopper.productsMasterList, set all button.text to the product name. for now assume a 1-1 relationship between productNames and the number of buttons.
    private void SetProductNames()
    {
        foreach (string productName in shopper.productsMasterList)
        {

        }
    }
    private void SetShopperName()
    {

    }
}

//button.RegisterCallback<ClickEvent>(OnClick);//may be used later in the constructor
//public void OnClick(ClickEvent evt)
//{
//    Debug.Log("the inventory slot with the item name " + item.displayName + " has been clicked");
//}
