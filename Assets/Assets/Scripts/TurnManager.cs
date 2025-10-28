using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class TurnManager : MonoBehaviour
{
    public static TurnManager instance;

    private enum TurnState { PlayerTurn, ProjectileTurn, EnemyTurn }
    private TurnState currentTurn;

    private List<GameObject> players;
    private List<GameObject> enemies;
    private List<GameObject> projectiles;

    private int currentPlayerIndex = 0;
    private int currentEnemyIndex = 0;
    private int currentProjectileIndex = 0;

    public GameData gameData; // Assign GameData object in inspector
    private Dictionary<GameObject, int> playerMoves; // track each player's moves

    public int turnNumber = 1;
    public TMP_Text turnDisplayUI;

    public enum FirstTurn
    {
        Player,
        Enemy
    };

    [SerializeField] private FirstTurn firstTurn; // who starts

    public bool HasShot = false;

    private void Awake()
    {
        if (instance == null) instance = this;
        else Destroy(gameObject);
    }

    private void Start()
    {
        if (gameData == null) gameData = FindObjectOfType<GameData>();

        RefreshSceneLists();

        // Initialize player move counts
        playerMoves = new Dictionary<GameObject, int>();
        foreach (var player in players)
        {
            if (player != null) playerMoves[player] = 0;
        }

        // Set starting turn
        currentTurn = firstTurn == FirstTurn.Player ? TurnState.PlayerTurn : TurnState.EnemyTurn;
        if (currentTurn == TurnState.EnemyTurn)
        {
            StartCoroutine(EnemyTurn());
        }

        HasShot = false;
        UpdateTurnDisplay();
    }

    private void RefreshSceneLists()
    {
        players = new List<GameObject>(GameObject.FindGameObjectsWithTag("Player"));
        enemies = new List<GameObject>(GameObject.FindGameObjectsWithTag("Enemy"));
        projectiles = new List<GameObject>(GameObject.FindGameObjectsWithTag("Projectile"));
    }

    public IEnumerator Delay()
    {
        yield return new WaitForSeconds(0.5f);
    }

    public IEnumerator SwitchToEnemyTurn()
    {
        currentTurn = TurnState.EnemyTurn;
        currentEnemyIndex = 0;

        RefreshSceneLists();
        if (enemies.Count == 0)
        {
            // No enemies? Go straight to next player turn.
            currentTurn = TurnState.PlayerTurn;
            turnNumber += 1;
            UpdateTurnDisplay();
            yield break;
        }

        StartCoroutine(EnemyTurn());
    }

    public IEnumerator SwitchToProjectileTurn()
    {
        yield return Delay();
        currentTurn = TurnState.ProjectileTurn;
        currentProjectileIndex = 0;

        RefreshSceneLists();
        if (projectiles.Count == 0)
        {
            // No projectiles? Skip to enemy phase
            StartCoroutine(SwitchToEnemyTurn());
            yield break;
        }

        StartCoroutine(ProjectileTurn());
    }

    // Call this when the player ends their turn (your button / logic)
    public void EndPlayerTurn()
    {
        if (currentTurn == TurnState.PlayerTurn)
        {
            currentPlayerIndex++;
            if (currentPlayerIndex >= players.Count)
            {
                currentPlayerIndex = 0;

                // After the last player finishes, go to PROJECTILE phase first.
                currentTurn = TurnState.ProjectileTurn;
                StartCoroutine(SwitchToProjectileTurn());
            }
            else
            {
                // Reset move count for the next player (if you use per-player turns)
                if (players[currentPlayerIndex] != null)
                {
                    playerMoves[players[currentPlayerIndex]] = 0;
                }
            }
        }

        RefreshSceneLists();
        HasShot = false;
    }

    void UpdateTurnDisplay()
    {
        if (turnDisplayUI != null)
            turnDisplayUI.text = "Turn: " + turnNumber;
    }

    // Method to check if the current player has remaining moves
    public bool CanPlayerMove()
    {
        if (players.Count == 0) return false;
        GameObject currentPlayer = players[Mathf.Clamp(currentPlayerIndex, 0, Mathf.Max(0, players.Count - 1))];
        int maxMoves = gameData.playerMaxMove; // Assuming all players share the same max moves
        if (currentPlayer == null || !playerMoves.ContainsKey(currentPlayer)) return false;
        return playerMoves[currentPlayer] < maxMoves;
    }

    public void PlayerMadeMove()
    {
        // Use the gameData instance to access playerMove and playerMaxMove
        if (gameData.playerMove >= gameData.playerMaxMove)
        {
            // End turn if playerMove is already equal to or exceeds playerMaxMove
            gameData.canMove = false;
        }
    }

    // =========================
    // ENEMY PHASE (as you had)
    // =========================

    // Call this method ONLY once all enemies have taken their turn
    public void EndEnemyTurn()
    {
        currentEnemyIndex++;
        if (currentEnemyIndex >= enemies.Count)
        {
            currentEnemyIndex = 0;
            currentTurn = TurnState.PlayerTurn;

            turnNumber += 1;
            UpdateTurnDisplay();
            Debug.Log("Turn number = " + turnNumber);
        }
        else
        {
            StartCoroutine(EnemyTurn());
        }

        RefreshSceneLists();
    }

    private IEnumerator EnemyTurn()
    {
        if (currentEnemyIndex < enemies.Count)
        {
            GameObject enemy = enemies[currentEnemyIndex];

            if (enemy == null)
            {
                EndEnemyTurn();
                yield break;
            }

            DefaultEnemyAI enemyAI = enemy.GetComponent<DefaultEnemyAI>();

            if (enemyAI != null)
            {
                enemyAI.EnemyAI();
            }

            yield return Delay();

            EndEnemyTurn();
        }
    }

    // ============================
    // PROJECTILE PHASE (NEW)
    // ============================

    public void EndProjectileTurn()
    {
        currentProjectileIndex++;
        if (currentProjectileIndex >= projectiles.Count)
        {
            currentProjectileIndex = 0;
            // After projectiles are all resolved, go to Enemy phase
            StartCoroutine(SwitchToEnemyTurn());
        }
        else
        {
            StartCoroutine(ProjectileTurn());
        }

        RefreshSceneLists();
    }

    private IEnumerator ProjectileTurn()
    {
        if (currentProjectileIndex < projectiles.Count)
        {
            GameObject proj = projectiles[currentProjectileIndex];

            // It may have been destroyed by physics/collisions
            if (proj == null)
            {
                EndProjectileTurn();
                yield break;
            }

            DefaultProjectileAI projAI = proj.GetComponent<DefaultProjectileAI>();

            if (projAI != null)
            {
                // Execute projectile logic for this "turn"
                projAI.ProjectileAI();
            }

            // Give a tiny delay so effects/animations can be seen and coroutines inside projectile can schedule
            yield return Delay();

            EndProjectileTurn();
        }
        else
        {
            // Safety: if no valid index, jump to enemy phase
            StartCoroutine(SwitchToEnemyTurn());
        }
    }

    public bool IsPlayerTurn()     => currentTurn == TurnState.PlayerTurn;
    public bool IsEnemyTurn()      => currentTurn == TurnState.EnemyTurn;
    public bool IsProjectileTurn() => currentTurn == TurnState.ProjectileTurn;
}
