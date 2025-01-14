using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkyboxCycler : MonoBehaviour
{
    public Material skyboxMaterial; // Assign your skybox material here
    public int frameCount = 8;      // Total number of frames
    public float frameDuration = 0.1f; // Duration of each frame

    private int currentFrame;
    private float timer;

    void Update()
    {
        if (!skyboxMaterial) return;

        // Update the timer
        timer += Time.deltaTime;

        // If the timer exceeds the frame duration, switch to the next frame
        if (timer >= frameDuration)
        {
            timer -= frameDuration;
            currentFrame = (currentFrame + 1) % frameCount;

            // Calculate the offset for the current frame
            float offsetX = (float)currentFrame / frameCount;

            // Set the offset on the skybox material
            skyboxMaterial.SetTextureOffset("_MainTex", new Vector2(offsetX, 0));
        }
    }
}