using NUnit.Framework.Constraints;
using UnityEngine;
using UnityEngine.InputSystem;

public class ThirdPersonMovement : MonoBehaviour
{
    [SerializeField] CharacterController controller;

    [SerializeField] float playerSpeed;

    [SerializeField] Transform playerCamera;

    Vector2 moveInput;

    [SerializeField] InputActionReference moveAction;

    [SerializeField] float gravityValue;

    public float turnSmoothTime = .1f;
    float turnSmoothVelocity;

    private Vector3 playerVelocity;


    private void Start()
    {

    }


    private void Update()
    {

        if (controller.isGrounded)
        {
            // Slight downward velocity to keep grounded stable
            if (playerVelocity.y < -2f)
                playerVelocity.y = -2f;
        }

        // Apply gravity
        playerVelocity.y += gravityValue * Time.deltaTime;
        moveInput = moveAction.action.ReadValue<Vector2>();

        float horizontal = moveInput.x;
        float vertical = moveInput.y;

        Vector3 direction = new Vector3(horizontal, 0f, vertical).normalized;

        if (direction.magnitude >= .1f)
        {
            float targetAngle = Mathf.Atan2(direction.x, direction.z) * Mathf.Rad2Deg + playerCamera.eulerAngles.y;
            float angle = Mathf.SmoothDampAngle(transform.eulerAngles.y, targetAngle, ref turnSmoothVelocity, turnSmoothTime);
            transform.rotation = Quaternion.Euler(0f, angle, 0f);


            Vector3 moveDireciton = Quaternion.Euler(0, targetAngle, 0) * Vector3.forward;
            controller.Move(moveDireciton.normalized * playerSpeed * Time.deltaTime);

        }

        controller.Move(playerSpeed * (Vector3.up * playerVelocity.y) * Time.deltaTime);

    }


}
