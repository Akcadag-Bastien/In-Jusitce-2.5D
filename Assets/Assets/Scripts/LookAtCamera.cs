using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LookAtCamera : MonoBehaviour
{
    [SerializeField] private Camera mainCamera;

    // Start is called before the first frame update
    void Start()
    {
        mainCamera = Camera.main; // Automatically gets the main camera
    }

    // Update is called once per frame
    void Update()
    {
        if (mainCamera != null)
        {
            // Face the actual camera position so sprites subtly swivel as the camera moves
            Vector3 directionToCamera = mainCamera.transform.position - transform.position;
            if (directionToCamera.sqrMagnitude > 0.0001f)
            {
                transform.forward = directionToCamera.normalized;
            }
        }
    }
}
