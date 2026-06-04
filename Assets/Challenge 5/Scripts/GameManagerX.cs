using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameManagerX : MonoBehaviour
{
    public TextMeshProUGUI scoreText;
    public TextMeshProUGUI gameOverText;
    public TextMeshProUGUI timeText;
    public GameObject titleScreen;
    public Button restartButton; 

    public List<GameObject> targetPrefabs;

    private int score;
    private int timeRemaining = 60;
    private float spawnRate = 2.0f;
    public bool isGameActive;

    private float spaceBetweenSquares = 2.5f; 
    private float minValueX = -3.75f;
    private float minValueY = -3.75f;
    
    public void StartGame(int difficulty)
    {
        spawnRate = spawnRate / difficulty;
        isGameActive = true;
        timeRemaining = 60;
        UpdateTimeDisplay();
        StartCoroutine(SpawnTarget());
        StartCoroutine(CountdownTimer());
        score = 0;
        UpdateScore(0);
        titleScreen.SetActive(false);
    }

    IEnumerator SpawnTarget()
    {
        while (isGameActive)
        {
            yield return new WaitForSeconds(spawnRate);
            int index = Random.Range(0, targetPrefabs.Count);

            if (isGameActive)
            {
                Instantiate(targetPrefabs[index], RandomSpawnPosition(), targetPrefabs[index].transform.rotation);
            }
            
        }
    }

    // Generate a random spawn position based on a random index from 0 to 3
    Vector3 RandomSpawnPosition()
    {
        float spawnPosX = minValueX + (RandomSquareIndex() * spaceBetweenSquares);
        float spawnPosY = minValueY + (RandomSquareIndex() * spaceBetweenSquares);

        Vector3 spawnPosition = new Vector3(spawnPosX, spawnPosY, 0);
        return spawnPosition;

    }

    // Generates random square index from 0 to 3, which determines which square the target will appear in
    int RandomSquareIndex()
    {
        return Random.Range(0, 4);
    }

    // Update score with value from target clicked
    public void UpdateScore(int scoreToAdd)
    {
        score += scoreToAdd;
        scoreText.text = "Score: " + score;
    }

    // Update time display
    void UpdateTimeDisplay()
    {
        timeText.text = "Time: " + timeRemaining;
    }

    // Countdown timer that decreases every second and triggers game over at 0
    IEnumerator CountdownTimer()
    {
        while (isGameActive && timeRemaining > 0)
        {
            yield return new WaitForSeconds(1f);
            if (isGameActive)
            {
                timeRemaining--;
                UpdateTimeDisplay();

                if (timeRemaining <= 0)
                {
                    GameOver();
                }
            }
        }
    }

    // Stop game, bring up game over text and restart button
    public void GameOver()
    {
        gameOverText.gameObject.SetActive(true);
        restartButton.gameObject.SetActive(true);
        isGameActive = false;
    }

    // Restart game by reloading the scene
    public void RestartGame()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

}