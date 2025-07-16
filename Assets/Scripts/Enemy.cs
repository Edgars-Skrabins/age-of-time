using System;
using System.Collections;
using UnityEngine;
using Random = UnityEngine.Random;

public class Enemy : MonoBehaviour
{
    [SerializeField] private float m_maxHealth;
    [SerializeField] private float m_minTimeGainedOnDeath;
    [SerializeField] private float m_maxTimeGainedOnDeath;
    [SerializeField] private float m_minTimeGainedOnDeathInLateGame;
    [SerializeField] private float m_maxTimeGainedOnDeathInLateGame;
    [SerializeField] private float m_currentHealth;
    [SerializeField] private float m_damage;
    [SerializeField] private float m_attackInterval = 1.5f;
    [SerializeField] private bool m_isBoss;
    [SerializeField] private float m_animationToHitInterval;
    [SerializeField] private float m_comboAnimationInterval;
    [SerializeField] private Vector2 m_minMaxMoveSpeed;
    [SerializeField] private Vector2 m_minMaxMoveSpeedLateGame;
    [SerializeField] private Rigidbody2D m_rigidbody;
    [SerializeField] private GameObject m_popupPrefab;
    [SerializeField] private Animator m_animator;
    [SerializeField] private bool m_isGameKiller;

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
        m_moveSpeed = LevelManager.I.IsLateGame()
            ? Random.Range(m_minMaxMoveSpeedLateGame.x, m_minMaxMoveSpeedLateGame.y)
            : Random.Range(m_minMaxMoveSpeed.x, m_minMaxMoveSpeed.y);
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
            m_rigidbody.velocity = direction * (m_moveSpeed * 0.35f);
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

    public float GetMaxHealth()
    {
        return m_maxHealth;
    }

    public float GetCurrentHealth()
    {
        return m_currentHealth;
    }

    public void TakeDamage(float _amount, bool _giveTime = true)
    {
        m_currentHealth -= _amount;
        m_rigidbody.velocity = Vector2.zero;

        if (m_currentHealth <= 0)
        {
            VoiceoverManager.I.Play("Zombie_Dead", new Vector2(0.75f, 1.5f));
            m_animator.SetBool("Walking", false);
            StopCoroutine(AttackLoop()); // Just in case
            Die(_giveTime);
        }
        else
        {
            m_animator.SetTrigger("Hurt");
            VoiceoverManager.I.Play("Zombie_Hurt", new Vector2(0.75f, 1.5f));
            SpawnPopup(_amount.ToString(), Color.red);
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
        if (m_isBoss)
        {
            ScoreManager.I.AddBonusPoints();
            if (m_isGameKiller) GameManager.I.WinGame();
        }
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
        int timeGainedOnDeath = Mathf.RoundToInt(
            LevelManager.I.IsLateGame()
                ? Random.Range(m_minTimeGainedOnDeathInLateGame, m_maxTimeGainedOnDeathInLateGame)
                : Random.Range(m_minTimeGainedOnDeath, m_maxTimeGainedOnDeath)
        );

        if (timeGainedOnDeath > 0)
        {
            SpawnPopup("+" + timeGainedOnDeath);
        }
        LevelManager.I.AddTime(timeGainedOnDeath);
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
            VoiceoverManager.I.Play("Zombie_Attack", new Vector2(0.75f, 1.5f));

            yield return new WaitForSeconds(m_animationToHitInterval);

            ScreenFlash.I.TriggerFlash();
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
                VoiceoverManager.I.Play("Zombie_Attack", new Vector2(0.75f, 1.5f));

                yield return new WaitForSeconds(m_animationToHitInterval);

                ScreenFlash.I.TriggerFlash();
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