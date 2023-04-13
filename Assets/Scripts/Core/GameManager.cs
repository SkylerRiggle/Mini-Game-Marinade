// Title: GameManager
// Author: Skyler Riggle

using UnityEngine;
using System.Collections;
using System.Collections.Generic;

/// <summary>
/// The game manager responsible for tracking global game information
/// and behaviour.
/// </summary>
public class GameManager : Singleton<GameManager> 
{
    // Game Control Values
    private int gamesCompleted;
    private int playerLives;
    private const int DEFAULT_LIVES_COUNT = 3;


    // Timer Values
    private Coroutine currentGameTimer = null;
    private float _currentTime = 0;
    public float currentTime { get { return _currentTime; } }

    // Game Tracking Values
    [SerializeField] private Game[] availableGames = new Game[0];
    private Queue<int> gameQueue = new Queue<int>();
    private int lastRandomIndex = 0;
    private Game currentGame;

    /// <summary>
    /// Starts the minigame gameplay loop.
    /// </summary>
    public void StartGame()
    {
        // Ensure that the default game values are set.
        gamesCompleted = 0;
        playerLives = DEFAULT_LIVES_COUNT;
        _currentTime = 0;

        // Minor error checking to ensure that a game can be played.
        if (availableGames.Length == 0)
        {
            throw new System.Exception("GAME MANAGER HAS NO GAMES!");
        }

        // Initialize the game queue.
        GetGameQueue();

        // Load the first game.
        LoadNewGame();
    }

    /// <summary>
    /// Stores a random queue of game indicies such that the same game may never 
    /// be played twice, and all games are played at least once in a queue.
    /// </summary>
    private void GetGameQueue()
    {
        // Initialize a random starting point from an incremental offset.
        int offset = Random.Range(1, availableGames.Length - 1);
        int index = (lastRandomIndex + offset) % availableGames.Length;

        // Place the game indexes into the queue.
        for (int _i = 0; _i < availableGames.Length; _i++)
        {
            gameQueue.Enqueue(index);
            index = (index + offset) % availableGames.Length;
        }
    }

    /// <summary>
    /// Updates the current game in favor of a new one.
    /// </summary>
    public void LoadNewGame()
    {
        // Stop the current timer.
        if (currentGameTimer != null)
        {
            StopCoroutine(currentGameTimer);
        }

        // End the current game and handle the player's success status.
        if (currentGame != null)
        {
            if (HandleSuccessState(currentGame.EndGame()))
            {
                return;
            }
            currentGame.UnLoad();
        }

        // Ensure that the queue is not empty.
        if (gameQueue.Count == 0)
        {
            GetGameQueue();
        }

        // Gather a new game from the queue.
        currentGame = availableGames[0];

        // Store the default run time for the new game.
        _currentTime = currentGame.GetGameTime(gamesCompleted);

        // Start the game and the timer.
        currentGame.Load();
        currentGame.StartGame();
        currentGameTimer = StartCoroutine(GameTimer());
    }

    /// <summary>
    /// Handles the success and failure states for a minigame.
    /// </summary>
    /// <param name="isVictorious">A boolean indicating the success status for the current game.</param>
    /// <returns>A boolean corresponding to whether the game has concluded (player has died).</returns>
    private bool HandleSuccessState(bool isVictorious)
    {
        // Handle the success and failure case.
        if (isVictorious)
        {
            gamesCompleted++;
        }
        else
        {
            // Decrease the player's life count and 
            // check to see if the game has ended.
            playerLives--;
            if (playerLives == 0)
            {
                EndGame();
                return true;
            }
        }

        return false;
    }

    /// <summary>
    /// Handle the ending of the minigame gameplay loop.
    /// </summary>
    private void EndGame()
    {
        // TODO: HIGHSCORE & NEXT SCENE
        Debug.Log("Game Over uwu");
    }

    /// <summary>
    /// Handles the timer for the current game.
    /// </summary>
    private IEnumerator GameTimer()
    {
        // Decrement the timer till it reads zero or less.
        while (_currentTime > 0)
        {
            _currentTime -= Time.deltaTime;
            yield return null;
        }

        // Move on to the next game.
        LoadNewGame();
        yield return null;
    }
}