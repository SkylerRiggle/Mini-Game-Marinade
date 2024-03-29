// Title: ManualOverdrive
// Author: Skyler Riggle

using UnityEngine;

/// <summary>
/// Racing time! Carefully navigate a track with the wheel, while also shifting the car into gear with the joystick.
/// </summary>
public class ManualOverdrive : Game
{
    // This parent game object holds all of this game's non-managerial assets.
    [SerializeField] private GameObject gameAssetParent = null;

    // The manager responsible for assigning the current road mesh.
    [SerializeField] private RoadManager roadManager = null;

    [Header("Difficulty Parameters:")]
    [SerializeField] private AnimationCurve difficultyCurve = null;
    [SerializeField] private float difficultyWeight = 0.01f;
    [SerializeField] private float difficultyBias = 0;

    public override int GetGameTime(int currentDifficulty)
    {
        float difficultyParameter = (currentDifficulty * difficultyWeight) + difficultyBias;
        return Mathf.CeilToInt(difficultyCurve.Evaluate(difficultyParameter));
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

        // Assign a random road mesh.
        roadManager.AssignRoad();
    }

    public override void UnLoad()
    {
        // Unload the current road mesh.
        roadManager.RemoveRoad();

        // Disable our game's assets.
        gameAssetParent.SetActive(false);
    }
}
