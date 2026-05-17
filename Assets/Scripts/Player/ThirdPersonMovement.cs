using NUnit.Framework.Constraints;
using System.Security.Principal;
using Unity.Cinemachine;
using UnityEngine;
using UnityEngine.InputSystem;

public class ThirdPersonMovement : MonoBehaviour
{
    [SerializeField] private CharacterController controller;

    [SerializeField] private Transform playerCamera;

    [SerializeField] CinemachineCamera cinemachineCamera;

    [SerializeField] CinemachineStateDrivenCamera lockOnCam;

    [Header("Input Actions")]
    [SerializeField] private InputActionReference targetLockOnAction;
    [SerializeField] private InputActionReference moveAction;


    [Header("Physics Values")]
    [SerializeField] private float playerSpeed;

    [SerializeField] private float gravityValue;

    [SerializeField] private float turnSmoothTime = .1f;

    [Header("Fov Variables")]
    [SerializeField] private float playerFov;
    [SerializeField, Range(0f, 360f)] private float fovAngle;

    private Vector3 playerVelocity;

    private Vector2 moveInput;

    private float turnSmoothVelocity;

    private BaseEnemy currTarget;

    private void Start()
    {
        targetLockOnAction.action.performed += LockOn;
        Cursor.lockState = CursorLockMode.Locked;

        
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



            Vector3 moveDireciton;

            if (currTarget == null)
            {
                float angle = Mathf.SmoothDampAngle(transform.eulerAngles.y, targetAngle, ref turnSmoothVelocity, turnSmoothTime);
                transform.rotation = Quaternion.Euler(0f, angle, 0f);
                moveDireciton = Quaternion.Euler(0, targetAngle, 0) * Vector3.forward;
            }
            else
            {
                transform.LookAt(currTarget.transform.position);
                isEnemyInRange();
                moveDireciton = Quaternion.Euler(0, targetAngle, 0) * Vector3.forward;
            }

            controller.Move(moveDireciton.normalized * playerSpeed * Time.deltaTime);

        }

        controller.Move(playerSpeed * (Vector3.up * playerVelocity.y) * Time.deltaTime);




    }

    /// <summary>
    /// Switches where the camera's main focus while still following the player
    /// </summary>
    private void LockOn(InputAction.CallbackContext callback)
    {
        currTarget = FindEnemies();
        

        if (currTarget != null)
        {
           
            EnableLockOnCamera();
            
        }
        else
        {
           
            EnableFreeCamera();
           
        }
    }

    /// <summary>
    /// Grabs all available enemies in the players view
    /// </summary>
    private BaseEnemy FindEnemies()
    {
        BaseEnemy bestEnemy = null;
        float bestEnemyDistance = float.MinValue;

        Collider[] enemiesInRange = Physics.OverlapSphere(transform.position, playerFov);


        foreach (Collider c in enemiesInRange)
        {
            float signedAngle = Vector3.Angle(transform.forward, c.transform.position - transform.position);
            if (!(Mathf.Abs(signedAngle) < fovAngle / 2)) continue;

            if (c.TryGetComponent<BaseEnemy>(out BaseEnemy enemy))
            {

                if (!IsEnemyInView(c)) continue;

                float distance = Vector3.Distance(transform.position, enemy.transform.position);

                if (bestEnemy == null)
                {
                    bestEnemy = enemy;
                    bestEnemyDistance = distance;
                }
                else if (distance < bestEnemyDistance)
                {
                    bestEnemy = enemy;
                    bestEnemyDistance = distance;
                }


            }
        }

        return bestEnemy;
    }
    private void EnableLockOnCamera()
    {
        CameraTarget cameraTarget = default;

        cameraTarget.TrackingTarget = transform;
        cinemachineCamera.Target = cameraTarget;
        cinemachineCamera.LookAt = currTarget.transform;

        lockOnCam.Priority = 20;
        cinemachineCamera.Priority = 10;
    }

    private void EnableFreeCamera()
    {
        CameraTarget cameraTarget = default;

        cameraTarget.TrackingTarget = transform;
        cinemachineCamera.Target = cameraTarget;

        cinemachineCamera.ForceCameraPosition(
            lockOnCam.transform.position,
            lockOnCam.transform.rotation
        );

        lockOnCam.Priority = 10;
        cinemachineCamera.Priority = 20;
    }

    private bool IsEnemyInView(Collider enemy)
    {
        RaycastHit hit;
        Vector3 raycastTargetPosition = new Vector3(0, .25f, 0) + enemy.transform.position;

        Physics.Raycast(transform.position, enemy.transform.position - transform.position, out hit);

        Debug.DrawLine(transform.position, raycastTargetPosition, Color.red, 3f);

        if (hit.collider.transform.gameObject == enemy.gameObject)
        {
            return true;
        }

        return false;
    }

    /// <summary>
    /// If the player is locked on to an enemy check to see if they are still in range, if not switch to free cam
    /// </summary>
    private void isEnemyInRange()
    {
        float distance = Vector3.Distance(transform.position,currTarget.transform.position);
        if(distance > playerFov)
        {
            EnableFreeCamera();
            currTarget = null;
        }
    }
}
