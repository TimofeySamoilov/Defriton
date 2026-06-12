using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerCamera : MonoBehaviour
{
    [SerializeField] private Transform cameraTarget;
    [SerializeField] private float sensitivity = 150f;

    private PlayerInputActions input;
    private Vector2 lookInput;

    private float pitch;

    private void Awake()
    {
        input = new PlayerInputActions();

        input.Player.Look.performed += ctx =>
            lookInput = ctx.ReadValue<Vector2>();

        input.Player.Look.canceled += ctx =>
            lookInput = Vector2.zero;
    }

    private void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    private void OnEnable()
    {
        input.Enable();
    }

    private void OnDisable()
    {
        input.Disable();
    }

    private void Update()
    {
        float mouseX = lookInput.x * sensitivity * Time.deltaTime;
        float mouseY = lookInput.y * sensitivity * Time.deltaTime;

        // Поворот игрока по горизонтали
        transform.Rotate(Vector3.up * mouseX);

        // Поворот камеры по вертикали
        pitch -= mouseY;
        pitch = Mathf.Clamp(pitch, -60f, 80f);

        cameraTarget.localRotation =
            Quaternion.Euler(pitch, 0f, 0f);
    }
}