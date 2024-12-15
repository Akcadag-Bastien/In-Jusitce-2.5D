using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkipTurn : MonoBehaviour
{
    [SerializeField] KeyCode skip = KeyCode.E;
    private GridMovement gridMovement; // Reference to the GridMovement script

    public bool skipThisTurn = false;

    public GameData gameData; // Assign GameData object in ispector

    public TurnManager turnManager; // Assign TurnManager object in inspector

    void Start()
    {
        // Get the GridMovement component from the player GameObject
        gridMovement = GetComponent<GridMovement>();
    }

    void Update()
    {
        // Ensure gridMovement is not null
        if (gridMovement != null && Input.GetKey(skip) && !gridMovement.isMoving) // Use IsMoving instead of accessing it directly
        {
            skipThisTurn = false;
            gameData.canMove = true;
            gameData.playerMove = 0;
            turnManager.EndPlayerTurn();
        }
    }
}