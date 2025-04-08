using UnityEngine.XR.ARFoundation;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class GameManager : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private ARPlaneManager planeManager;
    [SerializeField] private ARSession arSession;
    [SerializeField] private UiManager uiManager;
    [SerializeField] private GameObject[] enemyPrefabs;
    [SerializeField] private HealthManager healthManager;

    [Header("Enemy Settings")]
    [SerializeField] private float spawnRate = 2.5f;

    private bool _gameStarted = false;
    private List<GameObject> _spawnedEnemies = new List<GameObject>();

    void Start()
    {
        UiManager.OnStartButtonPressed += StartGame;
        UiManager.OnRetryButtonPressed += RestartGame;
    }

    void StartGame()
    {
        if (_gameStarted) return;
        _gameStarted = true;

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
        healthManager.resetHealth();
        StartCoroutine(RestartGameCoroutine());
    }

    IEnumerator RestartGameCoroutine()
    {
        while (ARSession.state != ARSessionState.SessionTracking)
        {
            yield return null;
        }

        arSession.Reset();
        planeManager.enabled = true;

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

        var randomPlane = planesList[Random.Range(0, planesList.Count)];
        var randomPosition = GetRandomPosition(randomPlane);
        var selectedPrefab = enemyPrefabs[Random.Range(0, enemyPrefabs.Length)];
        var enemy = Instantiate(selectedPrefab, randomPosition, Quaternion.identity);
        _spawnedEnemies.Add(enemy);
    }

    Vector3 GetRandomPosition(ARPlane plane)
    {
        var center = plane.center;
        var size = plane.size * 0.5f;
        var randomX = Random.Range(-size.x, size.x);
        var randomY = Random.Range(0f, 1f);
        return new Vector3(center.x + randomX, center.y + randomY, center.z + randomY);
    }

    IEnumerator SpawnEnemies()
    {
        while (_gameStarted)
        {
            SpawnEnemy();
            yield return new WaitForSeconds(spawnRate);
        }
    }
}