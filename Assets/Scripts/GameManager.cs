using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public enum GameState
{
    MainMenu,
    Playing,
    Paused,
    GameOver
}

public class GameManager : Singleton<GameManager>
{
    public GameState M_CurrentState { get; private set; }

    private void Start()
    {
        ChangeState(GameState.MainMenu);
        AudioManager.I.PlaySound("BGM_MainMenu");
        VoiceoverManager.I.Play("Menu_Intro");
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
                break;

            case GameState.Paused:
                Time.timeScale = 0f;
                break;

            case GameState.GameOver:
                Time.timeScale = 0f;
                break;
        }

        UIManager.Instance.ToggleUI(M_CurrentState);
        PlayerController.I.HandleAnimation();
    }

    public void StartGame()
    {
        ChangeState(GameState.Playing);
        AudioManager.I.StopSound("BGM_MainMenu");
        AudioManager.I.StopSound("BGM_JazzMainMenu");
        AudioManager.I.PlaySound("SFX_ShotgunCock");
        LevelManager.I.StartLevel();
        VoiceoverManager.I.Play("Game_Intro");
        AudioManager.I.PlaySound(!AudioManager.I.JazzMode() ? "BGM_GameMusic" + Random.Range(0, 2) : "BGM_JazzGameMusic");
    }

    public void PauseGame()
    {
        if (M_CurrentState == GameState.Playing) ChangeState(GameState.Paused);
    }

    public void ResumeGame()
    {
        if (M_CurrentState == GameState.Paused) ChangeState(GameState.Playing);
    }

    public void EndGame()
    {
        ChangeState(GameState.GameOver);
    }

    public void RetryLevel()
    {
        StartCoroutine(nameof(RetryGameCO));
    }

    public void RestartLevel()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    private IEnumerator RetryGameCO()
    {
        RestartLevel();
        yield return new WaitForSecondsRealtime(0.5f);
        StartGame();
    }
}