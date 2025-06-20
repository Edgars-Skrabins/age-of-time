using System.Collections;
using UnityEngine;

public class EnemyManager : MonoBehaviour
{
    [SerializeField] private GameObject m_enemyPrefab;
    [SerializeField] private Transform m_spawnPoint;
    [SerializeField] private float m_maxSpawnRangeY;
    [SerializeField] private float m_minSpawnDelay;
    [SerializeField] private float m_maxSpawnDelay;

    private Coroutine m_spawnRoutine;
    private GameState lastState;

    private void Update()
    {
        GameState currentState = GameManager.Instance.M_CurrentState;

        if (currentState == GameState.Playing && m_spawnRoutine == null)
        {
            m_spawnRoutine = StartCoroutine(SpawnEnemiesLoop());
        }
        else if (currentState != GameState.Playing && m_spawnRoutine != null)
        {
            StopCoroutine(m_spawnRoutine);
            m_spawnRoutine = null;
        }

        lastState = currentState;
    }

    private IEnumerator SpawnEnemiesLoop()
    {
        while (true)
        {
            yield return new WaitForSeconds(Random.Range(m_minSpawnDelay, m_maxSpawnDelay));
            SpawnEnemy();
        }
    }

    private void SpawnEnemy()
    {
        GameObject newEnemy = Instantiate(m_enemyPrefab);
        newEnemy.transform.position = GetRandomSpawnPoint();
    }

    private Vector3 GetRandomSpawnPoint()
    {
        Vector3 point = m_spawnPoint.position;
        point.y = Random.Range(point.y - m_maxSpawnRangeY, point.y + m_maxSpawnRangeY);
        return point;
    }
}
