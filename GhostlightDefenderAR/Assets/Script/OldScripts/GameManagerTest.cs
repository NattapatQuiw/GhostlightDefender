
using UnityEngine.XR.ARFoundation;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;


public class GameManagerTest : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private ARPlaneManager planeManager; 
    [SerializeField] private ARSession arSession;
    //New Code
    [SerializeField] private UIManagerTest uiManager;
    [SerializeField] private GameObject enemyPrefab;
    
    //New Code
    [Header("Enemy Settings")]
    [SerializeField] private int enemyCount = 1;
    [SerializeField] private float spawnRate = 2f;
    [SerializeField] private float despawnRate = 5f;
    
    private bool _gameStarted = false;
    
    //New Code
    private List<GameObject> _spawnedEnemies = new List<GameObject>();
    private int _score = 0;
    
    void Start()
    {
        //Called from UI button
        UIManagerTest.OnStartButtonPressed += StartGame;
        UIManagerTest.OnRestartButtonPressed += RestartGame;
    }

    void StartGame()
    {
        if (_gameStarted) return;
        _gameStarted = true;
        print("Game started!!!");

        planeManager.enabled = false;
        foreach (var plane in planeManager.trackables)
        {
            var meshVisual = plane.GetComponent<ARPlaneMeshVisualizer>();
            if (meshVisual) meshVisual.enabled = false;
            
            var lineVisual = plane.GetComponent<LineRenderer>();
            if (lineVisual) lineVisual.enabled = false;
        }
        StartCoroutine(SpawnEnemies());
    }

    void RestartGame()
    {
        _gameStarted = false;
        StartCoroutine(RestartGameCoroutine());
    }

    IEnumerator RestartGameCoroutine()
    {
        while (ARSession.state != ARSessionState.SessionTracking)
        {
            yield return null;
        }
        // New Code
        arSession.Reset();
        planeManager.enabled = true;
        _score = 0;
        uiManager.UpdateDateScore(_score);

        foreach (var enemy in _spawnedEnemies)
        {
            Destroy(enemy);
        }
        _spawnedEnemies.Clear();
    }

    void SpawnEnemy()
    {
        if (planeManager.trackables.count == 0) return;
        
        List<ARPlane> planesList = new List<ARPlane>();
        foreach (var plane in planeManager.trackables)
        {
            planesList.Add(plane);
        }
        // Add using Random = UnityEngine.Random; at the top first
        var randomPlane = planesList[Random.Range(0, planesList.Count)];
        var randomPosition = GetRandomPosition(randomPlane);
        var enemy = Instantiate(enemyPrefab, randomPosition, Quaternion.identity);
        _spawnedEnemies.Add(enemy);

        var enemyScript = enemy.GetComponent<EnemyScriptTest>();
        if (enemyScript != null)
        {
            enemyScript.OnEnemyDestroyed += AddScore;
        }
        
        StartCoroutine(DespawnEnemies(enemy));
    }

    Vector3 GetRandomPosition(ARPlane plane)
    {
        var center = plane.center;
        var size = plane.size * 0.5f;
        var randomX = Random.Range(-size.x, size.x);
        var randomY = Random.Range(-size.y, size.y);
        return new Vector3(center.x + randomX, center.y + randomY, center.z + randomY);
    }

    IEnumerator SpawnEnemies()
    {
        while (_gameStarted)
        {
            if (_spawnedEnemies.Count < enemyCount)
            {
                SpawnEnemy();
            }
            yield return new WaitForSeconds(spawnRate);
        }
    }

    IEnumerator DespawnEnemies(GameObject enemy)
    {
        yield return new WaitForSeconds(despawnRate);
        if (_spawnedEnemies.Contains(enemy))
        {
            _spawnedEnemies.Remove(enemy);
            Destroy(enemy);
        }
    }

    void AddScore()
    {
        _score++;
        uiManager.UpdateDateScore(_score);
    }
}
