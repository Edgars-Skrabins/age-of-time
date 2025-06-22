using UnityEngine;

public class PlayerController : Singleton<PlayerController>
{
    [SerializeField] private Transform m_gunSpriteObject;
    [SerializeField] private GameObject m_gunEffectObject;
    [SerializeField] private GameObject m_mouseClickEffectPrefab;
    [SerializeField] private float m_damage;
    [SerializeField] private float m_headshotMultiplier;
    [SerializeField] private float m_fireRate;
    [SerializeField] private float m_screenShakeAmount;
    private float m_fireRateTimer;

    protected override void Awake()
    {
        base.Awake();
        m_fireRateTimer = m_fireRate;
    }

    private void Update()
    {
        CountFireRateTimer();

        if (Input.GetMouseButton(0))
        {
            HandleMouseClick();
        }
    }

    public void IncreaseFireRate(float _amount)
    {
        m_fireRate -= _amount;
    }

    public void IncreaseDamage(float _amount)
    {
        m_damage += _amount;
    }

    private void CountFireRateTimer()
    {
        m_fireRateTimer += Time.deltaTime;
    }

    private Vector3 GetMouseWorldPosition()
    {
        Vector3 mouseWorldPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        mouseWorldPosition.z = 0f;
        return mouseWorldPosition;
    }

    private void HandleMouseClick()
    {
        if (m_fireRateTimer >= m_fireRate)
        {
            Shoot();
            m_fireRateTimer = 0f;
        }
    }

    private void Shoot()
    {
        Vector3 mouseWorldPosition = GetMouseWorldPosition();

        SpawnEffect(mouseWorldPosition);
        m_gunEffectObject.SetActive(true);

        RaycastHit2D hit = Physics2D.Raycast(mouseWorldPosition, Vector2.zero);
        if (hit.collider != null)
        {
            Enemy enemy = hit.collider.GetComponentInParent<Enemy>();
            if (enemy != null)
            {
                if (hit.collider.gameObject.layer == LayerMask.NameToLayer("Head"))
                {
                    enemy.TakeDamage(m_damage * m_headshotMultiplier);
                    Debug.Log("Headshot!");
                }
                else
                {
                    enemy.TakeDamage(m_damage);
                }
            }
        }

        ScreenShake.Instance.AddTrauma(m_screenShakeAmount);
    }

    private void SpawnEffect(Vector3 mouseWorldPosition)
    {
        GameObject effect = Instantiate(m_mouseClickEffectPrefab);
        effect.transform.position = mouseWorldPosition;
        effect.SetActive(true);
    }
}