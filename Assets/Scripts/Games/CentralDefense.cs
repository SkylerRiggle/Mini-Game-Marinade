using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class CentralDefense : Game
{
    [SerializeField] private GameObject gameAssetParent = null;

    [SerializeField] private GameObject enemy;
    [SerializeField] private GameObject player;
    [SerializeField] private TextMeshProUGUI objectiveText;
    [SerializeField] private TextMeshProUGUI timeText;

    [SerializeField] private GameEvent onWin;

    private float timer = 10f;
    private bool gameStarted = false;
    private bool hasLost = false;

    public override int GetGameTime(int currentDifficulty)
    {
        // Placeholder 10 seconds for now until I figure out how to use this
        return 10;
    }

    public override void StartGame()
    {
        timer = GetGameTime(0);

        timeText.text = Mathf.Ceil(timer).ToString();
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

        // Reset transform of player
        player.transform.rotation = Quaternion.Euler(0, 0, 0);

        // Reset conditions
        gameStarted = false;
        hasLost = false;
    }

    public override void UnLoad()
    {
        // Destroy all remaining projectiles
        foreach (GameObject obj in GameObject.FindGameObjectsWithTag("Enemy"))
        {
            Destroy(obj);
        }

        gameAssetParent.SetActive(false);
    }

    private void Update()
    {
        
    }

    IEnumerator IntroSequence()
    {
        objectiveText.enabled = true;
        yield return new WaitForSeconds(2);
        objectiveText.enabled = false;
        gameStarted = true;
        StartCoroutine(SpawnEnemies());
        yield return new WaitForEndOfFrame();
    }

    IEnumerator SpawnEnemies()
    {
        int spawnIdx;
        Vector2 spawnPos;

        float enemySpd = 0;
        GameObject spawnedEnemy;

        while (gameStarted && !hasLost)
        {
            spawnIdx = Random.Range(0, 8);

            // Set spawn points, starting from the right going counter-clockwise
            switch (spawnIdx)
            {
                case 0:
                    spawnPos.x = 12;
                    spawnPos.y = 0;
                    enemySpd = 7;
                    break;
                case 1:
                    spawnPos.x = 6;
                    spawnPos.y = 6;
                    enemySpd = 5;
                    break;
                case 2:
                    spawnPos.x = 0;
                    spawnPos.y = 6;
                    enemySpd = 3;
                    break;
                case 3:
                    spawnPos.x = -6;
                    spawnPos.y = 6;
                    enemySpd = 5;
                    break;
                case 4:
                    spawnPos.x = -12;
                    spawnPos.y = 0;
                    enemySpd = 7;
                    break;
                case 5:
                    spawnPos.x = -6;
                    spawnPos.y = -6;
                    enemySpd = 5;
                    break;
                case 6:
                    spawnPos.x = 0;
                    spawnPos.y = -6;
                    enemySpd = 3;
                    break;
                case 7:
                    spawnPos.x = 6;
                    spawnPos.y = -6;
                    enemySpd = 5;
                    break;
                default:
                    spawnPos.x = 12;
                    spawnPos.y = 0;
                    enemySpd = 7;
                    break;
            }

            if (gameStarted && !hasLost)
            {
                // Spawn the enemy
                spawnedEnemy = Instantiate(enemy, spawnPos, Quaternion.identity);
                // Set the enemy speed based on position
                spawnedEnemy.GetComponent<CDEnemyScript>().SetSpeed(enemySpd);
            }

            // Delay of 0.75 seconds between spawns
            yield return new WaitForSeconds(0.75f);
        }

        yield return new WaitForEndOfFrame();
    }

    public void OnLose()
    {
        hasLost = true;
        objectiveText.enabled = true;
        objectiveText.text = "LOSE";
    }

    public void OnWin()
    {
        objectiveText.enabled = true;
        objectiveText.text = "WIN";
    }
}
