using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridMovement : MonoBehaviour
{
    public bool isMoving;
    private Vector3 origPos, targetPos;
    private float timeToMove = 0.2f;

    [SerializeField] KeyCode moveUp = KeyCode.W;
    [SerializeField] KeyCode moveLeft = KeyCode.A;
    [SerializeField] KeyCode moveDown = KeyCode.S;
    [SerializeField] KeyCode moveRight = KeyCode.D;
    [SerializeField] private bool isPlayer; // True if this is the player's character, false for enemies

    void Start()
    {
        // Mark the initial position as occupied when the game starts
        Vector2 gridPosition = new Vector2(Mathf.Floor(transform.position.x), Mathf.Floor(transform.position.z));
        GridManager.instance.OccupyTile(gridPosition, this.gameObject);
    }

    void Update()
    {
        if (isPlayer)
        {
            // Only allow player movement during the player's turn
            if (!TurnManager.instance.IsPlayerTurn()) return;

            // Handle player movement with input
            if (Input.GetKey(moveUp) && !isMoving)
                StartCoroutine(MovePlayer(Vector3.forward));

            else if (Input.GetKey(moveLeft) && !isMoving)
                StartCoroutine(MovePlayer(Vector3.left));

            else if (Input.GetKey(moveDown) && !isMoving)
                StartCoroutine(MovePlayer(Vector3.back));

            else if (Input.GetKey(moveRight) && !isMoving)
                StartCoroutine(MovePlayer(Vector3.right));
        }
    }

    // Method to move enemies (called by TurnManager)
    public void MoveEnemy(Vector3 direction)
    {
        if (!isPlayer && !isMoving)
        {
            StartCoroutine(MovePlayer(direction));
        }
    }

    public IEnumerator MovePlayer(Vector3 direction)
    {
        isMoving = true;

        float elapsedTime = 0;

        origPos = transform.position;
        targetPos = origPos + direction;

        // Convert positions to grid coordinates
        Vector2 origGridPos = new Vector2(Mathf.Floor(origPos.x), Mathf.Floor(origPos.z));
        Vector2 targetGridPos = new Vector2(Mathf.Floor(targetPos.x), Mathf.Floor(targetPos.z));

        // Check if the target position is within grid boundaries
        if (targetGridPos.x < 0 || targetGridPos.x >= GridManager.instance.GetGridWidth() || 
            targetGridPos.y < 0 || targetGridPos.y >= GridManager.instance.GetGridHeight())  
        {
            isMoving = false;
            yield break;
        }

        // Check if the target tile is already occupied
        if (GridManager.instance.IsTileOccupied(targetGridPos))
        {
            Debug.Log("Tile is already occupied!");
            isMoving = false;
            yield break;
        }

        // Free the current tile before moving
        GridManager.instance.FreeTile(origGridPos);

        // Get reference to CameraFollow
        CameraFollow cameraFollow = Camera.main.GetComponent<CameraFollow>();
        if (cameraFollow != null)
        {
            // Update the target to the moving character
            cameraFollow.SetTarget(transform);
        }

        // Move the player
        while (elapsedTime < timeToMove)
        {
            transform.position = Vector3.Lerp(origPos, targetPos, (elapsedTime / timeToMove));
            
            // Update the camera position while the player is moving
            if (cameraFollow != null)
            {
                cameraFollow.FollowTarget();
            }

            elapsedTime += Time.deltaTime;
            yield return null;
        }

        // Update the player's position and mark the new tile as occupied
        transform.position = targetPos;
        GridManager.instance.OccupyTile(targetGridPos, this.gameObject);

        isMoving = false;
    }
}
