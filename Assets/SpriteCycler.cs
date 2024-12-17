using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpriteCycler : MonoBehaviour
{

    public SpriteRenderer spriteRenderer; // Drag the SpriteRenderer here in the inspector
    public Texture2D spriteSheet; // The sliced sprite sheet texture
    public int totalRows = 14; // Total number of rows in the sprite sheet
    public float cycleSpeed = 1f; // Time in seconds to wait before cycling to the next sprite
    private float timer = 0f;
    private int currentIndex = 0;
    private Sprite[] sprites; // Array of sliced sprites from the sprite sheet

    void Start()
    {
        if (spriteSheet != null)
        {
            sprites = GetSlicedSprites(spriteSheet);
            if (sprites.Length > 0)
            {
                spriteRenderer.sprite = sprites[0]; // Set the first sprite
            }
        }
    }

    void Update()
    {
        if (sprites.Length == 0) return;

        timer += Time.deltaTime;

        if (timer >= cycleSpeed)
        {
            // Reset the timer and cycle to the next sprite
            timer = 0f;
            currentIndex = (currentIndex + 1) % sprites.Length; // Loop through the sprites
            spriteRenderer.sprite = sprites[currentIndex];
        }
    }

    // Automatically slice the sprite sheet into individual sprites
    Sprite[] GetSlicedSprites(Texture2D texture)
    {
        float spriteHeight = texture.height / totalRows;
        float spriteWidth = texture.width; // Full width of the sprite sheet

        Sprite[] slicedSprites = new Sprite[totalRows];

        for (int i = 0; i < totalRows; i++)
        {
            // Calculate the Rect for each sprite
            Rect spriteRect = new Rect(0, i * spriteHeight, spriteWidth, spriteHeight);
            slicedSprites[i] = Sprite.Create(texture, spriteRect, new Vector2(0.5f, 0.5f));
        }

        return slicedSprites;
    }
}