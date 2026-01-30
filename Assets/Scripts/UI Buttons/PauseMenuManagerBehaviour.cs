using UnityEngine;

public class PauseMenuManagerBehaviour : MonoBehaviour
{
    public GameObject pauseMenu;
    public GameObject controlMenu;
    public FirstPersonPlayerMovementBehaviour cameraScript;
    public static bool isPaused = false;

    void Awake()
    {
        pauseMenu.SetActive(false);
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
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
        pauseMenu.SetActive(false);
        controlMenu.SetActive(false);
        Time.timeScale = 1f;
        isPaused = false;
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        cameraScript.enabled = true;
    }

    void Pause()
    {
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
        cameraScript.enabled = false;
        pauseMenu.SetActive(true);
        Time.timeScale = 0f;
        isPaused = true;
    }

    public void Quit()
    {
        Application.Quit();
    }

    public void Controls()
    {
        pauseMenu.SetActive(false);
        controlMenu.SetActive(true);
    }
}
