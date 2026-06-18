using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(NavMeshAgent))]
public class VampireMovement : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private Transform player;
    private NavMeshAgent agent;
    private Animator animator;

    [Header("Attack Settings")]
    [SerializeField] private float attackRange = 2.0f;
    [SerializeField] private float attackCooldown = 2.0f; // время между атаками
    private float lastAttackTime = -10f; // чтобы сразу можно было атаковать
    private bool isAttacking = false;

    private void Awake()
    {
        agent = GetComponent<NavMeshAgent>();
        animator = GetComponent<Animator>();
    }

    private void Update()
    {
        if (player == null) return;

        float distance = Vector3.Distance(transform.position, player.position);

        // Если уже атакуем — не двигаемся и ждём окончания
        if (isAttacking)
        {
            // Здесь можно проверить, закончилась ли анимация, и выйти из состояния
            // Но проще использовать таймер или событие анимации.
            // Пока используем просто таймер (но лучше через Animation Event)
            return;
        }

        // Проверяем, можем ли атаковать
        if (distance <= attackRange && Time.time >= lastAttackTime + attackCooldown)
        {
            Attack();
            return;
        }

        // Иначе — двигаемся к игроку
        agent.isStopped = false;
        agent.SetDestination(player.position);

        // Анимация скорости
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

        // Включаем таймер для выхода из состояния атаки (если нет Animation Event)
        // Здесь можно использовать Invoke или корутину.
        Invoke(nameof(EndAttack), 1.0f); // предположим, анимация длится 1 секунду
        // Лучше использовать Animation Event — см. ниже.
    }

    private void EndAttack()
    {
        isAttacking = false;
        agent.isStopped = false;
        // Движение возобновится в следующем Update
    }

    // Этот метод можно вызвать из Animation Event в конце анимации атаки
    // Тогда не нужен Invoke.
    public void OnAttackAnimationEnd()
    {
        EndAttack();
    }
}