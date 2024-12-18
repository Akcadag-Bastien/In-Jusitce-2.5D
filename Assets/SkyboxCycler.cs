using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkyboxCycler : MonoBehaviour
{
    public Material skyboxMaterial; // Assign your skybox material here
    public Texture2D spriteSheet;   // The vertical spritesheet texture
    public int totalRows = 7;      // Total number of rows in the spritesheet
    public float cycleSpeed = 1f;   // Time in seconds before cycling to the next frame

    private float timer = 0f;       // Timer to control frame changes
    private int currentIndex = 0;   // Current frame index
    private Vector2 frameSize;      // Size of each frame in UV coordinates

    void Start()
    {
        if (spriteSheet != null && skyboxMaterial != null)
        {
            // Calculate the size of each frame
            frameSize = new Vector2(1f, 1f / totalRows);

            // Set the initial texture scale
            skyboxMaterial.SetTextureScale("_MainTex", frameSize);

            // Set the initial frame
            SetFrame(0);
        }
    }

    void Update()
    {
        if (spriteSheet == null || skyboxMaterial == null) return;

        timer += Time.deltaTime;

        if (timer >= cycleSpeed)
        {
            // Reset the timer and cycle to the next frame
            timer = 0f;
            currentIndex = (currentIndex + 1) % totalRows;
            SetFrame(currentIndex);
        }
    }

    void SetFrame(int frameIndex)
    {
        // Calculate the vertical offset for the current frame
        float offsetY = 1f - (frameIndex + 1) * frameSize.y;
        Vector2 offset = new Vector2(0f, offsetY);

        // Apply the offset to the material
        skyboxMaterial.SetTextureOffset("_MainTex", offset);
    }
}