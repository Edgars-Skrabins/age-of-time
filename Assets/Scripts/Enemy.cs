using System;
using System.Collections;
using UnityEngine;
using Random = UnityEngine.Random;

public class Enemy : MonoBehaviour
{
    [SerializeField] private float m_maxHealth;
    [SerializeField] private float m_minTimeGainedOnDeath;
    [SerializeField] private float m_maxTimeGainedOnDeath;
    [SerializeField] private float m_currentHealth;
    [SerializeField] private float m_damage;
    [SerializeField] private float m_attackInterval = 1.5f;
    [SerializeField] private bool m_isBoss;
    [SerializeField] private float m_animationToHitInterval;
    [SerializeField] private float m_comboAnimationInterval;
    [SerializeField] private Vector2 m_minMaxMoveSpeed;
    [SerializeField] private Rigidbody2D m_rigidbody;
    [SerializeField] private GameObject m_popupPrefab;
    [SerializeField] private Animator m_animator;

    public Action OnDeath;
    private Transform m_target;
    private float m_moveSpeed;
    private bool m_isSlowed;
    private bool m_isDead;
    private bool m_isAttacking;

    private void Start()
    {
        m_currentHealth = m_maxHealth;
        m_target = LevelManager.I.m_BaseTarget;
        m_moveSpeed = Random.Range(m_minMaxMoveSpeed.x, m_minMaxMoveSpeed.y);
        m_animator.SetBool("Walking", true);
    }

    private void FixedUpdate()
    {
        if (m_target == null) return;

        if (m_animator.GetBool("Walking") && !m_isDead)
        {
            ApplyVelocity();
        }
    }

    private void ApplyVelocity()
    {
        if (m_isDead)
        {
            m_rigidbody.velocity = Vector2.zero;
            return;
        }

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
            m_animator.SetBool("Walking", false);
            if (!m_isAttacking)
            {
                StartAttack();
            }
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
        SpawnPopup(_amount.ToString(), Color.red);
        m_rigidbody.velocity = Vector2.zero;

        if (m_currentHealth <= 0)
        {
            m_animator.SetBool("Walking", false);
            StopCoroutine(AttackLoop()); // Just in case
            Die(_giveTime);
        }
        else
        {
            m_animator.SetTrigger("Hurt");
        }
    }

    private void Die(bool _giveTime)
    {
        if (m_isDead) return;
        m_rigidbody.velocity = Vector2.zero;

        m_animator.SetTrigger("Dead");
        if (_giveTime)
        {
            ScoreManager.I.AddKill();
            GiveTime();
        }
        Destroy(gameObject, 3f);
        OnDeath?.Invoke();
        m_isDead = true;
    }

    private void StartAttack()
    {
        m_isAttacking = true;
        if (!m_isBoss)
        {
            StartCoroutine(AttackLoop());
        }
        else
        {
            StartCoroutine(ComboAttackLoop());
        }
    }

    private void GiveTime()
    {
        int m_timeGainedOnDeath = Mathf.RoundToInt(Random.Range(m_minTimeGainedOnDeath, m_maxTimeGainedOnDeath));
        if (m_timeGainedOnDeath > 0)
        {
            SpawnPopup("+" + m_timeGainedOnDeath.ToString());
        }
        LevelManager.I.AddTime(m_timeGainedOnDeath);
    }

    private void SpawnPopup(string _value)
    {
        Popup popup = Instantiate(m_popupPrefab, transform.position, Quaternion.identity).GetComponent<Popup>();
        popup.ShowPopup(_value);
    }

    private void SpawnPopup(string _value, Color _color)
    {
        Popup popup = Instantiate(m_popupPrefab, transform.position, Quaternion.identity).GetComponent<Popup>();
        popup.ShowPopup(_value, _color);
    }

    private IEnumerator AttackLoop()
    {
        m_rigidbody.velocity = Vector2.zero;
        while (!m_isDead && m_target != null)
        {
            m_animator.SetTrigger("Attack");

            yield return new WaitForSeconds(m_animationToHitInterval);

            PlayerController.I.TriggerHitAnimation();
            LevelManager.I.RemoveTime(m_damage);

            yield return new WaitForSeconds(m_attackInterval);
        }
    }

    private IEnumerator ComboAttackLoop()
    {
        m_rigidbody.velocity = Vector2.zero;
        while (!m_isDead && m_target != null)
        {
            for (int i = 0; i < 3; i++)
            {
                m_animator.SetTrigger("Attack" + i);

                yield return new WaitForSeconds(m_animationToHitInterval);

                PlayerController.I.TriggerHitAnimation();
                LevelManager.I.RemoveTime(m_damage);
                if (i < 2)
                {
                    yield return new WaitForSeconds(m_comboAnimationInterval);
                }
            }
            yield return new WaitForSeconds(m_attackInterval);
        }
    }
}