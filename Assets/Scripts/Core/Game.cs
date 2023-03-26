// Title: Game
// Author: Skyler Riggle

using UnityEngine;

/// <summary>
/// The base abstract class for a minigame. This allows other
/// parts of the game to interact with a minigame without needing
/// to know any specific game information.
/// </summary>

public abstract class Game : MonoBehaviour
{
    /// <summary>
    /// Determines how long this minigame should run in seconds given
    /// the current difficulty.
    /// </summary>
    /// <param name="currentDifficulty">
    /// The current difficulty of the game measured in the number of games completed successfully.
    /// </param>
    /// <returns>This minigame's runtime in seconds</returns>
    public abstract int GetGameTime(int currentDifficulty);

    /// <summary>
    /// Handles the loading process for a game.
    /// </summary>
    public abstract void Load();

    /// <summary>
    /// Handles the unloading process for a game.
    /// </summary>
    public abstract void UnLoad();

    /// <summary>
    /// This method is called just after the load function has finished execution.
    /// </summary>
    public abstract void StartGame();

    /// <summary>
    /// This method is called immediately at the end of a game's life cycle,
    /// just before the unload function. This method also returns a boolean value
    /// indicating whether the player has succeeded in the game or failed.
    /// </summary>
    /// <returns>A boolean indicating the player's victory status for the game.</returns>
    public abstract bool EndGame();
}