using UnityEngine;

public class FlashlightLag : MonoBehaviour
{
    public Transform target; // The camera to follow
    public float lagSpeed = 5.0f;

    private void Update()
    {
        if (target != null)
        {
            // Smoothly follow the target's position and rotation
            transform.position = Vector3.Lerp(transform.position, target.position, Time.deltaTime * lagSpeed);
            transform.rotation = Quaternion.Slerp(transform.rotation, target.rotation, Time.deltaTime * lagSpeed);
        }
    }
}
