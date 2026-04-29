using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{
    private Rigidbody2D rb;
    private Camera mainCamera;

    [SerializeField] private float moveSpeed = 5f;
    [SerializeField] private float sprintSpeed = 10f;

    private bool isSprinting;
    private Vector2 moveInput;
    private bool mouseHeld;

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