using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(NavMeshAgent))]
public class VampireMovement : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private Transform player;

    [Header("Attack Settings")]
    [SerializeField] private float attackRange = 2.0f;
    [SerializeField] private float attackCooldown = 2.0f; // время между атаками
    [SerializeField] private float damage = 10f;
    private float lastAttackTime = -10f; // чтобы сразу можно было атаковать
    private bool isAttacking = false;

    [Header("Detection Settings")]
    [SerializeField] private float detectionRange = 15f;

    private NavMeshAgent agent;
    private Animator animator;
    private EnemyHealth enemyHealth;
    private Collider col;

    private void Awake()
    {
        agent = GetComponent<NavMeshAgent>();
        animator = GetComponent<Animator>();
        col = GetComponent<Collider>();
        enemyHealth = GetComponent<EnemyHealth>();

        if (enemyHealth != null)
        {
            enemyHealth.OnDeath.AddListener(HandleDeath);
        }
    }

    private void Update()
    {
        if (player == null) return;

        // Получаем компонент здоровья игрока
        PlayerHealth playerHealth = player.GetComponent<PlayerHealth>();
        if (playerHealth == null || !playerHealth.IsAlive)
        {
            // Игрок мёртв – останавливаем всё
            agent.isStopped = true;
            animator.SetFloat("Speed", 0f);
            isAttacking = false; // сбрасываем состояние атаки, если оно было
            return;
        }

        // Остальной код (расчёт дистанции, атака, движение) – без изменений
        float distance = Vector3.Distance(transform.position, player.position);

        if (distance > detectionRange)
        {
            agent.isStopped = true;
            animator.SetFloat("Speed", 0f);
            isAttacking = false;
            return;
        }

        if (isAttacking) return;

        if (distance <= attackRange && Time.time >= lastAttackTime + attackCooldown)
        {
            Attack();
            return;
        }

        agent.isStopped = false;
        agent.SetDestination(player.position);
        float currentSpeed = agent.velocity.magnitude;
        float normalizedSpeed = currentSpeed / agent.speed;
        animator.SetFloat("Speed", normalizedSpeed);
    }

    private void Attack()
    {
        // Останавливаем движение
        agent.isStopped = true;
        isAttacking = true;

        // Сбрасываем скорость в аниматоре (чтобы не бежал)
        animator.SetFloat("Speed", 0f);

        // Запускаем анимацию атаки
        animator.SetTrigger("Attack");

        // Запоминаем время атаки
        lastAttackTime = Time.time;

        if (player != null)
        {
            PlayerHealth playerHealth = player.GetComponent<PlayerHealth>();
            if (playerHealth != null)
                playerHealth.TakeDamage(damage);
        }

        Invoke(nameof(EndAttack), 1.0f);
    }

    private void EndAttack()
    {
        isAttacking = false;
        agent.isStopped = false;
    }

    private void HandleDeath()
    {
        if (agent != null) agent.enabled = false;
        this.enabled = false;

        if (col != null) col.enabled = false;

        animator.SetTrigger("Dead");
    }
}