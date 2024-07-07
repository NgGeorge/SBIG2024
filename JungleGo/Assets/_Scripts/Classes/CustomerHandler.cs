using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class CustomerHandler : MonoBehaviour
{
    private NavMeshAgent _agent;

    private Animator _animator;

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
    }

    // Update is called once per frame
    void Update()
    {
        if (CustomerData != null && CustomerData.GetNextProductInList() != null)
        {
            if (Vector3.Distance(transform.position, CustomerData.GetNextProductInList().Target.transform.position) > minDistance) 
            {
                // Walk
                if (!_animator.GetBool("IsWalking"))
                {
                    _animator.SetBool("IsWalking", true);
                }

                _agent.SetDestination(CustomerData.GetNextProductInList().Target.transform.position);
            }
            else
            {
                // Stop
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
                Destroy(gameObject);
            }
            // Call destory customer logic in here.
        }
        
    }

}
