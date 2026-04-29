using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Rendering.Universal;

public class PlayerMovement : MonoBehaviour
{
    private Rigidbody2D rb;
    private Camera mainCamera;

    [SerializeField] private float moveSpeed = 5f;
    [SerializeField] private float sprintSpeed = 10f;

    private bool isSprinting;
    private Vector2 moveInput;
    private bool mouseHeld;

    private Light2D pointLight;
    private Vector2 lastDir = Vector2.down;

    public void OnMove(InputValue value)
    {
        moveInput = value.Get<Vector2>();
    }

    public void OnMouseMove(InputValue value)
    {
        mouseHeld = value.Get<float>() > 0;
    }

    public void OnSprint(InputValue value)
    {
        isSprinting = value.isPressed;
    }

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        mainCamera = Camera.main;
        pointLight = GetComponentInChildren<Light2D>();
    }

    void Update()
    {
        // Toggle light on F
        if (Keyboard.current.fKey.wasPressedThisFrame)
        {
            pointLight.gameObject.SetActive(!pointLight.gameObject.activeSelf);
        }

        // Update light direction
        if (pointLight.gameObject.activeSelf)
        {
            if (moveInput != Vector2.zero)
            {
                lastDir = moveInput.normalized;
            }

            pointLight.transform.localPosition = lastDir * 0.5f;

            float angle = Mathf.Atan2(lastDir.y, lastDir.x) * Mathf.Rad2Deg - 90f;
            pointLight.transform.rotation = Quaternion.Euler(0f, 0f, angle);
        }
    }

    void FixedUpdate()
    {
        if (mouseHeld)
            MoveToMouse();
        else
            MoveWithKeyboard();
    }

    private float GetCurrentSpeed()
    {
        return isSprinting ? sprintSpeed : moveSpeed;
    }

    private void MoveWithKeyboard()
    {
        float speed = GetCurrentSpeed();

        Vector2 newPosition = rb.position + moveInput * speed * Time.fixedDeltaTime;
        rb.MovePosition(newPosition);
    }

    private void MoveToMouse()
    {
        float speed = GetCurrentSpeed();

        Vector2 mouseScreenPosition = Mouse.current.position.ReadValue();

        Vector3 mouseWorldPosition = mainCamera.ScreenToWorldPoint(
            new Vector3(
                mouseScreenPosition.x,
                mouseScreenPosition.y,
                Mathf.Abs(mainCamera.transform.position.z)
            )
        );

        Vector2 target = mouseWorldPosition;

        Vector2 newPosition = Vector2.MoveTowards(
            rb.position,
            target,
            speed * Time.fixedDeltaTime
        );

        rb.MovePosition(newPosition);
    }
}