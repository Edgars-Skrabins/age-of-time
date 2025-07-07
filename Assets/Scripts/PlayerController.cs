using UnityEngine;

public class PlayerController : Singleton<PlayerController>
{
    [SerializeField] private Transform m_gunSpriteObject;
    [SerializeField] private GameObject m_gunEffectObject;
    [SerializeField] private GameObject m_mouseClickEffectPrefab;
    [SerializeField] private Animator m_playerAnimator;
    [SerializeField] private UICursor m_cursor;
    [SerializeField] private float m_damage;
    [SerializeField] private float m_hitSlowDuration;
    [SerializeField] private float m_headshotMultiplier;
    [SerializeField] private float m_fireRate;
    [SerializeField] private float m_screenShakeAmount;
    [SerializeField] private Camera m_playerCamera;
    private float m_fireRateTimer;

    protected override void Awake()
    {
        base.Awake();
        m_fireRateTimer = m_fireRate;
    }

    private void Update()
    {
        if (GameManager.I.M_CurrentState != GameState.Playing) return;

        CountFireRateTimer();

        if (Input.GetMouseButton(0))
        {
            HandleMouseClick();
        }
    }

    public void HandleAnimation()
    {
        if (GameManager.I.M_CurrentState == GameState.MainMenu)
        {
            m_playerAnimator.SetTrigger("Idle");
        }
        if (GameManager.I.M_CurrentState == GameState.GameOver)
        {
            VoiceoverManager.I.Play("Player_Death");
            m_playerAnimator.SetTrigger("Dead");
        }
        if (GameManager.I.M_CurrentState == GameState.Playing)
        {
            m_playerAnimator.SetTrigger("Ready");
        }
    }

    public void TriggerHitAnimation()
    {
        m_playerAnimator.SetTrigger("Hurt");
        VoiceoverManager.I.Play("Player_Hurt");
    }

    public void IncreaseFireRate(float _amount)
    {
        m_fireRate -= _amount;
    }

    public bool CanIncreaseFireRate()
    {
        return m_fireRate > 0.02f;
    }

    public void IncreaseDamage(float _amount)
    {
        m_damage += _amount;
    }

    public bool CanIncreaseDamage()
    {
        return m_damage < 35;
    }

    private void CountFireRateTimer()
    {
        m_fireRateTimer += Time.deltaTime;
    }

    private Vector3 GetMouseWorldPosition()
    {
        Vector3 mouseWorldPosition = m_playerCamera.ScreenToWorldPoint(Input.mousePosition);
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
        Cursor.visible = false;

        Vector3 mouseWorldPosition = GetMouseWorldPosition();

        SpawnEffect(mouseWorldPosition);
        m_gunEffectObject.SetActive(true);
        m_playerAnimator.SetTrigger("Shoot");
        AudioManager.I.PlaySound("SFX_ShotgunFire");
        m_cursor.Click();

        RaycastHit2D hit = Physics2D.Raycast(mouseWorldPosition, Vector2.zero);
        if (hit.collider != null)
        {
            Enemy enemy = hit.collider.GetComponentInParent<Enemy>();
            if (enemy != null)
            {
                if (hit.collider.gameObject.name == "Head")
                {
                    enemy.ApplySlow(m_hitSlowDuration);
                    enemy.TakeDamage(m_damage * m_headshotMultiplier);
                    ScoreManager.I.AddHeadshot();
                }
                else
                {
                    enemy.ApplySlow(m_hitSlowDuration);
                    enemy.TakeDamage(m_damage);
                }
            }
        }

        ScreenShake.I.AddTrauma(m_screenShakeAmount);
    }

    private void SpawnEffect(Vector3 mouseWorldPosition)
    {
        GameObject effect = Instantiate(m_mouseClickEffectPrefab);
        effect.transform.position = mouseWorldPosition;
        effect.SetActive(true);
    }
}