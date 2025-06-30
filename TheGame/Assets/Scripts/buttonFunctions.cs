using UnityEngine;
using UnityEngine.SceneManagement;

public class buttonFunctions : MonoBehaviour
{
    public void Resume()
    {
        gameManager.instance.StateUnpause();
    }

    public void NextLevel()
    {
        if (SceneManager.GetActiveScene().name == "MainScene")
        {
            SceneManager.LoadScene("Forest1");
        }
        else if (SceneManager.GetActiveScene().name == "Forest1")
        {
            SceneManager.LoadScene("Showcase Level");
        }
        else if (SceneManager.GetActiveScene().name == "Showcase Level")
        {
            SceneManager.LoadScene("LevelSelect");
        }
    }

    public void Restart()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
            gameManager.instance.StateUnpause();
    }

    public void Quit()
    {
        if (SceneManager.GetActiveScene().name == "MainScene" || SceneManager.GetActiveScene().name == "Forest1" || SceneManager.GetActiveScene().name == "Showcase Level")
        {
            SceneManager.LoadScene("LevelSelect");
            Cursor.visible = true;
            Cursor.lockState = CursorLockMode.None;
        }
        else
        {

#if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false;

#else
        Application.Quit();

#endif
        }
    }
}
