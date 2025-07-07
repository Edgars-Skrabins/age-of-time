using UnityEngine;
using UnityEngine.UI;

public class HealthBar : MonoBehaviour
{
    [SerializeField] private Slider m_healthSlider;

    [SerializeField] private Enemy m_enemy;

    private void OnEnable()
    {
        m_healthSlider.minValue = 0;
        m_healthSlider.maxValue = m_enemy.GetMaxHealth();
        m_healthSlider.value = m_enemy.GetCurrentHealth();
    }

    private void LateUpdate()
    {
        m_healthSlider.value = m_enemy.GetCurrentHealth();
    }
}