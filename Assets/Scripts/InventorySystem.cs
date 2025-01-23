using System.Collections.Generic;
using UnityEngine;

public class KeyInventory : MonoBehaviour
{
    public static KeyInventory Instance;

    private List<string> keys = new List<string>();

    private void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);
    }

    public void AddKey(string keyID)
    {
        if (!keys.Contains(keyID))
        {
            keys.Add(keyID);
            Debug.Log($"Key {keyID} added to inventory.");
        }
    }

    public bool HasKey(string keyID)
    {
        return keys.Contains(keyID);
    }
}
