using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameOverUI : MonoBehaviour
{
    [SerializeField]
    string mouseHoverSound = "ButtonHover";
    [SerializeField]
    string buttonPressedSound = "ButtonPress";
    AudioManager audioManager;

    private void Start()
    {
        audioManager = AudioManager.instance;
        if (audioManager == null)
        {
            Debug.LogError("No AudioManager Found! ");
        }
    }

    public void Quit()
    {
        audioManager.PlaySound(buttonPressedSound);
        Debug.Log("APPLICATION QUIT!");
        Application.Quit();
    }

    public void Retry()
    {
        //Sound
        audioManager.PlaySound(buttonPressedSound);
        //Scene
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public void OnMouseOver()
    {
        audioManager.PlaySound(mouseHoverSound);
    }
}
