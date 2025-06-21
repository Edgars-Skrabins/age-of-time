using UnityEngine;

public class Survivor : MonoBehaviour
{
    [SerializeField] private float m_fireFrequency;
    [SerializeField] private Transform m_firePoint;
    [SerializeField] private GameObject m_bulletPrefab;

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
        if (m_fireTimer >= m_fireFrequency)
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
        Instantiate(m_bulletPrefab, m_firePoint.position, m_firePoint.rotation);
    }
}