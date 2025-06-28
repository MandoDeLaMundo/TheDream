using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Audio;
public class MainMenu : MonoBehaviour
{
    public AudioMixer mainMixer;
  
    public void Options()
    {
        SceneManager.LoadScene("Settings");
    }

    public void Credits()
    {
        SceneManager.LoadScene("Credits");
    }

    public void PlayLv1()
    {
        SceneManager.LoadScene("MainScene");
    }

    public void PlayLv2()
    {
        SceneManager.LoadScene("Forest1");
    }

    public void PlayLv3()
    {
        SceneManager.LoadScene("Showcase Level");
    }

    public void EscToMenu()
    {
        SceneManager.LoadScene("LevelSelect");
    }

    public void SetFullscreen(bool isFullscreen)
    {
        Screen.fullScreen = isFullscreen;
    }

    public void SetQuality(int qualityIndex)
    {
        QualitySettings.SetQualityLevel(qualityIndex);
    } 

    public void SetVolume(float volume)
    {
        mainMixer.SetFloat("volume", volume);
    }
}
