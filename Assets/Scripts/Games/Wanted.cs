// Title: Wanted
// Author: Eric Truong

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;


/// <summary>
/// Use the joystick to move around a searchlight to find the target!
/// </summary>
public class Wanted : Game
{
    // Holds all of game's non-managerial assets
    [SerializeField] private GameObject gameAssetParent = null;

    [SerializeField] private GameObject real;
    [SerializeField] private GameObject fake;
    [SerializeField] private GameObject player;
    [SerializeField] private TextMeshProUGUI objectiveText;
    [SerializeField] private TextMeshProUGUI timeText;

    // Boundaries for object spawn
    private float rightBoundary = 11f;
    private float leftBoundary = -11f;
    private float ceilBoundary = 4f;
    private float floorBoundary = -4f;

    // Total number of objects on screen
    private int numObjects = 100;
    // Index of when real object will spawn
    private int realIdx;

    // Placeholder timer for the game, will count down
    private float timer = 10f;

    // Used to allow time to decrease
    private bool gameStarted = false;

    /*
     * 0: Apple
     * 1: Grapes
     * 2: Orange
     * 3: Pear
     * 4: Watermelon
     * 5: Banana
     */
    private int realTypeIdx;
    // Contains object sprites
    public Sprite[] spriteArray;

    public override int GetGameTime(int currentDifficulty)
    {
        // Placeholder 10 seconds for now until I figure out how to use this
        return 10;
    }

    public override void StartGame()
    {
        // Set placeholder timer
        timer = GetGameTime(0);

        // Randomize when real object will spawn
        realIdx = Random.Range(1, numObjects);
        // Randomize which object is real
        realTypeIdx = Random.Range(0, 6);
        // Set objective
        switch (realTypeIdx)
        {
            case 0:
                objectiveText.text = "Find Apple";
                break;
            case 1:
                objectiveText.text = "Find Grapes";
                break;
            case 2:
                objectiveText.text = "Find Orange";
                break;
            case 3:
                objectiveText.text = "Find Pear";
                break;
            case 4:
                objectiveText.text = "Find Watermelon";
                break;
            case 5:
                objectiveText.text = "Find Banana";
                break;
        }

        // Spawn all fruits for the game
        GenerateObjects();

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

        // Reset position and scale of player
        player.transform.position = Vector3.zero;
        player.transform.localScale = new Vector3(3.5f, 3.5f, 1f);

        // Reset player's ability to win
        player.GetComponent<WantedPlayerController>().AllowWin(false);

        StartGame();
    }

    public override void UnLoad()
    {
        // Disable game assets
        gameAssetParent.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        if (gameStarted)
        {
            timer -= Time.deltaTime;
            timeText.text = Mathf.Ceil(timer).ToString();
            if (timer < 0)
            {
                gameStarted = false;
            }
        }
    }

    private void GenerateObjects()
    {
        // Corrdinates for spawning
        float spawnX;
        float spawnY;
        // Index to determine which object to spawn
        int objectIdx;
        Vector2 spawnPos;
        // Sprite of the object to spawn
        Sprite objectSprite;

        for (int i = 1; i <= numObjects; i++)
        {
            spawnX = Random.Range(leftBoundary, rightBoundary);
            spawnY = Random.Range(floorBoundary, ceilBoundary);
            spawnPos = new Vector2(spawnX, spawnY);

            objectIdx = Random.Range(0, 5);
            while (objectIdx == realTypeIdx)
            {
                objectIdx = Random.Range(0, 5);
            }
            objectSprite = spriteArray[objectIdx];

            // If index is index of real object then spawn it, otherwise spawn fake objects
            if (i == realIdx)
            {
                Instantiate(real, spawnPos, real.transform.rotation).GetComponent<SpriteRenderer>().sprite = spriteArray[realTypeIdx];
            }
            else
            {
                Instantiate(fake, spawnPos, fake.transform.rotation).GetComponent<SpriteRenderer>().sprite = objectSprite;
            }
        }
    }

    IEnumerator IntroSequence()
    {
        objectiveText.enabled = true;
        yield return new WaitForSeconds(2);
        objectiveText.enabled = false;
        gameStarted = true;
        player.GetComponent<WantedPlayerController>().AllowWin(true);
        yield return new WaitForEndOfFrame();
    }

    public float getTime()
    {
        return timer;
    }

    public void OnWin()
    {
        objectiveText.enabled = true;
        objectiveText.text = "WIN";
    }

    public void OnLose()
    {
        objectiveText.enabled = true;
        objectiveText.text = "LOSE";
    }
}
