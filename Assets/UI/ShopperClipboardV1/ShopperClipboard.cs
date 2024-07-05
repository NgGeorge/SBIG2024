using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class ShopperClipboard : MonoBehaviour
{
    UIDocument uiShopperClipboard;
    public VisualTreeAsset shopperButtonTemplate;//make it public so you can drag a .uxml to the empty slot in the Inspector; todo: rename to ShopperPageTemplate
    public List<Shopper> shoppers = new List<Shopper>();

    private void OnEnable()
    {
        uiShopperClipboard = GetComponent<UIDocument>();//Find the UIDocument that is on the same GameObject as this script

        ManuallyAddShoppers();

        foreach (Shopper shopper in shoppers)//todo: put in its own function
        {
            //Approach 1: using another class ShopperPage
            ShopperPage newSlot = new ShopperPage(shopper, shopperButtonTemplate);
            uiShopperClipboard.rootVisualElement.Q("ShopperRow").Add(newSlot.shopperPageRoot);

            //Approach 2: without using another class?
        }
    }
    private void ManuallyAddShoppers()//temp
    {
        shoppers.Add(new Shopper("Shopper 1"));
        shoppers.Add(new Shopper("Shopper 2"));
    }

    // monobehavior method: Start is called before the first frame update
    void Start()
    {
        
    }

    // monobehavior method: Update is called once per frame
    void Update()
    {
        
    }
}
