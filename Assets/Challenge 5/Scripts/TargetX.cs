using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class TargetX : MonoBehaviour
{
    private Rigidbody rb;
    private GameManagerX gameManagerX;
    public int pointValue;
    public GameObject explosionFx;

    public float timeOnScreen = 4.0f;

    private float minValueX = -3.75f; // the x value of the center of the left-most square
    private float minValueY = -3.75f; // the y value of the center of the bottom-most square
    private float spaceBetweenSquares = 2.5f; // the distance between the centers of squares on the game board
    

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        gameManagerX = GameObject.Find("Game Manager").GetComponent<GameManagerX>();

        transform.position = RandomSpawnPosition(); 
        StartCoroutine(RemoveObjectRoutine()); // Destroy food after timeOnScreen if not clicked

    }

    // When target is clicked, destroy it, update score, and generate explosion
    void Update()
    {
        // Only respond to actual mouse clicks, not continuous hovering
        if (Mouse.current.leftButton.wasPressedThisFrame)
        {
            Ray ray = Camera.main.ScreenPointToRay(Mouse.current.position.ReadValue());
            RaycastHit hitInfo;

            if (Physics.Raycast(ray, out hitInfo))
            {
                if (hitInfo.transform == transform)
                {
                    Destroy(gameObject);
                    gameManagerX.UpdateScore(pointValue);
                    Explode();
                }
            }
        }
        if (gameManagerX.isGameActive)
        {
            
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
    int RandomSquareIndex ()
    {
        return Random.Range(0, 4);
    }


    // If target collides with the sensor, trigger game over for good objects and destroy the target
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Sensor"))
        {
            if (!gameObject.CompareTag("Bad"))
            {
                gameManagerX.GameOver();
            }
            Destroy(gameObject);
        }
    }

    // Display explosion particle at object's position
    void Explode ()
    {
        Instantiate(explosionFx, transform.position, explosionFx.transform.rotation);
    }

    // After timeOnScreen seconds, move the object toward the sensor so missed targets trigger game over
    IEnumerator RemoveObjectRoutine ()
    {
        yield return new WaitForSeconds(timeOnScreen);
        if (gameManagerX.isGameActive)
        {
            transform.Translate(Vector3.forward * 5, Space.World);
        }
    }
}