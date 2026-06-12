using UnityEngine;
using UnityEngine.InputSystem;
using System.Collections;

[RequireComponent(typeof(CharacterController))]
[RequireComponent(typeof(Animator))]
public class PlayerMovement : MonoBehaviour
{
    [Header("Movement")]
    [SerializeField] private float walkSpeed = 5f;
    [SerializeField] private float runSpeed = 9f;

    private bool isRunning;

    [Header("Jump")]
    [SerializeField] private float jumpHeight = 2f;
    [SerializeField] private float gravity = -9.81f;

    [Header("References")]
    [SerializeField] private Transform cameraTransform;

    [Header("Attack")]
    [SerializeField] private GameObject fireballPrefab;   // ← префаб фаербола

    private CharacterController controller;
    private Animator animator;
    private PlayerInputActions input;

    private Vector2 moveInput;
    private float verticalVelocity;
    private bool isAttacking;

    private void Awake()
    {
        controller = GetComponent<CharacterController>();
        animator = GetComponent<Animator>();

        input = new PlayerInputActions();

        input.Player.Move.performed += ctx =>
            moveInput = ctx.ReadValue<Vector2>();

        input.Player.Move.canceled += ctx =>
            moveInput = Vector2.zero;

        input.Player.Jump.performed += _ =>
        {
            if (controller.isGrounded)
            {
                verticalVelocity = Mathf.Sqrt(jumpHeight * -2f * gravity);
                animator.SetTrigger("Jump");
            }
        };

        input.Player.Run.performed += _ => isRunning = true;
        input.Player.Run.canceled += _ => isRunning = false;

        input.Player.Attack.performed += _ => Attack();
    }

    private void OnEnable() => input.Enable();
    private void OnDisable() => input.Disable();

    private void Update()
    {
        // Направление относительно камеры
        Vector3 cameraForward = cameraTransform.forward;
        Vector3 cameraRight = cameraTransform.right;
        cameraForward.y = 0f;
        cameraRight.y = 0f;
        cameraForward.Normalize();
        cameraRight.Normalize();

        // Движение
        Vector3 moveDirection = cameraForward * moveInput.y + cameraRight * moveInput.x;
        if (moveDirection.magnitude > 1f)
            moveDirection.Normalize();

        float targetSpeed = 0f;
        if (moveDirection.magnitude > 0.1f)
        {
            targetSpeed = isRunning ? 1f : 0.5f;
        }
        animator.SetFloat("Speed", targetSpeed);
        animator.SetBool("IsGrounded", controller.isGrounded);

        // Гравитация
        if (controller.isGrounded && verticalVelocity < 0)
            verticalVelocity = -2f;
        verticalVelocity += gravity * Time.deltaTime;

        float currentSpeed = isRunning ? runSpeed : walkSpeed;
        Vector3 finalMove = moveDirection * currentSpeed + Vector3.up * verticalVelocity;
        controller.Move(finalMove * Time.deltaTime);

        // Поворот персонажа вслед за камерой
        if (cameraForward.sqrMagnitude > 0.01f)
        {
            Quaternion targetRotation = Quaternion.LookRotation(cameraForward);
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, 10f * Time.deltaTime);
        }
    }

    private void Attack()
    {
        if (isAttacking) return;
        if (!controller.isGrounded) return;

        isAttacking = true;
        animator.SetTrigger("Attack");
        StartCoroutine(PerformFireballCast());
    }

    private IEnumerator PerformFireballCast()
    {
        // Ждём момент каста
        yield return new WaitForSeconds(1.5f);

        // Спавним фаербол перед игроком
        Vector3 spawnPos = transform.position + transform.forward * 1.4f + Vector3.up * 2.4f;
        GameObject fireball = Instantiate(fireballPrefab, spawnPos, Quaternion.identity);
        Fireball fb = fireball.GetComponent<Fireball>();
        if (fb != null)
            fb.Launch(transform.forward);   // летит туда, куда смотрит персонаж

        // Кулдаун до следующей атаки
        yield return new WaitForSeconds(1f);
        isAttacking = false;
    }
}