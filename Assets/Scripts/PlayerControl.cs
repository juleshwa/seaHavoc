using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.EnhancedTouch;
using Touch = UnityEngine.InputSystem.EnhancedTouch.Touch;
using TouchPhase = UnityEngine.InputSystem.TouchPhase;

public class PlayerControl : MonoBehaviour
{
    private Camera mainCamera;
    private Vector3 offset;
    private bool isTouching = false;

    [SerializeField] private InputActionReference moveActiontoUse;
    [SerializeField] private float speed = 5f;

    private Vector2 moveInput;

    // Add Animator reference
    private Animator animator;

    public GameObject bulletPrefab;
    public Transform firePoint;
    public float bulletSpeed = 20f;

    private void OnEnable()
    {
        EnhancedTouchSupport.Enable();
        moveActiontoUse.action.Enable();
        moveActiontoUse.action.performed += OnMovePerformed;
        moveActiontoUse.action.canceled += OnMoveCanceled;
    }

    private void OnDisable()
    {
        EnhancedTouchSupport.Disable();
        moveActiontoUse.action.performed -= OnMovePerformed;
        moveActiontoUse.action.canceled -= OnMoveCanceled;
        moveActiontoUse.action.Disable();
    }

    void Start()
    {
        mainCamera = Camera.main;
        animator = GetComponent<Animator>();

        if (transform.position.z < mainCamera.transform.position.z)
        {
            Debug.LogWarning("Sprite berada di belakang kamera, memindahkan ke depan.");
            transform.position = new Vector3(transform.position.x, transform.position.y, mainCamera.transform.position.z + 1);
        }
    }

    void Update()
    {
        if (isTouching)
        {
            HandleTouchInput();
        }
        else
        {
            HandleMovement();
        }

        UpdateAnimationParameters();
    }

    private void HandleMovement()
    {
        // Handle movement based on keyboard or gamepad input
        Vector3 moveDirection = new Vector3(moveInput.x, moveInput.y, 0);
        transform.position += moveDirection * speed * Time.deltaTime;
    }

    private void HandleTouchInput()
    {
        
        if (Touch.activeTouches.Count > 0)
        {
            Touch touch = Touch.activeTouches[0];

            if (touch.phase == TouchPhase.Moved || touch.phase == TouchPhase.Stationary)
            {
                Vector3 touchPosition = touch.screenPosition;
                touchPosition.z = mainCamera.nearClipPlane; 
                Vector3 worldPosition = mainCamera.ScreenToWorldPoint(touchPosition);
                Vector3 direction = (worldPosition - transform.position).normalized;
                transform.position += direction * speed * Time.deltaTime;
                isTouching = true;
                moveInput = new Vector2(direction.x, direction.y);
            }
        }
        else
        {
            isTouching = false;
        }
    }

    private void OnMovePerformed(InputAction.CallbackContext context)
    {
        
        moveInput = context.ReadValue<Vector2>();
    }

    private void OnMoveCanceled(InputAction.CallbackContext context)
    {
       
        moveInput = Vector2.zero;
    }

    private void UpdateAnimationParameters() {
        if (animator != null)
        {
            animator.SetBool("isLeft", moveInput.x < 0 && Mathf.Abs(moveInput.x) > Mathf.Abs(moveInput.y));
            animator.SetBool("isRight", moveInput.x > 0 && Mathf.Abs(moveInput.x) > Mathf.Abs(moveInput.y));
            animator.SetBool("isUp", moveInput.y > 0 && Mathf.Abs(moveInput.y) > Mathf.Abs(moveInput.x));
            animator.SetBool("isDown", moveInput.y < 0 && Mathf.Abs(moveInput.y) > Mathf.Abs(moveInput.x));

            animator.SetBool("isUpRight", moveInput.x > 0 && moveInput.y > 0);
            animator.SetBool("isUpLeft", moveInput.x < 0 && moveInput.y > 0);
            animator.SetBool("isDownRight", moveInput.x > 0 && moveInput.y < 0);
            animator.SetBool("isDownLeft", moveInput.x < 0 && moveInput.y < 0);
        }
    }

    public void Shoot()
    {
        Debug.Log("Shoot");

    GameObject bullet = Instantiate(bulletPrefab, firePoint.position, firePoint.rotation);

    Rigidbody2D rb = bullet.GetComponent<Rigidbody2D>();
    if (rb != null)
    {
        rb.velocity = firePoint.up * bulletSpeed; // Fire bullet upwards in 2D space.
    }
    else
    {
        Debug.LogError("Bullet prefab is missing a Rigidbody2D component!");
    }

    Destroy(bullet, 2f); // Clean up after 2 seconds.
    }


}
