using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class TimerManager : MonoBehaviour
{
    [Header("Time Settings")]
    [SerializeField] private float time = 60f;
    [SerializeField] private int requiredKills = 5;

    [Header("UI")]
    [SerializeField] private TextMeshProUGUI timerText;
    [SerializeField] private TextMeshProUGUI killsText;

    [Header("Scenes")]
    [SerializeField] private string defeatSceneName = "Defeat";
    [SerializeField] private string victorySceneName = "Victory";

    [Header("Victory Flow")]
    [SerializeField] private string finalLevelName = "Level 2";

    private float initialTime;
    private int currentKills = 0;
    private bool gameFinished = false;

    void Start()
    {
        initialTime = time;
        currentKills = 0;
        gameFinished = false;

        GameFlowData.lastLevelSceneName = SceneManager.GetActiveScene().name;

        ActualizarUI();
    }

    void Update()
    {
        if (gameFinished)
        {
            return;
        }

        time -= Time.deltaTime;

        if (time < 0f)
        {
            time = 0f;
        }

        ActualizarUI();

        if (time <= 0f)
        {
            gameFinished = true;
            SceneManager.LoadScene(defeatSceneName);
        }
    }

    public void RegisterKill()
    {
        if (gameFinished)
        {
            return;
        }

        currentKills++;
        ActualizarUI();

        if (currentKills >= requiredKills)
        {
            CompletarNivel();
        }
    }

    private void CompletarNivel()
    {
        gameFinished = true;

        string nombreEscenaActual = SceneManager.GetActiveScene().name;
        int numeroNivel = ObtenerNumeroNivel(nombreEscenaActual);

        float tiempoUsado = initialTime - time;
        float tiempoRestante = time;

        GameFlowData.RegistrarNivelCompletado(
            tiempoUsado,
            tiempoRestante,
            currentKills
        );

        if (nombreEscenaActual == finalLevelName)
        {
            GameFlowData.RegistrarResultadoFinal(numeroNivel, nombreEscenaActual);
            SceneManager.LoadScene(victorySceneName);
        }
        else
        {
            LoadNextLevel();
        }
    }

    private void LoadNextLevel()
    {
        int currentIndex = SceneManager.GetActiveScene().buildIndex;
        int nextIndex = currentIndex + 1;

        if (nextIndex < SceneManager.sceneCountInBuildSettings)
        {
            SceneManager.LoadScene(nextIndex);
        }
        else
        {
            GameFlowData.RegistrarResultadoFinal(
                ObtenerNumeroNivel(SceneManager.GetActiveScene().name),
                SceneManager.GetActiveScene().name
            );

            SceneManager.LoadScene(victorySceneName);
        }
    }

    private int ObtenerNumeroNivel(string nombreEscena)
    {
        string numero = "";

        for (int i = 0; i < nombreEscena.Length; i++)
        {
            if (char.IsDigit(nombreEscena[i]))
            {
                numero += nombreEscena[i];
            }
        }

        if (numero.Length > 0)
        {
            return int.Parse(numero);
        }

        return -1;
    }

    private void ActualizarUI()
    {
        if (timerText != null)
        {
            timerText.text = time.ToString("F2");

            if (time <= 20f)
            {
                timerText.color = Color.red;
            }
            else
            {
                timerText.color = Color.white;
            }
        }

        if (killsText != null)
        {
            killsText.text = currentKills + " / " + requiredKills;
        }
    }
}






//public class TimerManager : MonoBehaviour
//{
//    [Header("Time Settings")]
//    [SerializeField] private float time = 60f;
//    [SerializeField] private int requiredKills = 5;

//    [Header("UI")]
//    [SerializeField] private TextMeshProUGUI timerText;
//    [SerializeField] private TextMeshProUGUI killsText;

//    [Header("Scenes")]
//    [SerializeField] private string defeatSceneName = "Defeat";
//    [SerializeField] private string victorySceneName = "Victory";

//    [Header("Victory Condition")]
//    [SerializeField] private string victoryLevelName = "Level2";

//    private int currentKills = 0;
//    private bool gameFinished = false;

//    void Start()
//    {
//        GameFlowData.lastLevelSceneName = SceneManager.GetActiveScene().name;
//    }

//    void Update()
//    {
//        if (gameFinished)
//        {
//            return;
//        }

//        time -= Time.deltaTime;

//        if (time < 0f)
//        {
//            time = 0f;
//        }

//        if (timerText != null)
//        {
//            timerText.text = time.ToString("F2");
//        }

//        if (killsText != null)
//        {
//            killsText.text = currentKills + " / " + requiredKills;
//        }

//        if (time <= 20f && timerText != null)
//        {
//            timerText.color = Color.red;
//        }


//        // Si el jugador llega a la cantidad de kills requeridas
//        // y está en el nivel configurado para victoria, gana automáticamente
//        if (currentKills >= requiredKills && SceneManager.GetActiveScene().name == victoryLevelName)
//        {
//            gameFinished = true;
//            ChangeScene(victorySceneName);
//            return;
//        }


//        if (time <= 0f)
//        {
//            gameFinished = true;

//            if (currentKills < requiredKills)
//            {
//                ChangeScene(defeatSceneName);
//            }
//        }
//    }

//    public void RegisterKill()
//    {
//        if (gameFinished)
//        {
//            return;
//        }

//        currentKills++;
//    }

//    public void ChangeScene(string sceneName)
//    {
//        SceneManager.LoadScene(sceneName);
//    }
//}