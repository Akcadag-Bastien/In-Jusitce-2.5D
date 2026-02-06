using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI; // Needed to detect the Button variable as valid
using UnityEngine.EventSystems;

public class CameraCycle : MonoBehaviour
{

    public List<Camera> cameras = new List<Camera>();
    public int currentCamera = 0;

    // Start is called before the first frame update
    void Start()
    {

        foreach (Camera cam in cameras)
        {
            cam.enabled = false;
        }

        if (cameras.Count > 0)
        {
            cameras[currentCamera].enabled = true;
        }
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(0) && !EventSystem.current.IsPointerOverGameObject())
        {
            // Disable current camera
            cameras[currentCamera].enabled = false;

            // Move to next camera and loop
            currentCamera = (currentCamera + 1) % cameras.Count;

            // Enable new camera
            cameras[currentCamera].enabled = true;

            Debug.Log("Switched to: " + cameras[currentCamera].name);
        }
    }
}
