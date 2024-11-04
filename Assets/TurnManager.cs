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
        players = new List<GameObject>(GameObject.FindGameObjectsWithTag("Player"));
        enemies = new List<GameObject>(GameObject.FindGameObjectsWithTag("Enemy"));

        if (firstTurn == FirstTurn.Player)
        {
            currentTurn = TurnState.PlayerTurn; // Start with the player's turn
        }
        else if (firstTurn == FirstTurn.Enemy)
        {
            currentTurn = TurnState.EnemyTurn;
            StartCoroutine(EnemyTurn());
        }
    }

    // Call this method when the player decides to end their turn (with a button or with the shortcut)
    public void EndPlayerTurn()
    {
        if (currentTurn == TurnState.PlayerTurn)
        {
            currentPlayerIndex++;
            if (currentPlayerIndex >= players.Count)
            {
                currentPlayerIndex = 0;
                currentTurn = TurnState.EnemyTurn;
                StartCoroutine(EnemyTurn());
            }
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
            // Continue to the next enemy's turn
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

            // Impose a delay to give time for the player to understand the enemy movements and prevent a bug
            yield return new WaitForSeconds(0.5f); // Adjust delay to prevent overlapping on the same tile

            // Once all enemies have moved, end their turn
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
