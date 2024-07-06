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
        ClipboardTop = clipboard.rootVisualElement.Q("ClipboardTop");
        ClipboardBody = clipboard.rootVisualElement.Q("ClipboardBody");
        currentCustomerIndex = 0;
        currentCustomer = customers[currentCustomerIndex];

        CreatePlayerInputData();
        RegisterEvents();
        UpdateClipboard();
    }

    private void RegisterEvents()
    {
        var paginationBack = ClipboardTop.Q("PageBack").Descendents();
        var paginationNext = ClipboardTop.Q("PageNext").Descendents();

        paginationBack.RegisterCallback<ClickEvent>(OnClickChoosePreviousCustomer);
        paginationNext.RegisterCallback<ClickEvent>(OnClickChooseNextCustomer);

        var productRows = ClipboardBody.Q<VisualElement>().ToList();
        foreach (var row in productRows) {
            var productSquares = row.Q<VisualElement>().ToList();
            foreach (var productSquare in productSquares) {
                var productInput = productSquare.Q("ProductInput");
                var decrease = productInput.Q("Decrease");
                var increase = productInput.Q("Increase");
                var prodCount = productInput.Q("ProductCount");

                increase.RegisterCallback<ClickEvent>(evt => OnClickIncreaseProdCount(prodCount));
                decrease.RegisterCallback<ClickEvent>(evt => OnClickDecreaseProdCount(prodCount));
            }
        }
    }

    private void OnClickIncreaseProdCount(IntegerField prodCount)
    {
        prodCount.value++;
        SaveClipboard();
    }

    private void OnClickDecreaseProdCount(IntegerField prodCount)
    {
        if (prodCount.value > 0)
        {
            prodCount.value--;
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
        var CustomerName = ClipboardTop.Q("CustomerName").Descendents();
        var CustomerId = ClipboardTop.Q("CustomerId").Descendents();
        CustomerName.value = currentCustomer.Name;
        CustomerId.value = currentCustomer.Id;

        var CustomerIcon = ClipboardTop.Q("CustomerPicture").Descendents();
        //CustomerIcon.style.backgroundImage = currentCustomer.Image;

        // Sneaking suspicion that if I just queried descendent product squares, it'll get it in the right order.
        int i = 0;
        var productRows = ClipboardBody.Q<VisualElement>().ToList();
        foreach (var row in productRows) {
            var productSquares = row.Q<VisualElement>().ToList();
            foreach (var productSquare in productSquares) {
                var productIcon = productSquare.Q("ProductIcon");
                var productInput = productSquare.Q("ProductInput");
                var prodCount = productInput.Q("ProductCount");
                var currentProduct = playerInputData[currentCustomer].Keys[i];
                prodCount.value = playerInputData[currentCustomer][currentProduct];
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
        var intFields = ClipboardBody.Q<IntegerField>().Descendents().ToList();
        foreach (var product in playerInputData[currentCustomer].Keys())
        {
            // Yeah this is working off a ton of assumptions that are probably not all going to be true 
            playerInputData[currentCustomer][product] = intFields[i].value;
            i++;
        }
    }
}
