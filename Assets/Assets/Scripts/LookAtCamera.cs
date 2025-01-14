using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LookAtCamera : MonoBehaviour
{
    private Camera mainCamera;

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
            // Align the sprite's forward direction with the opposite of the camera's forward vector
            transform.forward = -mainCamera.transform.forward;
        }
    }
}
