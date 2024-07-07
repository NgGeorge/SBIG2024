using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UIElements;

public class Clipboard : MonoBehaviour
{
    public UIDocument uiClipboard;
    public List<Customer> Customers { get;set; }
    public Dictionary<Product, int> Stock { get; set; }
    public Dictionary<Customer, Dictionary<Product, int>> playerInputData { get;set; }
    public AudioSource Pencil;
    public AudioSource Eraser;
    public AudioSource Paper;
    private VisualElement clipboardRoot { get;set; }
    private VisualElement clipboardTop { get;set; }
    private VisualElement clipboardBody { get;set; }
    private Customer currentCustomer;
    private int currentCustomerIndex;

    private void Awake()
    {
        uiClipboard = GetComponent<UIDocument>();
        Customers = new List<Customer>();
        playerInputData = new Dictionary<Customer, Dictionary<Product, int>>();
        Eraser = GameObject.Find("Eraser").GetComponent<AudioSource>();
        Paper = GameObject.Find("Paper").GetComponent<AudioSource>();
        Pencil = GameObject.Find("Pencil").GetComponent<AudioSource>();
    }

    /// <summary>
    /// Setups the initial state of the clipboard
    /// </summary>
    public void Initialize()
    {
        Debug.Log("Initialize Clipboard");
        clipboardRoot = uiClipboard.rootVisualElement.Q("Clipboard");
        clipboardTop = uiClipboard.rootVisualElement.Q("ClipboardTop");
        clipboardBody = uiClipboard.rootVisualElement.Q("ClipboardBody");
        currentCustomerIndex = 0;
        currentCustomer = Customers[currentCustomerIndex];

        CreatePlayerInputData();
        RegisterEvents();
        UpdateClipboard();
    }

    private void RegisterEvents()
    {
        Debug.Log("Registering Events on Clipboard");
        var paginationBack = clipboardTop.Q("PageBack");
        var paginationNext = clipboardTop.Q("PageNext");

        paginationBack.RegisterCallback<ClickEvent>(evt => OnClickChoosePreviousCustomer());
        paginationNext.RegisterCallback<ClickEvent>(evt => OnClickChooseNextCustomer());

        var productRows = clipboardBody.Children().ToList();
        Debug.Log($"Count of rows {productRows.Count}");
        foreach (var row in productRows) {
            var productSquares = row.Children().ToList();
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
        Debug.Log("Increase Prod Count");
        var count = int.Parse(prodCount.text);
        count++;
        prodCount.text = count.ToString();
        Pencil.Play();
        SaveClipboard();
    }

    private void OnClickDecreaseProdCount(Label prodCount)
    {
        Debug.Log("Decrease Prod Count");
        var count = int.Parse(prodCount.text);
        if (count > 0)
        {
            count--;
            prodCount.text = count.ToString();
        }
        Eraser.Play();
        SaveClipboard();
    }

    private void OnClickChooseNextCustomer()
    {
        Debug.Log("Choose next Customer");
        currentCustomerIndex = currentCustomerIndex == (Customers.Count - 1) ? 0 : currentCustomerIndex + 1;
        currentCustomer = Customers[currentCustomerIndex];
        Paper.Play();
        UpdateClipboard();
    }

    private void OnClickChoosePreviousCustomer()
    {
        Debug.Log("Choose previous Customer");
        currentCustomerIndex = currentCustomerIndex == 0 ? Customers.Count - 1 : currentCustomerIndex - 1;
        currentCustomer = Customers[currentCustomerIndex];
        Paper.Play();
        UpdateClipboard();
    }

    private void CreatePlayerInputData()
    {
        var productCounts = new Dictionary<Product, int>();
        foreach (var product in Stock.Keys)
        {
            productCounts.TryAdd(product, 0);
        }

        foreach (var customer in Customers)
        {
            playerInputData.TryAdd(customer, new Dictionary<Product, int>(productCounts));
        }
        Debug.Log($"Create player input data {playerInputData}");
    }


    /// <summary>
    /// UpdateClipboard fills in all of the values for the clipboard UI, handles pagination
    /// </summary>
    public void UpdateClipboard()
    {
        Debug.Log("Update Clipboard");
        var CustomerName = clipboardTop.Q("CustomerName") as Label;
        var CustomerId = clipboardTop.Q("CustomerId") as Label;
        CustomerName.text = currentCustomer.Name;
        CustomerId.text = "ID : " + currentCustomer.Id.ToString();

        var CustomerIcon = clipboardTop.Q("CustomerPicture");
        //CustomerIcon.style.backgroundImage = currentCustomer.Image;

        // Sneaking suspicion that if I just queried descendent product squares, it'll get it in the right order.
        int i = 0;
        var productRows = clipboardBody.Children().ToList();
        foreach (var row in productRows) {
            var productSquares = row.Children().ToList();
            foreach (var productSquare in productSquares) {
                var productIcon = productSquare.Q("ProductIcon");
                var productInput = productSquare.Q("ProductInput");
                var prodCount = productInput.Q("ProductCount") as Label;
                var currentProduct = playerInputData[currentCustomer].Keys.ToList()[i];
                prodCount.text = playerInputData[currentCustomer][currentProduct].ToString();
                productIcon.style.backgroundImage = Background.FromSprite(currentProduct.Icon);
                i++;
            }
        }
    }

    // Working on the assumption here that the list will be in the same order as the dictionary
    // Not super efficient but it's okay, if it works I'll be happy
    public void SaveClipboard()
    {
        Debug.Log("Save Clipboard");
        int i = 0;
        var prodCountLabels = clipboardBody.Query<Label>().ToList();
        var products = new List<Product>(playerInputData[currentCustomer].Keys);
        foreach (var product in products)
        {
            playerInputData[currentCustomer][product] = int.Parse(prodCountLabels[i].text);
            i++;
        }
    }

    public decimal CalculatePlayerTotal()
    {
        var total = 0.0M;
        foreach (var customerData in playerInputData.Values)
        {
            foreach (var product in customerData.Keys)
            {
                total += (product.getPrice() * customerData[product]);
            }
        }

        return total;
    }

    public void AddCustomer(Customer customer)
    {
        Debug.Log("AddCustomer");
        // Play audio here later
        Customers.Add(customer);
        CreatePlayerInputData();
    }
}
