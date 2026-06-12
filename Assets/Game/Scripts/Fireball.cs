using UnityEngine;

public class Fireball : MonoBehaviour
{
    [SerializeField] private float speed = 15f;
    [SerializeField] private float lifetime = 4f;
    [SerializeField] private int damage = 30;

    private Rigidbody rb;

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
    }

    public void Launch(Vector3 direction)
    {
        rb.linearVelocity = direction.normalized * speed;
        Destroy(gameObject, lifetime);
    }

    private void OnTriggerEnter(Collider other)
    {
        // Попробуем получить компонент здоровья врага
        EnemyHealth enemy = other.GetComponent<EnemyHealth>();
        if (enemy != null)
        {
            enemy.TakeDamage(damage);
        }

        // Уничтожаем фаербол при любом столкновении (можно доработать, чтобы не уничтожался о стены)
        Destroy(gameObject);
    }
}