using UnityEngine;
using UnityEngine.UI;

public class PlayerHealth : MonoBehaviour
{
    [Header("Health Settings")]
    [SerializeField] private float maxHealth = 100f;
    private float currentHealth;

    [Header("UI")]
    [SerializeField] private Slider healthSlider;

    private Animator animator;

    public bool IsAlive => currentHealth > 0;

    void Start()
    {
        animator = GetComponent<Animator>();
        currentHealth = maxHealth;
        UpdateHealthUI();
    }

    public void TakeDamage(float damage) {
        if (!IsAlive) return;

        currentHealth -= damage;
        currentHealth = Mathf.Clamp(currentHealth, 0, maxHealth);

        Debug.Log($"Игрок получил {damage} урона. Осталось {currentHealth} HP");

        UpdateHealthUI();

        if (currentHealth <= 0) {
            Die();
        }
    }

    private void Die() {
        animator.SetTrigger("Dead");
        Debug.Log("Игрок умер");
    }

    void UpdateHealthUI()
    {
        if (healthSlider != null)
        {
            healthSlider.value = currentHealth;
        }
    }
}
