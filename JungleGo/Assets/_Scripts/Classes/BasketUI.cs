using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System;
using UnityEngine;
using UnityEngine.UIElements;

public class BasketUI : MonoBehaviour
{
    public UIDocument uiBasket;
    public Basket currentBasket { get;set; }
    public bool isEnabled { get;set; }

    private StyleBackground openBasket;
    private StyleBackground closedBasket;
    private string openBasketPath = "Sprites/Basket_Open";
    private string closedBasketPath = "Sprites/Basket_Closed";

    private VisualElement basketWindow;
    private VisualElement insideBasket;
    private VisualElement basketContainer;
    private void Awake()
    {
        uiBasket = GetComponent<UIDocument>();
        isEnabled = false;
        openBasket = Resources.Load<Texture2D>(openBasketPath);
        closedBasket = Resources.Load<Texture2D>(closedBasketPath);
    }

    // Start is called before the first frame update
    void Start()
    {
        basketWindow = uiBasket.rootVisualElement.Q("BasketContainer");
        insideBasket = uiBasket.rootVisualElement.Q("InsideBasket");
        basketContainer = uiBasket.rootVisualElement.Q("Basket");
        CloseBasket();
    }

    public void OpenBasket(Basket basket)
    {
        Debug.Log("Opening Basket");
        isEnabled = true;
        insideBasket.visible = true;
        basketContainer.style.backgroundImage = openBasket; 
        var basketRows = insideBasket.Children().ToList();
        Debug.Log($"Basket rows : {basketRows.Count}");
        Debug.Log($"Basket products : {basket.Products.Count}");

        int i = 0;
        foreach (var product in basket.Products)
        {
            var prod = new VisualElement();
            prod.style.width = new Length(75, LengthUnit.Pixel);
            prod.style.height = new Length(75, LengthUnit.Pixel);
            prod.style.backgroundImage = Background.FromSprite(product.Icon);
            var index = Math.Min(i / 3, basketRows.Count);
            basketRows[index].Add(prod);
            i++;
        }
    }

    public void CloseBasket()
    {
        Debug.Log("Closing Basket");
        isEnabled = false;
        insideBasket.visible = false;
        basketContainer.style.backgroundImage = closedBasket; 
    }
}
