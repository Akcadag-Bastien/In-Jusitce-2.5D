using UnityEngine;

public class OverworldGroundGenerator : MonoBehaviour
{
    [SerializeField] private GameObject tilePrefab;
    [SerializeField] private int width = 10;
    [SerializeField] private int length = 10;

    private void Awake()
    {
        if (tilePrefab == null)
        {
            Debug.LogWarning($"{nameof(OverworldGroundGenerator)} is missing a tile prefab reference.");
            return;
        }

        Generate();
    }

    private void Generate()
    {
        for (int x = 0; x < width; x++)
        {
            for (int z = 0; z < length; z++)
            {
                Vector3 spawnPos = new Vector3(x, 0f, z);
                Instantiate(tilePrefab, spawnPos, Quaternion.identity * Quaternion.Euler(new Vector3(90, 0, 0)), transform);
            }
        }
    }
}
