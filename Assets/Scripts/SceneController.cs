using UnityEngine;
using System.Collections.Generic;

public class SceneController : MonoBehaviour
{
    [Header("References")]
    [Tooltip("Reference to all players in the game.")]
    [SerializeField] private PlayerCharacter[] players = null;

    [Header("Enemy Spawning")]
    [Tooltip("Reference to each enemy prefab that can be spawned ingame.")]
    [SerializeField] private GameObject[] enemyPrefabs = null;
    [Tooltip("Parallel array to Enemy Prefabs. Contains the spawn rate of the corresponding enemy.")]
    [SerializeField] private int[] enemyWeights = null;
    [Tooltip("Array containing empty objects designating spawn positions")]
    [SerializeField] private GameObject[] spawnPoints = null;

    // Enemy spawning
    private const int baseEnemies = 3;
    private int enemiesToSpawn;
    private int currNumEnemies = 0;
    private const float baseSpawnRate = 2.0f;
    private float spawnRate;
    private float nextSpawn = 0.0f;
    private const float enemyCountDelta = 1f;
    private const float spawnRateDelta = 0.95f;
    private const float playersPreventSpawnRadius = 10f;

    // Wave handling
    public int currWave { get; private set; }
    public const int NUM_WAVES = 5;
    private const int healthOnWaveEnd = 25;

    // Start is called before the first frame update
    void Start()
    {
        spawnRate = baseSpawnRate;
        StartWave();
    }

    // Do preparation for next wave
    private void StartWave()
    {
        // Heal players
        foreach (PlayerCharacter player in players)
        {
            player.FirstAid(healthOnWaveEnd);
        }

        // Setup enemies for next wave
        currWave++;
        enemiesToSpawn = (int)(baseEnemies * enemyCountDelta * currWave);
        spawnRate *= spawnRateDelta;

        // Broadcast start of new wave
        new WaveStartedEvent(currWave).Fire();
    }

    private void OnEnable()
    {
        EnemyDeadEvent.Register(OnEnemyDead);
    }

    // Update is called once per frame
    void Update()
    {
        // Spawn enemies if any remain to spawn and enough time has passed
        if (enemiesToSpawn > 0 && Time.time > nextSpawn)
        {
            enemiesToSpawn--;
            nextSpawn = Time.time + spawnRate;
            currNumEnemies++;

            SpawnEnemy();
        }
    }

    // Select an enemy from a weighted-random list and spawn it in at a random spawn location
    private void SpawnEnemy()
    {
        // Select the enemy type to spawn
        int selectedEnemy = -1;
        int totalWeight = 0;
        foreach(int weight in enemyWeights)
        {
            totalWeight += weight;
        }

        int randomWeight = Random.Range(0, totalWeight);
        for(int i = 0; i < enemyPrefabs.Length; i++)
        {
            if (randomWeight < enemyWeights[i])
            {
                selectedEnemy = i;
                break;
            }
            randomWeight -= enemyWeights[i];
        }

        if (selectedEnemy == -1)
        {
            Debug.LogError("Failed to select enemy type");
            return;
        }

        // Filter to valid Spawn positions
        List<GameObject> filteredSpawnPoints = new List<GameObject>();
        foreach (GameObject point in spawnPoints)
        {
            bool playerInRadius = false;

            Collider[] cols = Physics.OverlapSphere(point.transform.position, playersPreventSpawnRadius);
            foreach (var col in cols)
            {
                if (col.gameObject.CompareTag("Player"))
                {
                    playerInRadius = true;
                }
            }

            if (!playerInRadius)
            {
                filteredSpawnPoints.Add(point);
            }
        }

        // Spawn the enemy
        GameObject enemy = Instantiate(enemyPrefabs[selectedEnemy]) as GameObject;
        enemy.transform.position = spawnPoints[Random.Range(0, spawnPoints.Length)].transform.position;
    }

    private void OnDisable()
    {
        EnemyDeadEvent.Unregister(OnEnemyDead);
    }

    // Start new wave if no enemies remaining in current wave
    private void OnEnemyDead(EnemyDeadEvent ede)
    {
        currNumEnemies--;

        // Start new wave if no enemies remaining in wave
        if (currNumEnemies == 0 && enemiesToSpawn == 0)
        {
            if (currWave < NUM_WAVES)
            {
                StartWave();
            } else
            {
                new GameWonEvent().Fire();
            }
        } else if (currNumEnemies == 0 && enemiesToSpawn > 0)
        {
            nextSpawn = Time.time;
        }
    }
}
