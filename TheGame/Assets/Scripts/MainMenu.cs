using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
  public void PlayGame()
  {
    SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
  }

    public void Options()
    {

    }

    public GameObject credits;
    public GameObject x;

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
