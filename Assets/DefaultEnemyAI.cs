using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DefaultEnemyAI : MonoBehaviour
{
    public void EnemyAI()
    {
        // Generate a random direction (up, down, left, or right)
        Vector3[] directions = { Vector3.forward, Vector3.back, Vector3.left, Vector3.right };
        Vector3 randomDirection = directions[Random.Range(0, directions.Length)];

        // Calculate the target position
        Vector3 targetPosition = transform.position + randomDirection;
        Vector2 targetGridPos = new Vector2(Mathf.Floor(targetPosition.x), Mathf.Floor(targetPosition.z));

        // Check if the target tile is occupied
        if (GridManager.instance.IsTileOccupied(targetGridPos))
        {
            // If occupied, try the opposite direction
            Vector3 oppositeDirection = -randomDirection;
            targetPosition = transform.position + oppositeDirection;
            targetGridPos = new Vector2(Mathf.Floor(targetPosition.x), Mathf.Floor(targetPosition.z));

            // Only move if the opposite tile is free
            if (!GridManager.instance.IsTileOccupied(targetGridPos))
            {
                MoveEnemy(oppositeDirection);
            }
            else
            {
                Debug.Log("No valid moves available for this enemy.");
            }
        }
        else
        {
            // If the initial target tile is free, move in the original random direction
            MoveEnemy(randomDirection);
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
