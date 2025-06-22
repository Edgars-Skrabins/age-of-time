using UnityEngine;

public class Survivor : MonoBehaviour
{
    [SerializeField] private float m_damage;
    [SerializeField] private float m_fireFrequency;

    private void Update()
    {
        CountFireTimer();
        HandleShoot();
    }

    private float m_fireTimer;

    private void CountFireTimer()
    {
        m_fireTimer += Time.deltaTime;
    }

    private void HandleShoot()
    {
        if (m_fireTimer >= m_fireFrequency && EnemyManager.I.HasEnemiesSpawned())
        {
            ResetFireTimer();
            Shoot();
        }
    }

    private void ResetFireTimer()
    {
        m_fireTimer = 0f;
    }

    private void Shoot()
    {
        EnemyManager.I.DamageRandomEnemy(m_damage);
    }
}