using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseMenuController : MonoBehaviour
{
    public GameObject pausePanel;
    public static bool isPaused = false;

    // Start is called before the first frame update
    void Start()
    {
        Time.timeScale = 1;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (!isPaused) //isPaused == false
            {
                isPaused = true;
                pausePanel.SetActive(true);
                Time.timeScale = 0;
            }
            else
            {
                isPaused = false;
                pausePanel.SetActive(false);
                Time.timeScale = 1;
            }
        }
    }
    public void StartButton()
    {
        SceneManager.LoadScene("Game");
    }
    public void QuitButton()
    {
        Application.Quit();
    }
    public void SetVolume(float value) //funcion para que funcione el slider del sonido; cuando se asinga en unity hay que poner el set volume de arriba
    {
        AudioListener.volume = value;
    }
}

