using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DefaultEnemyAI : MonoBehaviour
{
    public void EnemyAI()
    {
        // Define potential movement directions (up, down, left, right)
        Vector3[] possibleDirections = { Vector3.forward, Vector3.back, Vector3.left, Vector3.right };

        List<Vector3> availableDirections = new List<Vector3>();

        // Get the current grid position of the enemy
        Vector2 currentGridPos = new Vector2(Mathf.Floor(transform.position.x), Mathf.Floor(transform.position.z));

        // Check all directions to see if the tile is available
        foreach (Vector3 direction in possibleDirections)
        {
            Vector2 targetGridPos = currentGridPos + new Vector2(direction.x, direction.z);

            // Check if the tile is within the grid bounds and not occupied
            if (!GridManager.instance.IsTileOccupied(targetGridPos) &&
                targetGridPos.x >= 0 && targetGridPos.x < GridManager.instance.GetGridWidth() &&
                targetGridPos.y >= 0 && targetGridPos.y < GridManager.instance.GetGridHeight())
            {
                availableDirections.Add(direction); // Add the available direction
            }
        }

        // If there are any available directions, choose one randomly
        if (availableDirections.Count > 0)
        {
            Vector3 randomDirection = availableDirections[Random.Range(0, availableDirections.Count)];

            // Call the MovePlayer function from GridMovement to move the enemy in the chosen direction
            GridMovement gridMovement = GetComponent<GridMovement>();
            if (gridMovement != null && !gridMovement.isMoving)
            {
                StartCoroutine(gridMovement.MovePlayer(randomDirection));
            }
        }
        else
        {
            Debug.Log("No available moves for the enemy!");
        }
    }
}