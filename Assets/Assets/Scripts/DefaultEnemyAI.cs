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
        Dictionary<Vector3, GameObject> occupantByDirection = new Dictionary<Vector3, GameObject>();

        // Get the current grid position of the enemy
        Vector2 currentGridPos = new Vector2(Mathf.Floor(transform.position.x), Mathf.Floor(transform.position.z));

        // Check all directions to see if the tile is available
        foreach (Vector3 direction in possibleDirections)
        {
            Vector2 targetGridPos = currentGridPos + new Vector2(direction.x, direction.z);

            // Check if the tile is within the grid bounds
            if (targetGridPos.x < 0 || targetGridPos.x >= GridManager.instance.GetGridWidth() ||
                targetGridPos.y < 0 || targetGridPos.y >= GridManager.instance.GetGridHeight())
            {
                continue;
            }

            GameObject occupant = GridManager.instance.GetOccupant(targetGridPos);

            if (occupant != null && !IsProjectile(occupant))
            {
                continue;
            }

            if (!occupantByDirection.ContainsKey(direction))
            {
                availableDirections.Add(direction); // Add the available direction
                occupantByDirection[direction] = occupant;
            }
        }

        // If there are any available directions, choose one randomly
        if (availableDirections.Count > 0)
        {
            Vector3 randomDirection = availableDirections[Random.Range(0, availableDirections.Count)];

            GridMovement gridMovement = GetComponent<GridMovement>();
            if (gridMovement != null && !gridMovement.isMoving)
            {
                GameObject occupant = occupantByDirection[randomDirection];

                if (occupant != null && IsProjectile(occupant))
                {
                    StartCoroutine(MoveThroughProjectile(gridMovement, randomDirection, occupant));
                }
                else
                {
                    StartCoroutine(gridMovement.MovePlayer(randomDirection));
                }
            }
        }
        else
        {
            Debug.Log("No available moves for the enemy!");
        }
    }

    private IEnumerator MoveThroughProjectile(GridMovement gridMovement, Vector3 direction, GameObject projectile)
    {
        if (gridMovement == null)
        {
            yield break;
        }

        GridManager gm = GridManager.instance;

        Vector2 startGrid = new Vector2(Mathf.Floor(transform.position.x), Mathf.Floor(transform.position.z));
        Vector2 targetGrid = startGrid + new Vector2(direction.x, direction.z);

        if (gm != null)
        {
            gm.FreeTile(targetGrid);
        }

        if (projectile != null)
        {

            var projectileAI = projectile.GetComponent<DefaultProjectileAI>();
            var stats = this.GetComponent<CharacterStats>();
            if (projectileAI != null)
            {
                DamageUtilities.DamageTarget(new CharacterStats[] { stats }, projectileAI.projectileDamage);
            }
            else
            {
                Debug.LogWarning($"{this.name} missing CharacterStats component.");
            }
        }

        yield return gridMovement.MovePlayer(direction);

        Destroy(projectile);
    }

    private bool IsProjectile(GameObject obj)
    {
        if (obj == null)
        {
            return false;
        }

        if (obj.CompareTag("Projectile"))
        {
            return true;
        }

        return obj.GetComponent<DefaultProjectileAI>() != null || obj.GetComponent<ProjectileMove>() != null;
    }
}
