using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(NavMeshAgent))]
public class VampireMovement : MonoBehaviour
{
    [SerializeField] private Transform player;

    private NavMeshAgent agent;
    private Animator animator;

    private void Awake()
    {
        agent = GetComponent<NavMeshAgent>();
        animator = GetComponent<Animator>();
    }

    private void Update()
    {
        if (player != null)
        {
            agent.SetDestination(player.position);

            float currentSpeed = agent.velocity.magnitude;
            float normalizedSpeed = currentSpeed / agent.speed;
            animator.SetFloat("Speed", normalizedSpeed);
        }
    }
}