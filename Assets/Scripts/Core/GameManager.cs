// Title: GameManager
// Author: Skyler Riggle

using UnityEngine;
using System.Collections;

/// <summary>
/// The game manager responsible for tracking global game information
/// and behaviour.
/// </summary>
public class GameManager : Singleton<GameManager> 
{
    // Static Constant Values
    private const int DEFAULT_GAMES_COMPLETED = 0;
    private const float DEFAULT_TIMER_COUNT = 0;
    private const int DEFAULT_LIVES_COUNT = 3;

    /// <summary>
    /// The available minigame's to be played.
    /// </summary>
    [SerializeField] private Game[] availableGames = new Game[0];

    /// <summary>
    /// The current game being played.
    /// </summary>
    private Game currentGame;

    /// <summary>
    /// The number of minigames completed by the player [ONLY successful completions]
    /// </summary>
    private int gamesCompleted = DEFAULT_GAMES_COMPLETED;

    /// <summary>
    /// The player's remaining lives.
    /// </summary>
    private int playerLives = DEFAULT_LIVES_COUNT;

    /// <summary>
    /// The amount of time remaining for the current game.
    /// </summary>
    private float currentTime = DEFAULT_TIMER_COUNT;

    /// <summary>
    /// Starts the minigame gameplay loop.
    /// </summary>
    public void StartGame()
    {
        // Ensure that the default game values are set.
        gamesCompleted = DEFAULT_GAMES_COMPLETED;
        playerLives = DEFAULT_LIVES_COUNT;
        currentTime = DEFAULT_TIMER_COUNT;

        // Load the first game.
        LoadNewGame();
    }

    /// <summary>
    /// Updates the current game in favor of a new one.
    /// </summary>
    private void LoadNewGame()
    {
        // Stop the current timer.
        StopCoroutine(GameTimer());

        // End the current game and handle the player's success status.
        if (currentGame != null)
        {
            if (!HandleSuccessState(currentGame.EndGame(), false))
            {
                return;
            }
            currentGame.UnLoad();
        }

        // Gather a new minigame that does not match the current game.
        int gameIndex;
        do
        {
            gameIndex = Random.Range(0, availableGames.Length);
        } while (availableGames[gameIndex].Equals(currentGame));

        // Assign the new minigame as the current.
        currentGame = availableGames[gameIndex];

        // Get the new game timer value.
        currentTime = currentGame.GetGameTime(gamesCompleted);

        // Start the game and the timer.
        currentGame.Load();
        currentGame.StartGame();
        StartCoroutine(GameTimer());
    }

    /// <summary>
    /// Handles the success and failure states for a minigame.
    /// </summary>
    /// <param name="isVictorious">A boolean indicating the success status for the current game.</param>
    /// <param name="canLoad">Indicates if this method is allowed to load a new game on failure.</param>
    /// <returns>A boolean indicating whether the game can continue.</returns>
    private bool HandleSuccessState(bool isVictorious, bool canLoad)
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
                return false;
            }

            // Otherwise, transition to the next game.
            if (canLoad)
            {
                LoadNewGame();
            }
        }

        // Return a status that the game has not ended.
        return true;
    }

    /// <summary>
    /// Called when a game has been beaten.
    /// </summary>
    public void GameSuccess() => HandleSuccessState(true, true);

    /// <summary>
    /// Called when a game has been failed.
    /// </summary>
    public void GameFailure() => HandleSuccessState(false, true);

    /// <summary>
    /// Handle the ending of the minigame gameplay loop.
    /// </summary>
    private void EndGame()
    {
        // TODO: HIGHSCORE & NEXT SCENE
    }

    /// <summary>
    /// Handles the current game's timer.
    /// </summary>
    private IEnumerator GameTimer()
    {
        // Decrement the timer till it reads zero or less.
        while (currentTime > 0)
        {
            currentTime -= Time.deltaTime;
            yield return null;
        }

        // Move on to the next game.
        LoadNewGame();

        // Stop the current coroutine.
        StopCoroutine(GameTimer());
        yield return null;
    }
}