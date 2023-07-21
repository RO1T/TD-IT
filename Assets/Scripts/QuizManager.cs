using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class QuizManager : Singleton<QuizManager>
{
    private bool gameOver = false;
    [SerializeField]
    private GameObject right;
    [SerializeField]
    private GameObject wrong;
    [SerializeField]
    private GameObject pauseMenu;
    [SerializeField]
    private GameObject optionsMenu;
    [SerializeField]
    private GameObject gameOverMenu;
    private bool isRightAnswer;
    public bool IsRightAnswer { get => isRightAnswer; }

    public void Options()
    {
        if (optionsMenu.activeSelf)
        {
            pauseMenu.SetActive(true);
            optionsMenu.SetActive(false);
        }
        else
        {
            pauseMenu.SetActive(false);
            optionsMenu.SetActive(true);
        }
    }
    public void Restart()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
    public void QuitGame()
    {
        SceneManager.LoadScene(0);
    }
    public void GameOver()
    {
        if (!gameOver)
        {
            gameOver = true;
            gameOverMenu.SetActive(true);
        }
    }
    public void RightAnswer()
    {
        SoundManager.Instance.PlaySfx("CorrectAnswer");
        PlayerPrefs.SetInt("IsRightAnswer", 1);
        right.SetActive(true);
        StartCoroutine(Wait(1f));
    }
    public void WrongAnswer()
    {
        SoundManager.Instance.PlaySfx("WrongAnswer");
        PlayerPrefs.SetInt("IsRightAnswer", 0);
        wrong.SetActive(true);
        StartCoroutine(Wait(1f));
    }
    private void Update()
    {
        HandleEscape();
    }
    private void HandleEscape()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            ShowInGameMenu();
        }
    }
    public void ShowInGameMenu()
    {
        if (optionsMenu.activeSelf)
        {
            ShowMainMenu();
        }
        else
        {
            pauseMenu.SetActive(!pauseMenu.activeSelf);
            if (!pauseMenu.activeSelf)
            {
                Time.timeScale = 1;
            }
            else
            {
                Time.timeScale = 0;
            }
        }
    }
    public void ShowOptions()
    {
        pauseMenu.SetActive(false);
        optionsMenu.SetActive(true);
    }
    public void ShowMainMenu()
    {
        pauseMenu.SetActive(true);
        optionsMenu.SetActive(false);
    }
    private IEnumerator Wait(float seconds)
    {
        yield return new WaitForSeconds(seconds);
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }
}
