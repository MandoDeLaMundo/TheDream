using UnityEngine;
using UnityEngine.SceneManagement;

public class buttonFunctions : MonoBehaviour
{
	public void Resume()
	{
		gameManager.instance.StateUnpause();
	}

	public void Restart()
	{
		SceneManager.LoadScene(SceneManager.GetActiveScene().name);
		gameManager.instance.StateUnpause();
	}

	public void Quit()
	{
#if UNITY_EDITOR
		UnityEditor.EditorApplication.isPlaying = false;

#else
        Application.Quit();

#endif
	}
}
