using UnityEngine;
using UnityEngine.SceneManagement;

#if UNITY_EDITOR
using UnityEditor;
#endif

public class VictoryMenu : MonoBehaviour
{
    [SerializeField] private string mainMenuSceneName = "MainMenu";
    [SerializeField] private string nextLevelSceneName = "Level3";

    public void GoToNextLevel()
    {
        SceneManager.LoadScene(nextLevelSceneName);
    }

    public void GoToMainMenu()
    {
        SceneManager.LoadScene(mainMenuSceneName);
    }

    public void RetryLastLevel()
    {
        if (!string.IsNullOrEmpty(GameFlowData.lastLevelSceneName))
        {
            SceneManager.LoadScene(GameFlowData.lastLevelSceneName);
        }
        else
        {
            SceneManager.LoadScene(mainMenuSceneName);
        }
    }

    public void ExitGame()
    {
        Debug.Log("Saliendo del juego...");

#if UNITY_EDITOR
        
        EditorApplication.isPlaying = false;
#else
        // En una build, esto sí cierra la aplicación.
        Application.Quit();
#endif
    }
}