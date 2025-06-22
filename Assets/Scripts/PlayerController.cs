using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [SerializeField] private Transform m_gunSpriteObject;
    [SerializeField] private GameObject m_gunEffectObject;
    [SerializeField] private GameObject m_mouseClickEffectPrefab;
    [SerializeField] private float m_damage = 25f;
    [SerializeField] private float m_fireRate;
    private float m_fireRateTimer;

    private void Awake()
    {
        m_fireRateTimer = m_fireRate;
    }

    private void Update()
    {
        TrackMouse();
        CountFireRateTimer();

        if (Input.GetMouseButton(0))
        {
            HandleMouseClick();
        }
    }


    private void CountFireRateTimer()
    {
        m_fireRateTimer += Time.deltaTime;
    }

    private void TrackMouse()
    {
        Vector3 mouseScreenPosition = Input.mousePosition;
        Vector3 mouseWorldPosition = Camera.main.ScreenToWorldPoint(mouseScreenPosition);
        mouseWorldPosition.z = 0f;

        Vector3 direction = mouseWorldPosition - m_gunSpriteObject.position;
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;

        m_gunSpriteObject.rotation = Quaternion.Euler(0f, 0f, angle);
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
            Enemy enemy = hit.collider.GetComponent<Enemy>();
            if (enemy != null)
            {
                enemy.TakeDamage(m_damage);
            }
        }

        ScreenShake.Instance.AddTrauma(.25f);
    }

    private void SpawnEffect(Vector3 mouseWorldPosition)
    {
        GameObject effect = Instantiate(m_mouseClickEffectPrefab);
        effect.transform.position = mouseWorldPosition;
        effect.SetActive(true);
    }
}