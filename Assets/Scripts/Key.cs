using UnityEngine;

public class Key : MonoBehaviour
{
    public string keyID; // Unique ID for the key

    public void PickUp()
    {
        Debug.Log($"Picked up key: {keyID}");
        // Add the key to the player's inventory
        KeyInventory.Instance.AddKey(keyID);
        Destroy(gameObject); // Remove the key from the scene
    }
}
