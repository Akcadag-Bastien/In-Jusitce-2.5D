using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    [Header("Follow Settings")]
    public float zoom = 1f;
    public Transform target;
    [Tooltip("Time (in seconds) the camera takes to settle on its target.")]
    [SerializeField] private float smoothTime = 0.2f;
    [Tooltip("Absolute cap on how fast the camera can move (units per second).")]
    [SerializeField] private float maxFollowSpeed = 25f;
    [Tooltip("If the camera is ever farther than this from the desired position, it snaps immediately.")]
    [SerializeField] private float snapDistance = 12f;

    [Tooltip("How close (in units) the camera must be to consider itself caught up.")]
    [SerializeField] private float alignmentThreshold = 0.05f;

    private Vector3 offset => new Vector3(zoom, zoom, -zoom);
    private Vector3 followVelocity;
    private int lastAppliedFrame = -1;
    private Vector3 desiredPosition;

    private void Start()
    {
        transform.position = new Vector3(2, 4, -3);
    }

    private void LateUpdate()
    {
        ApplyFollow(Time.frameCount, Time.deltaTime);
    }

    public void FollowTarget()
    {
        ApplyFollow(Time.frameCount, Time.deltaTime);
    }

    private void ApplyFollow(int frame, float deltaTime)
    {
        if (frame == lastAppliedFrame || target == null)
        {
            return;
        }

        lastAppliedFrame = frame;

        desiredPosition = target.position + offset;
        float distanceToTarget = Vector3.Distance(transform.position, desiredPosition);

        if (distanceToTarget >= snapDistance)
        {
            transform.position = desiredPosition;
            followVelocity = Vector3.zero;
            return;
        }

        float clampedSmoothTime = Mathf.Max(0.0001f, smoothTime);
        transform.position = Vector3.SmoothDamp(
            transform.position,
            desiredPosition,
            ref followVelocity,
            clampedSmoothTime,
            maxFollowSpeed,
            deltaTime);
    }

    public void SetTarget(Transform newTarget)
    {
        target = newTarget;
    }

    public bool HasArrived
    {
        get
        {
            if (target == null)
            {
                return true;
            }

            float distance = Vector3.Distance(transform.position, target.position + offset);
            return distance <= alignmentThreshold;
        }
    }

    public float DistanceToTarget => target == null ? 0f : Vector3.Distance(transform.position, target.position + offset);

    public static CameraFollow GetActiveCameraFollow()
    {
        Camera mainCamera = Camera.main;
        if (mainCamera != null)
        {
            CameraFollow follow = mainCamera.GetComponent<CameraFollow>();
            if (follow != null)
            {
                return follow;
            }
        }

        return FindObjectOfType<CameraFollow>();
    }
}
