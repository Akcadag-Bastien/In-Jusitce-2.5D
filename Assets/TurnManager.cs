using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TurnManager : MonoBehaviour
{
    public static TurnManager instance;

    private enum TurnState { PlayerTurn, EnemyTurn }
    private TurnState currentTurn;

    private List<GameObject> players;
    private List<GameObject> enemies;

    private int currentPlayerIndex = 0;
    private int currentEnemyIndex = 0;

    public GameData gameData; // Assign GameData object in ispector
    private Dictionary<GameObject, int> playerMoves; // Dictionary to track each player's moves

    public enum FirstTurn
    {
        Player,
        Enemy
    }

    [SerializeField] private FirstTurn firstTurn; // Declare a variable to store the starting turn

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void Start()
    {
        if (gameData == null)
        {
            gameData = FindObjectOfType<GameData>();
        }

        players = new List<GameObject>(GameObject.FindGameObjectsWithTag("Player"));
        enemies = new List<GameObject>(GameObject.FindGameObjectsWithTag("Enemy"));

        // Initialize player move counts
        playerMoves = new Dictionary<GameObject, int>();
        foreach (var player in players)
        {
            playerMoves[player] = 0;
        }

        // Set starting turn
        currentTurn = firstTurn == FirstTurn.Player ? TurnState.PlayerTurn : TurnState.EnemyTurn;
        if (currentTurn == TurnState.EnemyTurn)
        {
            StartCoroutine(EnemyTurn());
        }
    }

    public IEnumerator DelayBetweenTurns()
    {
        // Wait for 0.5 seconds
        yield return new WaitForSeconds(0.5f);
    }

    public IEnumerator SwitchToEnemyTurn()
    {
            yield return DelayBetweenTurns(); // Wait before switching turns
            currentTurn = TurnState.EnemyTurn;
            StartCoroutine(EnemyTurn());
    }


    // Call this method when the player decides to end their turn
    public void EndPlayerTurn()
    {
        if (currentTurn == TurnState.PlayerTurn)
        {
            currentPlayerIndex++;
            if (currentPlayerIndex >= players.Count)
            {

                currentPlayerIndex = 0;

                // Block player actions and start delayed enemy turn
                currentTurn = TurnState.EnemyTurn;
                StartCoroutine(SwitchToEnemyTurn());

            }
            else
            {
                // Reset move count for the next player
                playerMoves[players[currentPlayerIndex]] = 0;
            }
        }
    }

    // Method to check if the current player has remaining moves
    public bool CanPlayerMove()
    {
        GameObject currentPlayer = players[currentPlayerIndex];
        int maxMoves = gameData.playerMaxMove; // Assuming all players share the same max moves
        return playerMoves[currentPlayer] < maxMoves;
    }

    public void PlayerMadeMove()
    {
            // Use the gameData instance to access playerMove and playerMaxMove
        if (gameData.playerMove >= gameData.playerMaxMove)
            {

                // End turn if playerMove is already equal to or exceeds playerMaxMove

                EndPlayerTurn();
                gameData.playerMove = 1;
            }
    }

    // Call this method ONLY once all enemies have taken their turn
    public void EndEnemyTurn()
    {
        currentEnemyIndex++;
        if (currentEnemyIndex >= enemies.Count)
        {
            currentEnemyIndex = 0;
            currentTurn = TurnState.PlayerTurn;
        }
        else
        {
            StartCoroutine(EnemyTurn());
        }
    }

    private IEnumerator EnemyTurn()
    {
        if (currentEnemyIndex < enemies.Count)
        {
            GameObject enemy = enemies[currentEnemyIndex];
            DefaultEnemyAI enemyAI = enemy.GetComponent<DefaultEnemyAI>();

            if (enemyAI != null)
            {
                enemyAI.EnemyAI();
            }

            yield return new WaitForSeconds(0.5f);

            EndEnemyTurn();
        }
    }

    public bool IsPlayerTurn()
    {
        return currentTurn == TurnState.PlayerTurn;
    }

    public bool IsEnemyTurn()
    {
        return currentTurn == TurnState.EnemyTurn;
    }
}
