using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{
    private float moveSpeed = 5f;
    private float sprintSpeed = 10f;
    private float currentSpeed;
    private Rigidbody2D rb;
    private Vector2 moveInput;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        bool isSprinting = Keyboard.current !=null && Keyboard.current.leftShiftKey.isPressed;
        currentSpeed = isSprinting ? sprintSpeed : moveSpeed;

        rb.linearVelocity = moveInput * currentSpeed;
    }

    public void Move(InputAction.CallbackContext context)
    {
        moveInput = context.ReadValue<Vector2>();
    }
}
