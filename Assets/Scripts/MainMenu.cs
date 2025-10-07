using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    public AudioSource audioSource;   // Speelt het geluid af
    public AudioClip clickSound;      // Het klikgeluid

    public void playGame()
    {
        StartCoroutine(PlaySoundAndLoadScene("test"));
    }

    public void quitGame()
    {
        StartCoroutine(PlaySoundAndQuit());
    }

    private IEnumerator PlaySoundAndLoadScene(string sceneName)
    {
        if (audioSource && clickSound)
        {
            audioSource.PlayOneShot(clickSound);
            yield return new WaitForSeconds(clickSound.length); // wacht tot het geluid klaar is
        }

        SceneManager.LoadScene(sceneName);
    }

    private IEnumerator PlaySoundAndQuit()
    {
        if (audioSource && clickSound)
        {
            audioSource.PlayOneShot(clickSound);
            yield return new WaitForSeconds(clickSound.length);
        }

        Debug.Log("Quit Game pressed");
        Application.Quit();
    }

    public void GoToMainMenu()
    {
        Time.timeScale = 1.0f;
        SceneManager.LoadScene("MainMenuScene");

    }
}

