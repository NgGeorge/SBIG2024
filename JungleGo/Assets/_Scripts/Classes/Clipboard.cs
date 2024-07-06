using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UIElements;

public class Clipboard : MonoBehaviour
{
    UIDocument uiClipboard;
    public const int clipboardRows = 3;
    public const int clipboardCols = 3;
    public List<Customer> customers { get;set; }
    public Dictionary<Customer, Dictionary<Product, int>> playerInputData { get;set; }
    public VisualElement ClipboardRoot { get;set; }
    public VisualElement ClipboardTop { get;set; }
    public VisualElement ClipboardBody { get;set; }
    private Customer currentCustomer;
    private int currentCustomerIndex;

    public Clipboard(List<Customer> customers)
    {
        this.customers = customers;
    }

    // Start is called before the first frame update
    void Start()
    {
        uiClipboard = GetComponent<UIDocument>();
        Initialize();
    }

    /// <summary>
    /// Setups the initial state of the clipboard
    /// </summary>
    private void Initialize()
    {
        ClipboardRoot = uiClipboard.rootVisualElement.Q("Clipboard");
        ClipboardTop = uiClipboard.rootVisualElement.Q("ClipboardTop");
        ClipboardBody = uiClipboard.rootVisualElement.Q("ClipboardBody");
        currentCustomerIndex = 0;
        currentCustomer = customers[currentCustomerIndex];

        CreatePlayerInputData();
        RegisterEvents();
        UpdateClipboard();
    }

    private void RegisterEvents()
    {
        var paginationBack = ClipboardTop.Q("PageBack");
        var paginationNext = ClipboardTop.Q("PageNext");

        paginationBack.RegisterCallback<ClickEvent>(evt => OnClickChoosePreviousCustomer());
        paginationNext.RegisterCallback<ClickEvent>(evt => OnClickChooseNextCustomer());

        var productRows = ClipboardBody.Query<VisualElement>().ToList();
        foreach (var row in productRows) {
            var productSquares = row.Query<VisualElement>().ToList();
            foreach (var productSquare in productSquares) {
                var productInput = productSquare.Q("ProductInput");
                var decrease = productInput.Q("Decrease") as Button;
                var increase = productInput.Q("Increase") as Button;
                var prodCount = productInput.Q("ProductCount") as Label;

                increase.RegisterCallback<ClickEvent>(evt => OnClickIncreaseProdCount(prodCount));
                decrease.RegisterCallback<ClickEvent>(evt => OnClickDecreaseProdCount(prodCount));
            }
        }
    }

    private void OnClickIncreaseProdCount(Label prodCount)
    {
        var count = int.Parse(prodCount.text);
        count++;
        prodCount.text = count.ToString();
        SaveClipboard();
    }

    private void OnClickDecreaseProdCount(Label prodCount)
    {
        var count = int.Parse(prodCount.text);
        if (count > 0)
        {
            count--;
            prodCount.text = count.ToString();
        }
        SaveClipboard();
    }

    private void OnClickChooseNextCustomer()
    {
        currentCustomerIndex = currentCustomerIndex == customers.Count - 1 ? 0 : currentCustomerIndex - 1;
        currentCustomer = customers[currentCustomerIndex];
        UpdateClipboard();
    }

    private void OnClickChoosePreviousCustomer()
    {
        currentCustomerIndex = currentCustomerIndex == 0 ? customers.Count - 1 : currentCustomerIndex + 1;
        currentCustomer = customers[currentCustomerIndex];
        UpdateClipboard();
    }

    private void CreatePlayerInputData()
    {
        playerInputData = new Dictionary<Customer, Dictionary<Product, int>>();
        var productCounts = new Dictionary<Product, int>();
        foreach (var product in InventoryManager.Instance.Stock.Keys)
        {
            productCounts.TryAdd(product, 0);
        }

        foreach (var customer in customers)
        {
            playerInputData.TryAdd(customer, new Dictionary<Product, int>(productCounts));
        }
    }


    /// <summary>
    /// UpdateClipboard fills in all of the values for the clipboard UI, handles pagination
    /// </summary>
    public void UpdateClipboard()
    {
        var CustomerName = ClipboardTop.Q("CustomerName") as Label;
        var CustomerId = ClipboardTop.Q("CustomerId") as Label;
        CustomerName.text = currentCustomer.Name;
        CustomerId.text = currentCustomer.Id.ToString();

        var CustomerIcon = ClipboardTop.Q("CustomerPicture");
        //CustomerIcon.style.backgroundImage = currentCustomer.Image;

        // Sneaking suspicion that if I just queried descendent product squares, it'll get it in the right order.
        int i = 0;
        var productRows = ClipboardBody.Query<VisualElement>().ToList();
        foreach (var row in productRows) {
            var productSquares = row.Query<VisualElement>().ToList();
            foreach (var productSquare in productSquares) {
                var productIcon = productSquare.Q("ProductIcon");
                var productInput = productSquare.Q("ProductInput");
                var prodCount = productInput.Q("ProductCount") as Label;
                var currentProduct = playerInputData[currentCustomer].Keys.ToList()[i];
                prodCount.text = playerInputData[currentCustomer][currentProduct].ToString();
                //productIcon.style.backgroundImage = currentProduct.image;
                i++;
            }
        }
    }

    // Working on the assumption here that the list will be in the same order as the dictionary
    // Not super efficient but it's okay, if it works I'll be happy
    public void SaveClipboard()
    {
        int i = 0;
        var intFields = ClipboardBody.Query<Label>().ToList();
        foreach (var product in playerInputData[currentCustomer].Keys)
        {
            // Yeah this is working off a ton of assumptions that are probably not all going to be true 
            playerInputData[currentCustomer][product] = int.Parse(intFields[i].text);
            i++;
        }
    }
}
