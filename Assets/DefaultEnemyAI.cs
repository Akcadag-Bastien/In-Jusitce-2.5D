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

        // Call the MovePlayer function from GridMovement to move the enemy randomly
        GridMovement gridMovement = GetComponent<GridMovement>();
        if (gridMovement != null && !gridMovement.isMoving)
        {
            StartCoroutine(gridMovement.MovePlayer(randomDirection));
        }
    }
}
