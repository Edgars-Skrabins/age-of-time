using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [SerializeField] private Transform m_gunSpriteObject;
    [SerializeField] private GameObject m_gunEffectObject;
    [SerializeField] private GameObject m_mouseClickEffectPrefab;
    [SerializeField] private float m_damage = 25f;

    void Update()
    {
        TrackMouse();

        if (Input.GetMouseButtonDown(0))
        {
            HandleMouseClick();
        }
    }

    void TrackMouse()
    {
        Vector3 mouseScreenPosition = Input.mousePosition;
        Vector3 mouseWorldPosition = Camera.main.ScreenToWorldPoint(mouseScreenPosition);
        mouseWorldPosition.z = 0f;

        Vector3 direction = mouseWorldPosition - m_gunSpriteObject.position;
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;

        m_gunSpriteObject.rotation = Quaternion.Euler(0f, 0f, angle);
    }

    void HandleMouseClick()
    {
        Vector3 mouseWorldPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        mouseWorldPosition.z = 0f;

        // Click effect
        SpawnEffect(mouseWorldPosition);
        m_gunEffectObject.SetActive(true);

        // Raycast to detect enemy
        RaycastHit2D hit = Physics2D.Raycast(mouseWorldPosition, Vector2.zero);
        if (hit.collider != null)
        {
            Enemy enemy = hit.collider.GetComponent<Enemy>();
            if (enemy != null)
            {
                enemy.TakeDamage(m_damage);
            }
        }
    }

    private void SpawnEffect(Vector3 mouseWorldPosition)
    {
        GameObject effect = Instantiate(m_mouseClickEffectPrefab);
        effect.transform.position = mouseWorldPosition;
        effect.SetActive(true);
    }
}
