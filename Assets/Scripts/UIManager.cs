using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    public static UIManager Instance;
    private float m_maxTime;
    private bool m_hasTimeOverflowed;

    [SerializeField] private float m_timeUnderflowDrainSpeed;
    [SerializeField] private float m_timeOverflowDrainSpeed;

    [Header("UI Panels")]
    [SerializeField] private GameObject m_menuUI;
    [SerializeField] private GameObject m_gameUI;
    [SerializeField] private GameObject m_pauseUI;
    [SerializeField] private GameObject m_gameoverUI;
    //[SerializeField] private GameObject m_settingsUI;
    [Space]
    [SerializeField] private Slider m_currentTimeSlider;
    [SerializeField] private Image m_sliderFill;
    [SerializeField] private Image m_sliderFillAccent;
    [SerializeField] private TMP_Text m_currentTimeText;


    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            //DontDestroyOnLoad(gameObject);
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
    }

    public void InitializeGameUI(float _maxTimeValue)
    {
        m_maxTime = _maxTimeValue;
        m_currentTimeSlider.minValue = 0f;
        m_currentTimeSlider.maxValue = _maxTimeValue;
        m_currentTimeSlider.value = _maxTimeValue;
        m_currentTimeText.text = Mathf.FloorToInt(_maxTimeValue).ToString();
    }

    public void UpdateTime(float _value)
    {
        m_currentTimeText.text = Mathf.FloorToInt(_value).ToString();
        if (_value > m_maxTime)
        {
            SetTimeOverflowSettings();
            return;
        }
        m_currentTimeSlider.value = _value;
        SetTimeUnderflowSettings();
    }

    private void SetTimeOverflowSettings()
    {
        if (m_hasTimeOverflowed)
        {
            return;
        }
        LevelManager.I.SetTimeReductionMultiplier(m_timeOverflowDrainSpeed);
        m_sliderFill.color = Color.magenta;
        m_sliderFillAccent.color = Color.magenta;
        m_currentTimeText.color = Color.magenta;
        m_currentTimeSlider.value = m_maxTime;

        m_hasTimeOverflowed = true;
    }

    private void SetTimeUnderflowSettings()
    {
        if (!m_hasTimeOverflowed)
        {
            return;
        }
        LevelManager.I.SetTimeReductionMultiplier(m_timeUnderflowDrainSpeed);
        m_sliderFill.color = Color.green;
        m_sliderFillAccent.color = Color.green;
        m_currentTimeText.color = Color.white;
        m_hasTimeOverflowed = false;
    }
}