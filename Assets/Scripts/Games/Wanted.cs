// Title: Wanted
// Author: Eric Truong

using System.Collections;
using System.Collections.Generic;
using UnityEngine;


/// <summary>
/// Use the joystick to move around a searchlight to find the target!
/// </summary>
public class Wanted : Game
{
    // Holds all of game's non-managerial assets
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
        // Enable game assets
        gameAssetParent.SetActive(true);
    }

    public override void UnLoad()
    {
        // Disable game assets
        gameAssetParent.SetActive(false);
    }
}
