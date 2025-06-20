using UnityEngine;
using UnityEngine.SceneManagement;

public enum GameState
{
    MainMenu, Playing, Paused, Shop, GameOver
}

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    public GameState M_CurrentState { get; private set; }

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

    private void Start()
    {
        ChangeState(GameState.MainMenu);
    }

    public void ChangeState(GameState _newState)
    {
        M_CurrentState = _newState;

        switch (_newState)
        {
            case GameState.MainMenu:
                Time.timeScale = 1f;
                break;

            case GameState.Playing:
                Time.timeScale = 1f;
                Debug.Log("Game is now Playing.");
                break;

            case GameState.Paused:
                Time.timeScale = 0f;
                Debug.Log("Game Paused.");
                break;

            case GameState.Shop:
                Time.timeScale = 0f;
                Debug.Log("Shop Opened.");
                break;

            case GameState.GameOver:
                Time.timeScale = 0f;
                Debug.Log("Game Over.");
                break;
        }

        UIManager.Instance.ToggleUI(M_CurrentState);
    }

    public void StartGame()
    {
        ChangeState(GameState.Playing);
        LevelManager.Instance.StartLevel();
    }

    public void PauseGame()
    {
        if (M_CurrentState == GameState.Playing)
            ChangeState(GameState.Paused);
    }

    public void ResumeGame()
    {
        if (M_CurrentState == GameState.Paused)
            ChangeState(GameState.Playing);
    }

    public void EndGame()
    {
        ChangeState(GameState.GameOver);
    }

    public void RestartLevel()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}
