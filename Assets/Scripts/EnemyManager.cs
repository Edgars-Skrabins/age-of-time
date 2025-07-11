using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class EnemyManager : Singleton<EnemyManager>
{
    [SerializeField] private Enemy m_clusterEnemyPrefab;
    [SerializeField] private AnimationCurve m_minClusterSpawnRate;
    [SerializeField] private AnimationCurve m_maxClusterSpawnRate;

    [SerializeField] private AnimationCurve m_minClusterSize;
    [SerializeField] private AnimationCurve m_maxClusterSize;

    [SerializeField] private Enemy[] m_enemyPrefabs;
    [SerializeField] private Enemy[] m_strongerEnemyPrefabs;
    [SerializeField] private Enemy[] m_miniBossPrefabs;

    private Enemy m_miniBossPrefab;
    private Enemy m_enemyPrefab;
    private Enemy m_strongerEnemyPrefab;

    [SerializeField] private Transform m_spawnPoint;
    [SerializeField] private float m_maxSpawnRangeY;

    [SerializeField] private float m_miniBossSpawnRate;
    private int m_miniBossSpawnIndex;

    [SerializeField] private AnimationCurve m_strongerSpawnRateCurve;
    [SerializeField] private AnimationCurve m_minSpawnDelayCurve;
    [SerializeField] private AnimationCurve m_maxSpawnDelayCurve;

    private readonly List<Enemy> m_spawnedEnemies = new List<Enemy>();

    private Coroutine m_spawnRoutine;
    private Coroutine m_strongerSpawnRoutine;
    private Coroutine m_miniBossSpawnRoutine;

    private bool m_hasClusterSpawned;

    [SerializeField] private GameObject m_endGameBoss;

    private void Update()
    {
        GameState currentState = GameManager.I.M_CurrentState;

        if (currentState == GameState.Playing && m_spawnRoutine == null)
        {
            m_spawnRoutine = StartCoroutine(SpawnEnemiesLoop());
            m_strongerSpawnRoutine = StartCoroutine(SpawnStrongerEnemiesLoop());
            m_miniBossSpawnRoutine = StartCoroutine(SpawnMiniBossLoop());
        }

        else if (currentState != GameState.Playing && m_spawnRoutine != null)
        {
            StopSpawningEnemies();
        }

        if (currentState == GameState.Playing && LevelManager.I.GetGameTime() > 1800f)
        {
            StopSpawningEnemies();
            m_endGameBoss.SetActive(true);
        }
    }

    public void DamageRandomEnemy(float _damage)
    {
        List<Enemy> spawnedEnemiesCopy = new List<Enemy>(m_spawnedEnemies);
        Enemy randomEnemy = spawnedEnemiesCopy[Random.Range(0, spawnedEnemiesCopy.Count)];

        if (randomEnemy)
        {
            randomEnemy.ApplySlow(_damage);
            randomEnemy.TakeDamage(_damage);
        }
    }

    public bool HasEnemiesSpawned()
    {
        return m_spawnedEnemies.Count > 0;
    }

    public void SlowAllEnemies(float _slowDuration)
    {
        List<Enemy> spawnedEnemiesCopy = new List<Enemy>(m_spawnedEnemies);
        foreach (Enemy enemy in spawnedEnemiesCopy)
        {
            if (enemy)
            {
                enemy.ApplySlow(_slowDuration);
            }
        }
    }

    public void KillAllEnemies()
    {
        List<Enemy> spawnedEnemiesCopy = new List<Enemy>(m_spawnedEnemies);
        foreach (Enemy enemy in spawnedEnemiesCopy)
        {
            if (enemy)
            {
                const int oneShotDamage = 999;
                enemy.TakeDamage(oneShotDamage, false);
            }
        }
    }

    private void StopSpawningEnemies()
    {
        StopCoroutine(m_spawnRoutine);
        StopCoroutine(m_strongerSpawnRoutine);
        StopCoroutine(m_miniBossSpawnRoutine);
        m_strongerSpawnRoutine = null;
        m_spawnRoutine = null;
        m_miniBossSpawnRoutine = null;
    }

    private IEnumerator SpawnEnemiesLoop()
    {
        while (true)
        {
            float timeValue = LevelManager.I.GetGameTime();
            float minDelay = m_minSpawnDelayCurve.Evaluate(timeValue);
            float maxDelay = m_maxSpawnDelayCurve.Evaluate(timeValue);
            yield return new WaitForSeconds(Random.Range(minDelay, maxDelay));
            SpawnEnemy();
        }
    }

    private IEnumerator SpawnStrongerEnemiesLoop()
    {
        while (true)
        {
            float timeValue = LevelManager.I.GetGameTime();
            yield return new WaitForSeconds(m_strongerSpawnRateCurve.Evaluate(timeValue));
            SpawnStrongerEnemy();
        }
    }

    private IEnumerator SpawnMiniBossLoop()
    {
        while (true)
        {
            float timeValue = LevelManager.I.GetGameTime();
            yield return new WaitForSeconds(m_miniBossSpawnRate);
            SpawnMiniBoss();
        }
    }

    private void SpawnEnemy()
    {
        m_enemyPrefab = m_enemyPrefabs[Random.Range(0, m_enemyPrefabs.Length)];

        Enemy newEnemy = Instantiate(m_enemyPrefab);
        m_spawnedEnemies.Add(newEnemy);
        newEnemy.transform.position = GetRandomSpawnPoint();
        newEnemy.OnDeath += () => m_spawnedEnemies.Remove(newEnemy);
        if (!m_hasClusterSpawned)
        {
            float gameTime = LevelManager.I.GetGameTime();
            Invoke(nameof(SpawnCluster), Random.Range(m_minClusterSpawnRate.Evaluate(gameTime), m_maxClusterSpawnRate.Evaluate(gameTime)));
            m_hasClusterSpawned = true;
        }
    }

    private void SpawnStrongerEnemy()
    {
        m_strongerEnemyPrefab = m_strongerEnemyPrefabs[Random.Range(0, m_strongerEnemyPrefabs.Length)];

        Enemy newEnemy = Instantiate(m_strongerEnemyPrefab);
        m_spawnedEnemies.Add(newEnemy);
        newEnemy.transform.position = GetRandomSpawnPoint();
        newEnemy.OnDeath += () => m_spawnedEnemies.Remove(newEnemy);
    }

    private void SpawnMiniBoss()
    {
        if (m_miniBossSpawnIndex > m_miniBossPrefabs.Length) return;

        m_miniBossPrefab = m_miniBossPrefabs[m_miniBossSpawnIndex];

        Enemy newEnemy = Instantiate(m_miniBossPrefab);
        m_spawnedEnemies.Add(newEnemy);
        newEnemy.transform.position = m_spawnPoint.position;
        newEnemy.OnDeath += () => m_spawnedEnemies.Remove(newEnemy);

        m_miniBossSpawnIndex++;
    }

    private void SpawnCluster()
    {
        float gameTime = LevelManager.I.GetGameTime();
        int amountOfEnemies = Random.Range((int)m_minClusterSize.Evaluate(gameTime), (int)m_maxClusterSize.Evaluate(gameTime));
        for (int i = 0; i < amountOfEnemies; i++)
        {
            Enemy newEnemy = Instantiate(m_clusterEnemyPrefab);
            m_spawnedEnemies.Add(newEnemy);
            newEnemy.transform.position = GetRandomSpawnPoint();
            newEnemy.OnDeath += () => m_spawnedEnemies.Remove(newEnemy);
        }
        Invoke(nameof(SpawnCluster), Random.Range(m_minClusterSpawnRate.Evaluate(gameTime), m_maxClusterSpawnRate.Evaluate(gameTime)));
    }

    private Vector3 GetRandomSpawnPoint()
    {
        Vector3 point = m_spawnPoint.position;
        point.y = Random.Range(point.y - m_maxSpawnRangeY, point.y + m_maxSpawnRangeY);
        return point;
    }
}