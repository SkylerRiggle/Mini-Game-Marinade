// Title: GameManager
// Author: Skyler Riggle

using UnityEngine;

/// <summary>
/// The game manager responsible for tracking global game information
/// and behaviour.
/// </summary>
public class GameManager : Singleton<GameManager> 
{
    // Static Constant Values
    private const int DEFAULT_GAMES_COMPLETED = 0;
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
    /// Starts the minigame gameplay loop.
    /// </summary>
    public void StartGame()
    {
        // Ensure that the default game values are set.
        gamesCompleted = DEFAULT_GAMES_COMPLETED;
        playerLives = DEFAULT_LIVES_COUNT;

        // Load the first game.
        LoadNewGame();
    }

    /// <summary>
    /// Updates the current game in favor of a new one.
    /// </summary>
    private void LoadNewGame()
    {
        // End the current game and handle the player's success status.
        if (currentGame != null)
        {
            HandleSuccessState(currentGame.EndGame());
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

        // Start the game.
        currentGame.Load();
        currentGame.StartGame();
    }

    /// <summary>
    /// Handles the success and failure states for a minigame.
    /// </summary>
    /// <param name="isVictorious">A boolean indicating the success status for the current game.</param>
    private void HandleSuccessState(bool isVictorious)
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
                return;
            }

            // Otherwise, transition to the next game.
            LoadNewGame();
        }
    }

    /// <summary>
    /// Called when a game has been beaten.
    /// </summary>
    public void GameSuccess() => HandleSuccessState(true);

    /// <summary>
    /// Called when a game has been failed.
    /// </summary>
    public void GameFailure() => HandleSuccessState(false);

    private void EndGame()
    {

    }
}