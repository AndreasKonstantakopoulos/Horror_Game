using UnityEngine;

public class ExitDoor : MonoBehaviour
{
    [Header("Door Settings")]
    public string requiredKeyID = "MainDoor"; // The key ID required to exit
    public string playerTag = "Player"; // Tag to identify the player
    public Transform exitPosition; // Position where the player is teleported after exiting

    [Header("Messages")]
    public string noKeyMessage = "You need the Main Door key to exit!";
    public string exitMessage = "You have escaped the asylum!";

    private bool isPlayerNearby = false;

    private void Update()
    {
        // Check for player interaction when nearby
        if (isPlayerNearby && Input.GetKeyDown(KeyCode.E))
        {
            AttemptExit();
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag(playerTag))
        {
            isPlayerNearby = true;
            Debug.Log("Press 'E' to attempt to exit.");
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag(playerTag))
        {
            isPlayerNearby = false;
        }
    }

    private void AttemptExit()
    {
        FirstPersonController playerController = FindObjectOfType<FirstPersonController>();
        if (playerController != null && playerController.playerKeyID == requiredKeyID)
        {
            // Player has the correct key
            Debug.Log(exitMessage);
            ExitBuilding(playerController);
        }
        else
        {
            // Player doesn't have the key
            Debug.Log(noKeyMessage);
        }
    }

    private void ExitBuilding(FirstPersonController player)
    {
        if (exitPosition != null)
        {
            // Teleport the player to the exit position
            player.transform.position = exitPosition.position;
        }

        // Alternatively, you can trigger a win screen, end the game, or load another scene here
        Debug.Log("Game End: Player has exited the asylum.");
    }
}
