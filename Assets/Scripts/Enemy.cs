using System;
using UnityEngine;
using Random = UnityEngine.Random;

public class Enemy : MonoBehaviour
{
    [SerializeField] private float m_maxHealth;
    [SerializeField] private float m_minTimeGainedOnDeath;
    [SerializeField] private float m_maxTimeGainedOnDeath;
    [SerializeField] private float m_currentHealth;
    [SerializeField] private float m_damage;

    [SerializeField] private Vector2 m_minMaxMoveSpeed;
    [SerializeField] private Rigidbody2D m_rigidbody;
    [SerializeField] private GameObject m_popupPrefab;

    public Action OnDeath;
    private Transform m_target;
    private float m_moveSpeed;
    private bool m_isSlowed;

    private void Start()
    {
        m_currentHealth = m_maxHealth;
        m_target = LevelManager.I.m_BaseTarget;
        m_moveSpeed = Random.Range(m_minMaxMoveSpeed.x, m_minMaxMoveSpeed.y);
    }

    private void FixedUpdate()
    {
        if (m_target == null) return;
        ApplyVelocity();
    }

    private void ApplyVelocity()
    {
        Vector2 direction = ((Vector2)m_target.position - m_rigidbody.position).normalized;
        if (m_isSlowed)
        {
            m_rigidbody.velocity = direction * (m_moveSpeed * 0.5f);
            return;
        }

        m_rigidbody.velocity = direction * m_moveSpeed;
    }

    private void OnTriggerEnter2D(Collider2D _other)
    {
        if (_other.transform == m_target)
        {
            LevelManager.I.RemoveTime(m_damage);
            PlayerController.I.TriggerHitAnimation();
            Destroy(gameObject);
        }
    }

    public void ApplySlow(float _slowDuration)
    {
        m_isSlowed = true;
        Invoke(nameof(ResetSlow), _slowDuration);
    }

    private void ResetSlow()
    {
        m_isSlowed = false;
    }

    public void TakeDamage(float _amount, bool _giveTime = true)
    {
        m_currentHealth -= _amount;
        if (m_currentHealth <= 0)
        {
            Die(_giveTime);
        }
    }

    private void Die(bool _giveTime)
    {
        if (_giveTime)
        {
            GiveTime();
        }
        Destroy(gameObject);
        OnDeath?.Invoke();
    }

    private void GiveTime()
    {
        int m_timeGainedOnDeath = Mathf.RoundToInt(Random.Range(m_minTimeGainedOnDeath, m_maxTimeGainedOnDeath));
        if (m_timeGainedOnDeath > 0)
        {
            SpawnPopup(m_timeGainedOnDeath);
        }
        LevelManager.I.AddTime(m_timeGainedOnDeath);
    }

    private void SpawnPopup(int _value)
    {
        Popup popup = Instantiate(m_popupPrefab, transform.position, Quaternion.identity).GetComponent<Popup>();
        popup.ShowPopup(_value);
    }
}