using UnityEngine;

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

    private Transform m_target;
    private float m_moveSpeed;

    private void Start()
    {
        m_currentHealth = m_maxHealth;
        m_target = LevelManager.Instance.m_BaseTarget;
        m_moveSpeed = Random.Range(m_minMaxMoveSpeed.x, m_minMaxMoveSpeed.y);
    }

    private void FixedUpdate()
    {
        if (m_target == null) return;

        Vector2 direction = ((Vector2)m_target.position - m_rigidbody.position).normalized;
        m_rigidbody.velocity = direction * m_moveSpeed;
    }

    private void OnTriggerEnter2D(Collider2D _other)
    {
        if (_other.transform == m_target)
        {
            LevelManager.Instance.RemoveTime(m_damage);

            Destroy(gameObject);
        }
    }

    public void TakeDamage(float _amount)
    {
        m_currentHealth -= _amount;
        if (m_currentHealth <= 0)
        {
            int m_timeGainedOnDeath = Mathf.RoundToInt(Random.Range(m_minTimeGainedOnDeath, m_maxTimeGainedOnDeath));
            if (m_timeGainedOnDeath > 0)
            {
                SpawnPopup(m_timeGainedOnDeath);
            }
            LevelManager.Instance.AddTime(m_timeGainedOnDeath);
            Destroy(gameObject);
        }
    }

    private void SpawnPopup(int _value)
    {
        Popup popup = Instantiate(m_popupPrefab, transform.position, Quaternion.identity).GetComponent<Popup>();
        popup.ShowPopup(_value);
    }
}