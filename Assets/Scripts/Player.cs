using System.Collections;
using UnityEngine;

public class FirstPersonController : MonoBehaviour
{
    [Header("Player Settings")]
    public float walkSpeed = 3.0f;
    public float sprintSpeed = 6.0f;
    public float crouchSpeed = 1.5f;
    public float jumpForce = 5.0f;
    public float crouchHeight = 0.5f;


    [Header("Look Settings")]
    public Camera camera;
    public float lookSensitivity = 2.0f;
    public float maxLookX = 60.0f;
    public float minLookX = -60.0f;

    [Header("Flashlight Settings")]
    public Light flashlight;
    public KeyCode flashlightKey = KeyCode.F;

    [Header("Interaction Settings")]
    public float interactionDistance = 2.0f;
    public KeyCode interactKey = KeyCode.E;

    [Header("Inventory")]
    public string playerKeyID = ""; // Stores the player's current key ID
    public KeyCode InventoryKey = KeyCode.Tab;


    private float moveSpeed;
    private bool isCrouching = false;
    private float defaultHeight;

    private float rotX = 0.0f;
    private Rigidbody rb;
    private CapsuleCollider capsuleCollider;

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
        capsuleCollider = GetComponent<CapsuleCollider>();

        // Lock the cursor to the game screen
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

        // Store default height for crouching
        defaultHeight = capsuleCollider.height;

        moveSpeed = walkSpeed;

        // Ensure camera is positioned correctly relative to player
        if (camera != null)
        {
            camera.transform.position = transform.position + new Vector3(0, capsuleCollider.height / 2, 0);
        }
    }

    private void Update()
    {
        Movement();
        CameraLook();
        HandleFlashlight();
        HandleCrouch();
        HandleInteraction();
        ShowInventory();
    }

    private void Movement()
    {
        float x = Input.GetAxis("Horizontal");
        float z = Input.GetAxis("Vertical");

        Vector3 moveDirection = transform.right * x + transform.forward * z;
        Vector3 velocity = moveDirection * moveSpeed;
        velocity.y = rb.velocity.y;

        rb.velocity = velocity;

        // Sprinting
        if (Input.GetKey(KeyCode.LeftShift) && !isCrouching)
        {
            moveSpeed = sprintSpeed;
        }
        else
        {
            moveSpeed = isCrouching ? crouchSpeed : walkSpeed;
        }

        // Jumping
        if (Input.GetKeyDown(KeyCode.Space) && IsGrounded())
        {
            rb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
        }
    }

    private void CameraLook()
    {
        float mouseX = Input.GetAxis("Mouse X") * lookSensitivity;
        float mouseY = Input.GetAxis("Mouse Y") * lookSensitivity;

        rotX -= mouseY;
        rotX = Mathf.Clamp(rotX, minLookX, maxLookX);

        camera.transform.localRotation = Quaternion.Euler(rotX, 0, 0);
        transform.rotation *= Quaternion.Euler(0, mouseX, 0);

        // Ensure camera follows the player's position
        if (camera != null)
        {
            camera.transform.position = transform.position + new Vector3(0, capsuleCollider.height / 2, 0);
        }
    }

    private void HandleFlashlight()
    {
        if (flashlight != null && Input.GetKeyDown(flashlightKey))
        {
            flashlight.enabled = !flashlight.enabled;
        }
    }

    private void HandleCrouch()
    {
        if (Input.GetKeyDown(KeyCode.C))
        {
            isCrouching = !isCrouching;
            capsuleCollider.height = isCrouching ? crouchHeight : defaultHeight;
            moveSpeed = isCrouching ? crouchSpeed : walkSpeed;

            // Adjust camera position during crouch
            if (camera != null)
            {
                camera.transform.position = transform.position + new Vector3(0, capsuleCollider.height / 2, 0);
            }
        }
    }

    private void HandleInteraction()
    {
        if (Input.GetKeyDown(interactKey))
        {
            Ray ray = new Ray(camera.transform.position, camera.transform.forward);
            RaycastHit hit;

            // Cast a ray to detect objects in front of the player
            if (Physics.Raycast(ray, out hit, interactionDistance))
            {
                // Check for objects with specific tags
                if (hit.collider.CompareTag("Door"))
                {
                    Debug.Log("Interacted with a Door");
                    Door door = hit.collider.GetComponent<Door>();
                    if (door != null)
                    {
                        // Pass the player's key ID to the door script
                        door.ToggleDoor(playerKeyID);
                    }
                }
                else if (hit.collider.CompareTag("Key"))
                {
                    Debug.Log("Picked up a Key");
                    Key key = hit.collider.GetComponent<Key>();
                    if (key != null)
                    {
                        // Assign the key ID to the player
                        playerKeyID = key.keyID;
                        key.PickUp();
                        Debug.Log("Key acquired: " + playerKeyID);
                    }
                }
                else
                {
                    Debug.Log("No valid interaction found.");
                }
            }
        }
    }

    private void ShowInventory()
    {
        if (Input.GetKeyDown(InventoryKey))
        {
            if (playerKeyID == "")
            {
                Debug.Log("Player Inventory is empty.");
                return;
            }
            else
            {
                Debug.Log("Player Inventory: " + playerKeyID);
            }
        }
    }
    private bool IsGrounded()
    {
        return Physics.Raycast(transform.position, Vector3.down, capsuleCollider.bounds.extents.y + 0.1f);
    }
}
