using UnityEngine;
using UnityEngine.SceneManagement;

public enum GameState
{
    MainMenu, Playing, Paused, Shop, GameOver
}

public class GameManager : Singleton<GameManager>
{
    public GameState M_CurrentState { get; private set; }

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
        PlayerController.I.HandleAnimation();
    }

    public void StartGame()
    {
        ChangeState(GameState.Playing);
        LevelManager.I.StartLevel();
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
