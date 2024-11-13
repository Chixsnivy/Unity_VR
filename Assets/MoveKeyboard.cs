using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(CharacterController))]

public class MoveKeyboard : MonoBehaviour
{
    public float speed;
    public float runMultiplier;
    public float gravity = -9.81f;
    public float jumpHeight;
    public float rotationSpeed;

    private CharacterController characterController;
    private Vector3 velocity;
    private bool isRunning = false;
    private bool isGrounded;
    private PlayerInputActions InputActions;
    private Vector2 moveInput;
    private float rotateInput;

    private void Awake()
    {
        characterController = GetComponent<CharacterController>();
        InputActions = new PlayerInputActions();
        InputActions.Player.Move.performed += ctx => moveInput = ctx.ReadValue<Vector2>();
        InputActions.Player.Move.canceled += ctx => moveInput = Vector2.zero;

        InputActions.Player.Run.performed += ctx => isRunning = true;
        InputActions.Player.Run.canceled += ctx => isRunning = false;

        InputActions.Player.Jump.performed += ctx => Jump();

        InputActions.Player.Rotate.performed += ctx => rotateInput = ctx.ReadValue<float>();
        InputActions.Player.Rotate.canceled += ctx => rotateInput = 0.0f;
    }

    private void OnEnable()
    {
        InputActions.Enable();

    }

    private void OnDisable()
    {
        InputActions.Disable();
    }

    void Jump()
    {
        if (isGrounded) {
            velocity.y = Mathf.Sqrt(jumpHeight * -2.0f * gravity);
        }
    }

    // Update is called once per frame
    void Update()
    {
        isGrounded = characterController.isGrounded;
        if (isGrounded && velocity.y < 0)
        {
            velocity.y = -2.0f;
        }

        Vector3 move = new Vector3(moveInput.x, 0, moveInput.y);
        move = transform.TransformDirection(move);
        float currentSpeed = isRunning ? speed * runMultiplier : speed;
        characterController.Move(move * currentSpeed * Time.deltaTime);
        float rotation = rotateInput * rotationSpeed * Time.deltaTime;
        transform.Rotate(0, rotation, 0);
        velocity.y += gravity * Time.deltaTime;
        characterController.Move(velocity * Time.deltaTime);
    }
}
