using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class CustomerHandler : MonoBehaviour
{
    private NavMeshAgent _agent;

    private Animator _animator;

    private AudioSource audioSource;

    public Customer CustomerData;

    public BasketUI basketUI; 

    private Dictionary<Product, bool> purchaseHistory;

    [SerializeField]
    float minDistance;
    
    [SerializeField]
    public GameObject speechBubblePrefab;

    private GameObject _currentSpeechBubblePrefab;

    private Product _currentProduct;

    private bool _isWaiting;



    // Start is called before the first frame update
    void Start()
    {
        Debug.Log($"CustomerHandler Start()");

        // NavMesh init
        _agent = GetComponent<NavMeshAgent>();
		_agent.updateRotation = false;
		_agent.updateUpAxis = false;

        //  Animation
        _animator = GetComponent<Animator>();

        // GameObject
        audioSource = gameObject.AddComponent<AudioSource>();
        basketUI = GameObject.Find("BasketUI").GetComponent<BasketUI>();
    }

    // Update is called once per frame
    void Update()
    {
        var product = CustomerData?.GetNextProductInList();
        if (CustomerData != null && product != null)
        {
            if (!_isWaiting && Vector3.Distance(transform.position, product.Target.transform.position) > minDistance) 
            {
                // Walk
                if (!_animator.GetBool("IsWalking"))
                {
                    _animator.SetBool("IsWalking", true);
                }

                _agent.SetDestination(product.Target.transform.position);
            }
            else if (!_isWaiting)
            {
                // Stop
                _currentProduct = product;                
                StartCoroutine(WaitAndPerform(2f)); // Wait for 2 seconds
                CustomerData.TravelToNextShelf();
            }
        }
        else 
        {
            if (Vector3.Distance(transform.position, GameManager.Instance.EndPosition) > minDistance) 
            {
                if (!_animator.GetBool("IsWalking"))
                {
                    _animator.SetBool("IsWalking", true);
                }

                _agent.SetDestination(GameManager.Instance.EndPosition);
            }
            else 
            {
                var DoorNoise = GameObject.Find("Door").GetComponent<AudioSource>();
                DoorNoise.Play();
                if (basketUI.currentCustomer == CustomerData)
                {
                    basketUI.CloseBasket();
                }
                Destroy(gameObject);
            }
            // Call destory customer logic in here.
        }
        
    }

    void OnMouseDown()
    {
        basketUI.currentCustomer = CustomerData;
        basketUI.OpenBasket(CustomerData.Basket);
        Debug.Log($"{CustomerData.Basket.Products.Count} customer basket count");
        Debug.Log($"{basketUI.currentBasket.Products.Count} basket count");
    }

     IEnumerator WaitAndPerform(float waitTime)
    {
        _isWaiting = true;
        WaitForTransaction();

        yield return new WaitForSeconds(waitTime);
        _isWaiting = false;
        Destroy(_currentSpeechBubblePrefab);
    }

    void WaitForTransaction()
    {
        _animator.SetBool("IsWalking", false);

        audioSource.clip = _currentProduct.Audio;
        audioSource.Play();

        _currentSpeechBubblePrefab = Instantiate(speechBubblePrefab, transform);
        Transform childTransform = _currentSpeechBubblePrefab.transform.Find("Icon"); 
        
        if (childTransform != null)
        {
            SpriteRenderer spriteRenderer = childTransform.GetComponent<SpriteRenderer>();
            if (spriteRenderer != null)
            {
                spriteRenderer.sprite = _currentProduct.Icon;
            }
        }

        _currentSpeechBubblePrefab.transform.localPosition = transform.position + new Vector3(5f,15f,0);

        CustomerData.Basket.AddProduct(_currentProduct);
    }
}
