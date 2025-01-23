using UnityEngine;

public class Door : MonoBehaviour
{
    [Header("Door Settings")]
    public bool requiresKey = false;  // If true, the door needs a key to open
    public string requiredKeyID;      // The key ID needed to unlock this door
    public bool isOpen = false;       // Tracks the current state of the door
    public Transform doorHinge;       // The hinge or pivot point for the door
    public float openAngle = 90f;     // The angle the door opens to
    public float openSpeed = 2f;      // Speed of the door's opening/closing

    private Quaternion closedRotation;
    private Quaternion openRotation;

    private void Start()
    {
        // Set the closed and open rotations of the door
        if (doorHinge != null)
        {
            closedRotation = doorHinge.localRotation;
            openRotation = Quaternion.Euler(doorHinge.localRotation.eulerAngles + new Vector3(0, openAngle, 0));
        }
    }

    public void ToggleDoor(string playerKeyID)
    {
        // Check if the door requires a key
        if (requiresKey)
        {
            // If the player has the correct key, open the door
            if (playerKeyID == requiredKeyID)
            {
                Debug.Log("Door unlocked with the key: " + playerKeyID);
                isOpen = !isOpen; // Toggle door state
                StopAllCoroutines();
                StartCoroutine(AnimateDoor());
            }
            else
            {
                Debug.Log("This door is locked. Requires key: " + requiredKeyID);
            }
        }
        else
        {
            // If the door doesn't require a key, toggle its state
            Debug.Log("This door doesn't require a key. Opening/closing.");
            isOpen = !isOpen; // Toggle door state
            StopAllCoroutines();
            StartCoroutine(AnimateDoor());
        }
    }

    private System.Collections.IEnumerator AnimateDoor()
    {
        // Smoothly animate the door between open and closed states
        Quaternion targetRotation = isOpen ? openRotation : closedRotation;

        while (Quaternion.Angle(doorHinge.localRotation, targetRotation) > 0.1f)
        {
            doorHinge.localRotation = Quaternion.Slerp(doorHinge.localRotation, targetRotation, Time.deltaTime * openSpeed);
            yield return null;
        }

        doorHinge.localRotation = targetRotation; // Snap to final position
    }
}
