using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(NavMeshAgent))]
public class EnemyMovement : MonoBehaviour
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

        Debug.Log(agent.isOnNavMesh);
        Debug.Log(agent.remainingDistance);
        if (player != null)
        {
            agent.SetDestination(player.position);
        }
    }
}