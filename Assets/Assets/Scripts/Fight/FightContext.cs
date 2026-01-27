using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Provides a single entry point for fight modifiers to mutate the scene that was just loaded.
/// Attach this to any always-on object inside the fight scene and assign your core managers.
/// </summary>
public class FightContext : MonoBehaviour
{
    [SerializeField] private GridManager gridManager;
    [SerializeField] private TurnManager turnManager;
    [SerializeField] private GameData gameData;

    private void Awake()
    {
        if (gridManager == null) gridManager = FindObjectOfType<GridManager>();
        if (turnManager == null) turnManager = FindObjectOfType<TurnManager>();
        if (gameData == null) gameData = FindObjectOfType<GameData>();
    }

    private void Start()
    {
        ApplyQueuedModifiers();
    }

    private void ApplyQueuedModifiers()
    {
        if (FightEncounterService.Instance == null)
        {
            return;
        }

        IReadOnlyList<FightModifier> modifiers = FightEncounterService.Instance.ConsumeModifiers();
        bool appliedModifier = false;
        foreach (var modifier in modifiers)
        {
            if (modifier == null)
            {
                continue;
            }

            try
            {
                modifier.Apply(this);
                appliedModifier = true;
            }
            catch (System.Exception ex)
            {
                Debug.LogException(ex);
            }
        }

        if (appliedModifier)
        {
            ReclaimOccupiedTiles();
        }
    }

    public void AdjustGridSize(int widthDelta, int lengthDelta)
    {
        if (gridManager == null)
        {
            Debug.LogWarning("Cannot modify grid size because no GridManager is assigned.");
            return;
        }

        gridManager.AdjustGridSize(widthDelta, lengthDelta);
    }

    public GridManager Grid => gridManager;
    public TurnManager Turn => turnManager;
    public GameData GameData => gameData;

    private void ReclaimOccupiedTiles()
    {
        GridMovement[] movers = FindObjectsOfType<GridMovement>();
        foreach (GridMovement mover in movers)
        {
            if (mover == null)
            {
                continue;
            }

            mover.ClaimCurrentTile();
        }
    }
}
