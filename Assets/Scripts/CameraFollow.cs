using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    [SerializeField] private Transform target;      // The character/player to follow
    [SerializeField] private Vector3 offset = new Vector3(0f, 2.5f, -6f);  // Default offset
    [SerializeField] private float smoothSpeed = 0.125f;  // The speed of the camera's smooth movement

    private void LateUpdate()
    {
        if (target == null) return;

        // Calculate the desired position
        Vector3 desiredPosition = target.position + offset;

        // Smoothly interpolate between current and desired position
        Vector3 smoothedPosition = Vector3.Lerp(transform.position, desiredPosition, smoothSpeed);

        // Update camera position
        transform.position = smoothedPosition;
        // No need to update rotation — it will stay the same unless you change it elsewhere
    }
}
