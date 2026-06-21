using UnityEngine;
using UnityEngine.Events;

public class EnemyHealth : MonoBehaviour
{
    [SerializeField] private float maxHealth = 100f;
    private float currentHealth;
    private Animator animator;

    public UnityEvent OnDeath;

    private void Start()
    {
        currentHealth = maxHealth;
        animator = GetComponent<Animator>();
    }

    public void TakeDamage(float damage)
    {
        currentHealth -= damage;
        Debug.Log($"Враг получил {damage} урона. Осталось HP: {currentHealth}");

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
       OnDeath.Invoke();
    }
}