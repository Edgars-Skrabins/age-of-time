using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    public static UIManager Instance;

    [Header("UI Panels")]
    [SerializeField] private GameObject m_menuUI;
    [SerializeField] private GameObject m_gameUI;
    [SerializeField] private GameObject m_shopUI;
    [SerializeField] private GameObject m_pauseUI;
    [SerializeField] private GameObject m_gameoverUI;
    [Space]
    [SerializeField] private Slider m_currentTimeSlider;
    [SerializeField] private TMP_Text m_currentTimeText;


    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void ToggleUI(GameState _state)
    {
        CloseAllUIPanels();

        switch (_state)
        {
            case GameState.MainMenu:
                m_menuUI.SetActive(true);
                break;
            case GameState.Playing:
                m_gameUI.SetActive(true);
                break;
            case GameState.Paused:
                m_pauseUI.SetActive(true);
                break;
            case GameState.Shop:
                m_shopUI.SetActive(true);
                break;
            case GameState.GameOver:
                m_gameoverUI.SetActive(true);
                break;
        }
    }

    private void CloseAllUIPanels()
    {
        m_gameUI.SetActive(false);
        m_gameoverUI.SetActive(false);
        m_menuUI.SetActive(false);
        m_pauseUI.SetActive(false);
        m_shopUI.SetActive(false);
    }

    public void InitializeGameUI(float _maxTimeValue)
    {
        m_currentTimeSlider.minValue = 0f;
        m_currentTimeSlider.maxValue = _maxTimeValue;
        m_currentTimeSlider.value = _maxTimeValue;
        m_currentTimeText.text = Mathf.FloorToInt(_maxTimeValue).ToString();
    }

    public void UpdateTime(float _value)
    {
        m_currentTimeSlider.value = _value;
        m_currentTimeText.text = Mathf.FloorToInt(_value).ToString();
    }

    public void UpdateMaxTime(float _value)
    {
        m_currentTimeSlider.maxValue = _value;
    }
}
