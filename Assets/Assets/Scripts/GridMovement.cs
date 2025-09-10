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

    public GameData gameData; // Assign in Inspector or find it in Start()
    public TurnManager TurnManager; // Assign in Inspector or find it in Start()

    void Start()
    {

         if (gameData == null)
        {
            gameData = FindObjectOfType<GameData>();
        }

        GameObject cameraManager = GameObject.Find("CameraManager");
        CameraCycle cameraCycle = GameObject.Find("CameraManager").GetComponent<CameraCycle>();
        Debug.Log("Current camera : " + cameraCycle.currentCamera);

        // Mark the initial position as occupied when the game starts
        Vector2 gridPosition = new Vector2(Mathf.Floor(transform.position.x), Mathf.Floor(transform.position.z));
        GridManager.instance.OccupyTile(gridPosition, this.gameObject);
    }

    void Update()
    {
        if (isPlayer)
        {
            // Only allow player movement during the player's turn
            if (!TurnManager.instance.IsPlayerTurn()) 
            {
                return;
            }

            // Handle player movement with input
            if (Input.GetKey(moveUp) && !isMoving && gameData.playerMove <= gameData.playerMaxMove)
            {
                StartCoroutine(MovePlayer(Vector3.forward));
                Debug.Log(gameData.playerMove);
                Debug.Log(gameData.playerMaxMove);
                TurnManager.instance.PlayerMadeMove();
            }

            else if (Input.GetKey(moveLeft) && !isMoving && gameData.playerMove <= gameData.playerMaxMove)
            {
                StartCoroutine(MovePlayer(Vector3.left));
                Debug.Log(gameData.playerMove);
                Debug.Log(gameData.playerMaxMove);
                TurnManager.instance.PlayerMadeMove();
            }

            else if (Input.GetKey(moveDown) && !isMoving && gameData.playerMove <= gameData.playerMaxMove)
            {
                StartCoroutine(MovePlayer(Vector3.back));
                Debug.Log(gameData.playerMove);
                Debug.Log(gameData.playerMaxMove);
                TurnManager.instance.PlayerMadeMove();
            }

            else if (Input.GetKey(moveRight) && !isMoving && gameData.playerMove <= gameData.playerMaxMove)
            {
                StartCoroutine(MovePlayer(Vector3.right));
                Debug.Log(gameData.playerMove);
                Debug.Log(gameData.playerMaxMove);
                TurnManager.instance.PlayerMadeMove();
            }

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
            TurnManager.DelayBetweenTurns();
            isMoving = false;
            Debug.Log("Tile out of range !");
            yield break;
        }

        // Check if the target tile is already occupied
        if (GridManager.instance.IsTileOccupied(targetGridPos))
        {
            TurnManager.DelayBetweenTurns();
            isMoving = false;
            Debug.Log("Tile is already occupied !");
            yield break;
        }

        // Free the current tile before moving
        GridManager.instance.FreeTile(origGridPos);

        // Get reference to CameraFollow
        CameraFollow cameraFollow = null;
        if (Camera.main != null)
        {
            cameraFollow = Camera.main.GetComponent<CameraFollow>();
        }

        // Only set target and follow if cameraFollow exists
        if (cameraFollow != null)
        {
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

        // Increase the amount of mmoves made by 1 after a move

        if (gameObject.CompareTag("Player"))
        {

            // Allow the move, then increment playerMove
            gameData.playerMove++;

        }
        else
        {
            gameData.playerMove = 1;
        }

        // Update the player's position and mark the new tile as occupied
        transform.position = targetPos;
        GridManager.instance.OccupyTile(targetGridPos, this.gameObject);

        isMoving = false;
    }
}
