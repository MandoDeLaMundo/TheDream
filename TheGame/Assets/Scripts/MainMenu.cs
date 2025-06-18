using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    public GameObject credits;
    public GameObject x;
    public void PlayGame()
  {
    SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
  }

    public void Options()
    {

    }

    public void PlayLv1()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }

    public void PlayLv2()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 2);
    }

    public void PlayLv3()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 3);
    }

    public void CreditsMenu()
    {
        if (credits.activeInHierarchy == false)
            credits.SetActive(true);
        else
            credits.SetActive(false);
    }

    public void exitCreditsMenu()
    {
        if (credits.activeInHierarchy == true)
            credits.SetActive(false);
        else
            credits.SetActive(true);
    }
}
