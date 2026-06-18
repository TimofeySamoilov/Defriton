using UnityEngine;

public class EnemyHealth : MonoBehaviour
{
    [SerializeField] private float maxHealth = 100f;
    private float currentHealth;
    private Animator animator;

    private void Start()
    {
        currentHealth = maxHealth;
        animator = GetComponent<Animator>();
    }

    public void TakeDamage(float damage)
    {
        currentHealth -= damage;
        Debug.Log($"Вампир получил {damage} урона. Осталось HP: {currentHealth}");

        if (currentHealth <= 0f)
        {
            Die();
        } else
        {
            animator.SetTrigger("Hit");
        }
    }

    private void Die()
    {
        Debug.Log("Вампир умер!");
        // Здесь можно добавить анимацию смерти, отключение коллайдера, уничтожение объекта и т.д.
        animator.SetTrigger("Dead");
    }
}