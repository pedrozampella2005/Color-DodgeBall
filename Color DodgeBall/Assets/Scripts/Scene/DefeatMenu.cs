using UnityEngine;
using UnityEngine.SceneManagement;


using UnityEngine;
using UnityEngine.SceneManagement;

public class DefeatMenu : MonoBehaviour
{
    [SerializeField] private string mainMenuSceneName = "MainMenu";

    public void RetryLastLevel()
    {
        if (!string.IsNullOrEmpty(GameFlowData.lastLevelSceneName))
        {
            // al reintentar, arrancás el nivel de nuevo, pero limpiando resultados de la sesión
            GameFlowData.ResetSession();
            SceneManager.LoadScene(GameFlowData.lastLevelSceneName);
        }
        else
        {
            Debug.LogWarning("No hay un ultimo nivel guardado. Volviendo al menu principal.");
            SceneManager.LoadScene(mainMenuSceneName);
        }
    }

    public void GoToMainMenu()
    {
        GameFlowData.ResetSession();
        SceneManager.LoadScene(mainMenuSceneName);
    }
}






//public class DefeatMenu : MonoBehaviour
//{
//    [SerializeField] private string mainMenuSceneName = "MainMenu";

//    public void RetryLastLevel()
//    {
//        if (!string.IsNullOrEmpty(GameFlowData.lastLevelSceneName))
//        {
//            SceneManager.LoadScene(GameFlowData.lastLevelSceneName);
//        }
//        else
//        {
//            Debug.LogWarning("No hay un ultimo nivel guardado. Volviendo al menu principal.");
//            SceneManager.LoadScene(mainMenuSceneName);
//        }
//    }

//    public void GoToMainMenu()
//    {
//        SceneManager.LoadScene(mainMenuSceneName);
//    }
//}
