using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI; // Needed to detect the Button variable as valid

public class SkipTurn : MonoBehaviour
{
    public KeyCode skip; // Assign this in the Inspector
    public Button skipButton; // Assign the Button in the Inspector
    public GridMovement gridMovement; // Reference to the GridMovement script
    public GameData gameData; // Reference to the GameData script
    public TurnManager turnManager; // Reference to the TurnManager script
    public bool skipThisTurn = false;

    void Start()
    {
        // Get the GridMovement component from the player GameObject
        gridMovement = GetComponent<GridMovement>();
    }

    void Update()
    {
        // Ensure gridMovement is not null
        if (Input.GetKey(skip))
        {
            SkipPlayerTurn();
        }

        skipButton.onClick.AddListener(SkipPlayerTurn);
    }

    // Skip the player's turn when called

    void SkipPlayerTurn()
    {
        if (gridMovement != null && !gridMovement.isMoving)
        {
            skipThisTurn = false;
            gameData.canMove = true;
            gameData.playerMove = 0;
            turnManager.EndPlayerTurn();
        }
    }
}