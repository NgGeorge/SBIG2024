using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class CustomerMovement : MonoBehaviour
{
    [SerializeField] public GameObject target;
    private NavMeshAgent agent;
    private Animator animator;


    // Start is called before the first frame update
    void Start()
    {
        // NavMesh init
        agent = GetComponent<NavMeshAgent>();
		agent.updateRotation = false;
		agent.updateUpAxis = false;

        //  Animation
        animator = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Vector3.Distance(transform.position, target.transform.position) > 1.0f) 
        {
            // Walk
            if (!animator.GetBool("IsWalking"))
            {
                animator.SetBool("IsWalking", true);
            }
            
            agent.SetDestination(target.transform.position);
        }
        else
        {
            // Stop
            animator.SetBool("IsWalking", false);
        }
    }
}
