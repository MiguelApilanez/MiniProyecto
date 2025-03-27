using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuController : MonoBehaviour
{
    private AudioSource sonido;
    public AudioClip clickAudio;
    void Start()
    {
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
        sonido = GetComponent<AudioSource>();

    }
    public void StartButton()
    {
        StartCoroutine(ChangeSceneAfterDelay(0.6f));
        Debug.Log("Empieza el juego");
    }
    public void ExitButton()
    {
        Application.Quit();
        Debug.Log("Se cierra el juego");
    }
    public void ClickAudioOn()
    {
        sonido.PlayOneShot(clickAudio);
    }
    IEnumerator ChangeSceneAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay);
        SceneManager.LoadScene("GameScene");
    }
}
