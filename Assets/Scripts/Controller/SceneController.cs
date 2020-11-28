using UnityEngine;

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

    private const int baseEnemies = 3;
    private int enemiesToSpawn;
    private int currNumEnemies;
    private const float baseSpawnRate = 2.0f;
    private float spawnRate = 2.0f;
    private float nextSpawn = 0.0f;
    private float difficultyDelta = 1f;

    private const int numWaves = 5;
    private int currWave = 0;
    private const int healthOnWaveEnd = 25;

    // Start is called before the first frame update
    void Start()
    {
        currNumEnemies = 0;
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
        enemiesToSpawn = (int)(baseEnemies * difficultyDelta * currWave);
        spawnRate = baseSpawnRate / (difficultyDelta * currWave);

        // Broadcast start of new wave
        Messenger<int, int>.Broadcast(GameEvent.WAVE_STARTED, currWave, numWaves);
    }

    private void Awake()
    {
        Messenger.AddListener(GameEvent.ENEMY_DEAD, OnEnemyDead);
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
                GameObject enemy = Instantiate(enemyPrefabs[i]) as GameObject;
                enemy.transform.position = spawnPoints[Random.Range(0, spawnPoints.Length)].transform.position;
                return;
            }
            randomWeight -= enemyWeights[i];
        }
        Debug.LogError("Failed to spawn enemy");
    }

    private void OnDestroy()
    {
        Messenger.RemoveListener(GameEvent.ENEMY_DEAD, OnEnemyDead);
    }

    // Start new wave if no enemies remaining in current wave
    public void OnEnemyDead()
    {
        currNumEnemies--;

        // Start new wave if no enemies remaining in wave
        if (currNumEnemies == 0 && enemiesToSpawn == 0)
        {
            if (currWave < numWaves)
            {
                StartWave();
            } else
            {
                Messenger.Broadcast(GameEvent.GAME_WON);
            }
            
        } else if (currNumEnemies == 0 && enemiesToSpawn > 0)
        {
            nextSpawn = Time.time;
        }
    }
}
