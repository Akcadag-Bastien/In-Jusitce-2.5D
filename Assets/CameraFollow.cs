using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{

    public float zoom = 1;

    public Transform target; // The character the camera follows
    public float smoothSpeed = 0.125f; // Speed for smooth camera movement
    private Vector3 offset => new Vector3(zoom, zoom, -zoom); // Offset to maintain distance from the target

    // Update is called once per frame
    public void FollowTarget()
    {
        if (target != null)
        {
            // Desired position is the target position plus the offset
            Vector3 desiredPosition = target.position + offset;

            // Smooth transition between the current and desired positions
            Vector3 smoothedPosition = Vector3.Lerp(transform.position, desiredPosition, smoothSpeed);

            // Update the camera's position
            transform.position = smoothedPosition;
        }
    }

    // Method to set the target (the moving character)
    public void SetTarget(Transform newTarget)
    {
        target = newTarget;
    }
}
