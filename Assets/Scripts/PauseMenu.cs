using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
//using UnityEngine.SceneManager;

public class PauseMenu : MonoBehaviour
{

    // Development for Pause menu is based on tutorial video made by Brackeys in YouTube

    public static bool isPaused = false;
    public GameObject pauseMenuUI;
    public GameObject image;

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            Debug.Log("Escape key is pressed");
            if (isPaused)
            {
                Resume();
            }
            else
            {
                Pause();
            }
        }
    }

    public void Resume()
    {
        Debug.Log("Resume playing");
        pauseMenuUI.SetActive(false);
        image.SetActive(false);
        Time.timeScale = 1f;
        isPaused = false;
    }

    public void Pause()
    {
        Debug.Log("Pausing the game");
        pauseMenuUI.SetActive(true);
        image.SetActive(true);
        Time.timeScale = 0f;
        isPaused = true;
    }

    public void QuitMenu()
    {
        Debug.Log("An empty button that can be used for connecting the main menu");
        //SceneManager.LoadScene("MainMenu");
    }
}
