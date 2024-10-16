using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestEnemyAI_2 : MonoBehaviour
{
    public void EnemyAI()
    {
        // Always move +1 on the Z axis
        Vector3 direction = Vector3.back;

        // Calculate the target position
        Vector3 targetPosition = transform.position + direction;
        Vector2 targetGridPos = new Vector2(Mathf.Floor(targetPosition.x), Mathf.Floor(targetPosition.z));

        // Check if the target tile is occupied
        if (!GridManager.instance.IsTileOccupied(targetGridPos))
        {
            // If the target tile is free, move
            MoveEnemy(direction);
        }
        else
        {
            Debug.Log("Tile is occupied, cannot move forward.");
        }
    }

    private void MoveEnemy(Vector3 direction)
    {
        // Call the MovePlayer function from GridMovement to move the enemy
        GridMovement gridMovement = GetComponent<GridMovement>();
        if (gridMovement != null && !gridMovement.isMoving)
        {
            StartCoroutine(gridMovement.MovePlayer(direction));
        }
    }
}
