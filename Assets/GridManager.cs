using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridManager : MonoBehaviour
{
    public static GridManager instance;

    [SerializeField] private int gridWidth = 2;
    [SerializeField] private int gridHeight = 7;  // This still represents the depth (z-axis)
    [SerializeField] private GameObject tilePrefab;
    [SerializeField] private Vector3 gridOrigin = new Vector3(0, 0, 0);  // Updated to Vector3 for 3D
    [SerializeField] private float tileSpacing = 1.0f;

    // Dictionary to track occupied positions
    private Dictionary<Vector3, GameObject> occupiedTiles = new Dictionary<Vector3, GameObject>();  // Updated to use Vector3

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void Start()
    {
        GenerateGrid();
    }

    private void GenerateGrid()
{
    for (int x = 0; x < gridWidth; x++)
    {
        for (int z = 0; z < gridHeight; z++)  // Use z instead of y for 3D grid
        {
            Vector3 tilePosition = new Vector3(gridOrigin.x + x * tileSpacing, gridOrigin.y, gridOrigin.z + z * tileSpacing);
            
            // Rotate the tile to face upwards (90 degrees around the X-axis)
            Quaternion tileRotation = Quaternion.Euler(90, 0, 0);
            
            Instantiate(tilePrefab, tilePosition, tileRotation);
        }
    }
}

    // Check if a position is occupied
    public bool IsTileOccupied(Vector3 position)  // Updated to use Vector3
    {
        return occupiedTiles.ContainsKey(position);
    }

    // Mark a position as occupied
    public void OccupyTile(Vector3 position, GameObject occupier)  // Updated to use Vector3
    {
        if (!occupiedTiles.ContainsKey(position))
        {
            occupiedTiles.Add(position, occupier);
        }
    }

    // Free a tile when a character moves away
    public void FreeTile(Vector3 position)  // Updated to use Vector3
    {
        if (occupiedTiles.ContainsKey(position))
        {
            occupiedTiles.Remove(position);
        }
    }

    // Get grid width
    public int GetGridWidth()
    {
        return gridWidth;
    }

    // Get grid height
    public int GetGridHeight()
    {
        return gridHeight;
    }
}
