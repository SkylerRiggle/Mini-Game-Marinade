// Title: Flight
// Author: Eric Truong

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

/// <summary>
/// Control your flying player and hit the moving target!
/// </summary>
public class Flight : Game
{
    // Used for loading and resetting game assets
    [SerializeField] private GameObject gameAssetParent = null;

    [SerializeField] private GameObject playerParent = null;
    [SerializeField] private GameObject target = null;
    [SerializeField] private GameObject lookParent = null;
    [SerializeField] private GameObject mainCam = null;
    [SerializeField] private GameObject playerCam = null;

    // Used for game events and functionality
    [SerializeField] private AudioClip loseSound;
    [SerializeField] private TextMeshProUGUI objectiveText;
    [SerializeField] private TextMeshProUGUI timeText;
    [SerializeField] private GameEvent onLose;

    private AudioSource audioSource;

    private float timer = 10f;

    private bool gameStarted = false;
    private bool won = false;

    public override int GetGameTime(int currentDifficulty)
    {
        // Placeholder return value until I figure out how to use this
        return 0;
    }

    public override void StartGame()
    {
        audioSource = GetComponent<AudioSource>();

        gameStarted = true;
        StartCoroutine(IntroSequence());
    }

    public override bool EndGame()
    {
        UnLoad();
        return true;
    }

    public override void Load()
    {
        // Enable game assets
        gameAssetParent.SetActive(true);

        // Enable the camera for this minigame
        playerCam.SetActive(true);
        playerCam.tag = "MainCamera";
        // Disable the camera of the main game
        mainCam.tag = "Untagged";
        mainCam.SetActive(false);

        // Reset position of player container
        playerParent.transform.position = Vector3.zero;
        // Reset position of target
        target.transform.position = new Vector3(0, 50, 1000);
        // Reset position of look parent
        lookParent.transform.position = new Vector3(0, 50, -40);

        StartGame();
    }

    public override void UnLoad()
    {
        // Enable the main camera of the main game
        mainCam.SetActive(true);
        mainCam.tag = "MainCamera";
        // Disable the camera for this minigame
        playerCam.tag = "Untagged";
        playerCam.SetActive(false);


        // Disable game assets
        gameAssetParent.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        if (gameStarted)
        {
            // Decrease timer
            timer -= Time.deltaTime;
            // Only display time in full seconds
            timeText.text = Mathf.Ceil(timer).ToString();
            if (timer < 0)
            {
                // Prevent timer from decreasing once timer hits 0
                gameStarted = false;
                // If player has not already won, trigger lose sequence
                if (!won)
                {
                    objectiveText.enabled = true;
                    objectiveText.text = "LOSE";
                    audioSource.PlayOneShot(loseSound);

                    // Invoke lose event if player has not won
                    onLose?.Invoke();
                }
            }
        }
    }

    IEnumerator IntroSequence()
    {
        objectiveText.enabled = true;
        objectiveText.text = "Hit the Target!";
        yield return new WaitForSeconds(2);
        objectiveText.enabled = false;
        yield return new WaitForEndOfFrame();
    }

    public void Win()
    {
        won = true;

        objectiveText.enabled = true;
        objectiveText.text = "WIN";
    }
}
