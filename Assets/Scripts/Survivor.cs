using UnityEngine;

public class Survivor : MonoBehaviour
{
    [SerializeField] private float m_damage;
    [SerializeField] private float m_fireFrequency;
    [SerializeField] private Animator m_animator;

    private void OnEnable()
    {
        int randomAnimationType = Random.Range(0, 100);
        if (randomAnimationType < 51)
        {
            m_animator.SetInteger("Type", 0);
        }
        else
        {
            m_animator.SetInteger("Type", 1);
        }
    }

    private void Update()
    {
        if (GameManager.I.M_CurrentState != GameState.Playing) return;

        CountFireTimer();
        HandleShoot();
    }

    public void IncreaseFireRate()
    {
        m_fireFrequency -= 1;
        Debug.Log($"`Fire Rate` is now {m_fireFrequency}");
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
        m_animator.SetTrigger("Shoot");
        EnemyManager.I.DamageRandomEnemy(m_damage);
    }
}