using UnityEngine;
using System.Collections;

[RequireComponent(typeof(GridMovement))]
public class DefaultProjectileAI : MonoBehaviour
{

[Tooltip("World/grid step applied each projectile phase.")]
[SerializeField] private Vector3 direction = Vector3.forward;

[SerializeField] public int projectileDamage = 10;

    private GridMovement gridMovement;
    private GridManager gridManager;

    private void Awake()
    {
        gridMovement = GetComponent<GridMovement>();
        gridManager = GridManager.instance;
    }

    public void ProjectileAI()
    {
        if (gridMovement != null && gridMovement.isMoving)
        {
            return;
        }

        if (gridManager == null)
        {
            Debug.LogWarning($"{name}: GridManager not present; destroying projectile.");
            Destroy(gameObject);
            return;
        }

        Vector3Int stepInt = Vector3Int.RoundToInt(direction);
        stepInt.y = 0;

        if (stepInt == Vector3Int.zero && direction.sqrMagnitude > Mathf.Epsilon)
        {
            Vector3 normalized = direction.normalized;
            stepInt = Vector3Int.RoundToInt(new Vector3(normalized.x, 0f, normalized.z));
        }

        if (stepInt == Vector3Int.zero)
        {
            return;
        }

        Vector2Int currentGrid = new Vector2Int(Mathf.FloorToInt(transform.position.x), Mathf.FloorToInt(transform.position.z));
        Vector2Int targetGrid = currentGrid + new Vector2Int(stepInt.x, stepInt.z);

        if (targetGrid.x < 0 || targetGrid.x >= gridManager.GetGridWidth() ||
            targetGrid.y < 0 || targetGrid.y >= gridManager.GetGridHeight())
        {
            Destroy(gameObject);
            return;
        }

        Vector3 moveStep = new Vector3(stepInt.x, 0f, stepInt.z);
        StartCoroutine(MoveAndResolve(moveStep));
    }

    private IEnumerator MoveAndResolve(Vector3 step)
    {
        if (gridMovement == null)
        {
            yield break;
        }

        // Move even if tile is occupied so the camera follows
        yield return gridMovement.MovePlayer(step, ignoreOccupied: true);

        if (gridManager == null)
        {
            yield break;
        }

        Vector2 newGrid = new Vector2(Mathf.Floor(transform.position.x), Mathf.Floor(transform.position.z));
        GameObject occupant = gridManager.GetOccupant(newGrid);

        // If someone else still owns this tile, projectile dies
        if (occupant != null && occupant != gameObject)
        {

            // If the thing that owns the tile it's moving to is an enemy or a player, the target takes 10 damage.
            if (occupant.CompareTag("Enemy") == true || occupant.CompareTag("Player"))
            {
                Debug.LogWarning("projectile hit an enemy or a player");
                var stats = occupant.GetComponent<CharacterStats>();
                if (stats != null)
                {
                    DamageUtilities.DamageTarget(new CharacterStats[] { stats }, projectileDamage);
                }
                else
                {
                    Debug.LogWarning($"{occupant.name} missing CharacterStats component.");
                }
            }


            // destroy the projectile
            Destroy(gameObject);
            
            


        }
    }
}
