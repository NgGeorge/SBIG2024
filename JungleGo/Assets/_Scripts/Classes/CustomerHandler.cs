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

    [SerializeField]
    float minDistance;


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
    }

    // Update is called once per frame
    void Update()
    {
        var product = CustomerData?.GetNextProductInList();
        if (CustomerData != null && product != null)
        {
            if (Vector3.Distance(transform.position, product.Target.transform.position) > minDistance) 
            {
                // Walk
                if (!_animator.GetBool("IsWalking"))
                {
                    _animator.SetBool("IsWalking", true);
                }

                _agent.SetDestination(product.Target.transform.position);
            }
            else
            {
                // Stop
                audioSource.clip = product.Audio;
                audioSource.Play();

                _animator.SetBool("IsWalking", false);
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
                Destroy(gameObject);
            }
            // Call destory customer logic in here.
        }
        
    }

}
