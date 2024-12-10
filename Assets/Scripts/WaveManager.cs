using System.Collections;
using UnityEngine;

public class WaveManager : MonoBehaviour
{
    public GameObject enemyPrefab; 
    public Vector2 spawnAreaMin; 
    public Vector2 spawnAreaMax; 
    public float timeBetweenWaves = 0.5f;
    public float spawnRate = 1f; 

    private int waveNumber = 5; 

    void Start()
    {
        StartCoroutine(SpawnWaves());
    }

    IEnumerator SpawnWaves()
    {
        while (true)
        {
            yield return new WaitForSeconds(timeBetweenWaves);

            for (int i = 0; i < waveNumber; i++) 
            {
                SpawnEnemy();
                SpawnEnemy();
                SpawnEnemy();

                yield return new WaitForSeconds(spawnRate); 
            }

            waveNumber++; 
        }
    }

    void SpawnEnemy()
    {
        // Generate a random position within the spawn area
        float randomX = Random.Range(spawnAreaMin.x, spawnAreaMax.x);
        float randomY = Random.Range(spawnAreaMin.y, spawnAreaMax.y);
        Vector2 randomPosition = new Vector2(randomX, randomY);

        // Instantiate the enemy at the random position
        Instantiate(enemyPrefab, randomPosition, Quaternion.identity);
    }
}
