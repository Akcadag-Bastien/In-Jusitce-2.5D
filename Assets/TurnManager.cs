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

        currentTurn = TurnState.PlayerTurn; // Start with the player's turn
    }

    // Method to be called when the player ends their turn
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

    // Method for ending the enemy's turn
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

            // Wait for a short delay to simulate AI decision making
            yield return new WaitForSeconds(0.5f); /// /!\ IF THIS VALUE IS TOO SMALL, TWO CHARACTERS WILL BE ABLE TO MOVE TO THE SAME TILE /!\
            /// /!\ THIS IS BECAUSE THE GridManager DOES NOT HAVE THE TIME TO SET THE TILE AS "OCCUPIED" /!\

            // After the enemy moves, end their turn
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