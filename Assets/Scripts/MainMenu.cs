using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    public Canvas canvas;
    
    public void Start() {

    }

    public void Update() {

    }

    public void PlayGame()
    {
        Cursor.lockState = CursorLockMode.Locked;
        canvas.gameObject.SetActive(false);
        //SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex - 1);
    }

    public void QuitGame()
    {
        print("quitting");
        Application.Quit();
    }
}
