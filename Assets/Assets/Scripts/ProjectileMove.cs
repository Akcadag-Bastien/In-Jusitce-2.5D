using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class ProjectileMove : MonoBehaviour
{
    [SerializeField] private TurnManager turnManager;
    [SerializeField] private int projectileSpeed = 1;
    [SerializeField] private bool enemyProjectile = false;
    private GridMovement gridMovement;
    private void Awake()
    {
        gridMovement = GetComponent<GridMovement>();
    }
    private void Start()
    {
        if (turnManager == null)
        {
            turnManager = TurnManager.instance != null
                ? TurnManager.instance
                : FindObjectOfType<TurnManager>();
        }
    }
    public void MoveOnTurnEnd()
    {
        if (gridMovement == null)
        {
            Debug.LogWarning($"{name} is missing a GridMovement component and cannot move.");
            return;
        }
        if (projectileSpeed == 0)
        {
            return;
        }
        if (gridMovement.isMoving)
        {
            return;
        }
        Vector3 zDirection = enemyProjectile ? Vector3.back : Vector3.forward;
        Vector3 movement = zDirection * projectileSpeed;
        StartCoroutine(ProjectileStep(movement));
    }

    private IEnumerator ProjectileStep(Vector3 movement)
    {
        // Move even onto occupied tiles so camera follow runs
        yield return gridMovement.MovePlayer(movement, ignoreOccupied: true);

        GridManager gm = GridManager.instance;
        if (gm == null)
        {
            yield break;
        }

        Vector2 newGrid = new Vector2(Mathf.Floor(transform.position.x), Mathf.Floor(transform.position.z));
        GameObject occupant = gm.GetOccupant(newGrid);

        if (occupant != null && occupant != gameObject)
        {
            Destroy(gameObject);
        }
    }
}
