using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridManager : MonoBehaviour
{

    // Create a new class to circumvent the single-type array limitation of C#

    [System.Serializable]

    public class TileData
    {
        public GameObject prefab;
        public TileCondition condition; // Use enum instead of bool
    }

    public enum TileCondition // Add conditions for custom tiles to spawn
    {
        ZEqualsZero,
        ZEqualsLength,
        Regular,
    }

    public List<TileData> tiles = new List<TileData>();

    //

    public static GridManager instance;

    [SerializeField] private int gridWidth = 2; // z-axis
    [SerializeField] private int gridHeight = 7; // z-axis
    [SerializeField] private GameObject tilePrefab;
    [SerializeField] private Vector3 gridOrigin = new Vector3(0, 0, 0); // Updated to Vector3 for 3D
    [SerializeField] private float tileSpacing = 1.0f;


    // Dictionary to track occupied positions
    private Dictionary<Vector3, GameObject> occupiedTiles = new Dictionary<Vector3, GameObject>(); // Updated to use Vector3
    private readonly List<GameObject> spawnedTiles = new List<GameObject>();

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
            for (int z = 0; z < gridHeight; z++) 
            {
                Vector3 tilePosition = new Vector3(gridOrigin.x + x * tileSpacing, gridOrigin.y, gridOrigin.z + z * tileSpacing);
                Quaternion tileRotation = Quaternion.Euler(90, 0, 0);

                foreach (var tile in tiles)
                {
                    if (EvaluateCondition(tile.condition, z))
                    {
                        var spawnedTile = Instantiate(tile.prefab, tilePosition, tileRotation);
                        spawnedTiles.Add(spawnedTile);
                        break;
                    }
                }
            }
        }
    }

    private void ClearGrid()
    {
        foreach (var tile in spawnedTiles)
        {
            if (tile != null)
            {
                Destroy(tile);
            }
        }

        spawnedTiles.Clear();
        occupiedTiles.Clear();
    }

    public void RebuildGrid()
    {
        ClearGrid();
        GenerateGrid();
    }

    public void AdjustGridSize(int widthDelta, int heightDelta)
    {
        int newWidth = Mathf.Max(1, gridWidth + widthDelta);
        int newHeight = Mathf.Max(1, gridHeight + heightDelta);

        if (newWidth == gridWidth && newHeight == gridHeight)
        {
            return;
        }

        gridWidth = newWidth;
        gridHeight = newHeight;
        RebuildGrid();
    }

    public void OverrideGridSize(int width, int height)
    {
        width = Mathf.Max(1, width);
        height = Mathf.Max(1, height);

        if (width == gridWidth && height == gridHeight)
        {
            return;
        }

        gridWidth = width;
        gridHeight = height;
        RebuildGrid();
    }

    private bool EvaluateCondition(TileCondition condition, int z) // Checks whether the tile is special or not 
    {
        switch (condition)
        {
            case TileCondition.ZEqualsZero:
                return z == 0;
            case TileCondition.ZEqualsLength:
                return z == gridHeight - 1;
            case TileCondition.Regular:
                // Regular is true if none of the other conditions match
                return !(z == 0 || z == gridHeight - 1);
            default:
                return false;
        }
    }


    // Check if a position is occupied
    public bool IsTileOccupied(Vector3 position)  // Updated to use Vector3
    {
        if (occupiedTiles.TryGetValue(position, out GameObject occupier))
        {
            if (occupier == null)
            {
                occupiedTiles.Remove(position);
                return false;
            }

            return true;
        }

        return false;
    }

    // Retrieve the GameObject currently occupying the tile, or null if empty
    public GameObject GetOccupant(Vector3 position)
    {
        if (occupiedTiles.TryGetValue(position, out GameObject occupier))
        {
            if (occupier == null)
            {
                occupiedTiles.Remove(position);
                return null;
            }

            return occupier;
        }

        return null;
    }

    // Mark a position as occupied
    public void OccupyTile(Vector3 position, GameObject occupier)  // Updated to use Vector3
    {
        if (occupiedTiles.TryGetValue(position, out GameObject current))
        {
            if (current == null || current == occupier)
            {
                occupiedTiles[position] = occupier;
            }

            return;
        }

        occupiedTiles[position] = occupier;
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
