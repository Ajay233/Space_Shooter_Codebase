using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnManager : MonoBehaviour
{
    [SerializeField]
    private Enemy _enemyPrefab;
    [SerializeField]
    private Asteroid _asteroidPrefab;
    [SerializeField]
    private GameObject _enemyContainer;
    [SerializeField]
    private GameObject _asteroidContainer;
    private bool _stopSpawn = false;
    [SerializeField]
    private PowerUp _powerupPrefab;
    [SerializeField]
    private PowerUp[] powerups;
    private int _waveNumber;
    private int _enemyWaveSize;
    

    public void StartSpawnRoutines(int waveNumber)
    {
        StartCoroutine(SpawnEnemyRoutine(waveNumber));
        StartCoroutine(SpawnPowerUpRoutine());
    }

    IEnumerator SpawnEnemyRoutine(int waveNumber)
    {
        yield return new WaitForSeconds(3f);  // Take this out once the or decrease the time once warning implemented
        while (_stopSpawn == false)
        {
            if (waveNumber % 2 == 0)
            {
                Debug.Log("Spawning Enemies");
                Enemy enemy = Instantiate(_enemyPrefab, new Vector3(Random.Range(-8.8f, 8.8f), 7, 0), Quaternion.identity);
                enemy.transform.parent = _enemyContainer.transform;   // We have to assign a transform to a transform
            } else
            {
                Debug.Log("Spawning Asteroids");
                Asteroid asteroid = Instantiate(_asteroidPrefab, new Vector3(Random.Range(-8.8f, 8.8f), 7, 0), Quaternion.identity);
                asteroid.transform.parent = _asteroidContainer.transform; // Adds the asteroid prefab to the asteroidContainer while it's active
            }
            yield return new WaitForSeconds(5);
        }
    }

    public void stopEnemySpawn()
    {
        _stopSpawn = true;
    }

    IEnumerator SpawnPowerUpRoutine()
    {
        yield return new WaitForSeconds(3f); // Take this out or decrease the time when wave is warning implemented
        while (_stopSpawn == false)
        {
            int randomIndex = Random.Range(0, 3);
            PowerUp prefab = powerups[randomIndex];
            PowerUp powerup = Instantiate(prefab, new Vector3(Random.Range(-8.8f, 8.8f), 7, 0), Quaternion.identity);
            yield return new WaitForSeconds(Random.Range(6f, 12f));
        }
    }

    
    // Set enemiesKilled value in Player class
    // Then begin spawn routine


    // set stopSpawn method to set the stopSpawn bool to true
    public void StartSpawn()
    {
        _stopSpawn = false;
    }

    IEnumerator NewWaveIntro()
    {
        //StartCoroutine(ShowNewWaveBanner());
        yield return new WaitForSeconds(3);
        //_showWaveNumberWarning = false;
    }

    
}
