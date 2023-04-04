// Title: ManualOverdrive
// Author: Skyler Riggle

using UnityEngine;

/// <summary>
/// Racing time! Carefully navigate a track with the wheel, while also shifting the car into gear with the joystick.
/// </summary>
public class ManualOverdrive : Game
{
    /// <summary>
    /// This parent game object holds all of this game's non-managerial assets.
    /// </summary>
    [SerializeField] private GameObject gameAssetParent = null;

    public override int GetGameTime(int currentDifficulty)
    {
        throw new System.NotImplementedException();
    }

    public override void StartGame()
    {
        throw new System.NotImplementedException();
    }

    public override bool EndGame()
    {
        throw new System.NotImplementedException();
    }

    public override void Load()
    {
        // Enable our game's assets.
        gameAssetParent.SetActive(true);
    }

    public override void UnLoad()
    {
        // Disable our game's assets.
        gameAssetParent.SetActive(false);
    }
}
