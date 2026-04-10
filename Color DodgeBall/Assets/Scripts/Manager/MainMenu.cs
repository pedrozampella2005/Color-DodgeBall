using System.Collections.Generic;
using System.IO;
using System.Text.RegularExpressions;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    private class LevelData
    {
        public string sceneName;
        public int levelNumber;
        public int buildIndex;
    }

    private List<LevelData> levels = new List<LevelData>();

    void Awake()
    {
        BuscarNivelesAutomaticamente();
    }

    private void BuscarNivelesAutomaticamente()
    {
        levels.Clear();

        for (int i = 0; i < SceneManager.sceneCountInBuildSettings; i++)
        {
            string scenePath = SceneUtility.GetScenePathByBuildIndex(i);
            string sceneName = Path.GetFileNameWithoutExtension(scenePath);

            int numeroNivel;
            if (EsNivelValido(sceneName, out numeroNivel))
            {
                LevelData newLevel = new LevelData();
                newLevel.sceneName = sceneName;
                newLevel.levelNumber = numeroNivel;
                newLevel.buildIndex = i;

                levels.Add(newLevel);
            }
        }

        levels.Sort((a, b) => a.levelNumber.CompareTo(b.levelNumber));

        Debug.Log("Niveles encontrados automaticamente: " + levels.Count);
        for (int i = 0; i < levels.Count; i++)
        {
            Debug.Log("Nivel " + levels[i].levelNumber + " -> " + levels[i].sceneName);
        }
    }

    private bool EsNivelValido(string sceneName, out int numeroNivel)
    {
        numeroNivel = -1;

        //busca la esena de forma automatica 

        Match match = Regex.Match(sceneName, @"^level[\s_-]*(\d+)$", RegexOptions.IgnoreCase);

        if (match.Success)
        {
            numeroNivel = int.Parse(match.Groups[1].Value);
            return true;
        }

        return false;
    }

    public void PlayGame()
    {
        if (levels.Count == 0)
        {
            Debug.LogWarning("No se encontraron escenas de nivel en Build Settings.");
            return;
        }

        SceneManager.LoadScene(levels[0].sceneName);
    }

    public void LoadLevelByNumber(int levelNumber)
    {
        for (int i = 0; i < levels.Count; i++)
        {
            if (levels[i].levelNumber == levelNumber)
            {
                SceneManager.LoadScene(levels[i].sceneName);
                return;
            }
        }

        Debug.LogWarning("No existe un nivel con numero: " + levelNumber);
    }

    public void LoadNextLevel()
    {
        string currentSceneName = SceneManager.GetActiveScene().name;

        int currentLevelNumber;
        if (!EsNivelValido(currentSceneName, out currentLevelNumber))
        {
            Debug.LogWarning("La escena actual no es un nivel valido.");
            return;
        }

        for (int i = 0; i < levels.Count; i++)
        {
            if (levels[i].levelNumber == currentLevelNumber + 1)
            {
                SceneManager.LoadScene(levels[i].sceneName);
                return;
            }
        }

        Debug.Log("No hay mas niveles. Volviendo al menu principal.");
        SceneManager.LoadScene("MainMenu");
    }

    public void ReloadCurrentLevel()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public void QuitGame()
    {
        Debug.Log("Saliendo del juego...");
        Application.Quit();
    }
}
