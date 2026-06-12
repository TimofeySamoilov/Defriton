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
        // Задаём постоянную скорость в указанном направлении
        rb.linearVelocity = direction.normalized * speed;
        // Уничтожим через время, если ни во что не попал
        Destroy(gameObject, lifetime);
    }

    private void OnTriggerEnter(Collider other)
    {
        // Здесь можно проверять, попали ли во врага
        // if (other.TryGetComponent<EnemyHealth>(out var enemy))
        // {
        //     enemy.TakeDamage(damage);
        // }

        // После попадания уничтожаем фаербол
        Destroy(gameObject);
    }
}