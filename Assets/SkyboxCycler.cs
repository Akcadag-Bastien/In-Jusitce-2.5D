using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkyboxCycler : MonoBehaviour
{
    public Texture2D spriteSheet;   // The vertical spritesheet texture
    public int totalRows = 14;      // Total number of rows in the spritesheet
    public float cycleSpeed = 1f;   // Time in seconds before cycling to the next frame

    private Material skyboxMaterial; // Active skybox material
    private float timer = 0f;        // Timer to control frame changes
    private int currentIndex = 0;    // Current frame index
    private Vector2 frameSize;       // Size of each frame in UV coordinates

    void Start()
    {
        // Get the active skybox material
        skyboxMaterial = RenderSettings.skybox;

        // Ensure the spritesheet and material are valid
        if (spriteSheet != null && skyboxMaterial != null)
        {
            // Check if the material supports _Tex
            if (!skyboxMaterial.HasProperty("_Tex"))
            {
                Debug.LogError("Skybox material does not support _Tex. Ensure you are using the Skybox/Panoramic shader.");
                return;
            }

            // Calculate the size of each frame
            frameSize = new Vector2(1f, 1f / totalRows);

            // Set the initial texture scale
            skyboxMaterial.SetTextureScale("_Tex", frameSize);

            // Set the first frame
            SetFrame(0);
        }
        else
        {
            Debug.LogError("Skybox Material or SpriteSheet is not set!");
        }
    }

    void Update()
    {
        if (skyboxMaterial == null || spriteSheet == null) return;

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

        // Apply the offset to the skybox material
        skyboxMaterial.SetTextureOffset("_Tex", offset);

        // Debug to confirm the frame changes
        Debug.Log($"Frame Index: {frameIndex}, Offset: {offset}");
    }
}
