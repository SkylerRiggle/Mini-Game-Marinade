// Title: Game
// Author: Skyler Riggle

/// <summary>
/// The base interface class for a minigame. This allows other
/// parts of the game to interact with a minigame without needing
/// to know any specific game information.
/// </summary>
public interface Game 
{
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
    /// just before the unload function.
    /// </summary>
    public abstract void EndGame();
}