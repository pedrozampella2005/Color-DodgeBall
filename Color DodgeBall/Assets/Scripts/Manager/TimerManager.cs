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

    [Header("Victory Condition")]
    [SerializeField] private string victoryLevelName = "Level2";

    private int currentKills = 0;
    private bool gameFinished = false;

    void Start()
    {
        GameFlowData.lastLevelSceneName = SceneManager.GetActiveScene().name;
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

        if (timerText != null)
        {
            timerText.text = time.ToString("F2");
        }

        if (killsText != null)
        {
            killsText.text = currentKills + " / " + requiredKills;
        }

        if (time <= 20f && timerText != null)
        {
            timerText.color = Color.red;
        }

        
        // Si el jugador llega a la cantidad de kills requeridas
        // y está en el nivel configurado para victoria, gana automáticamente
        if (currentKills >= requiredKills && SceneManager.GetActiveScene().name == victoryLevelName)
        {
            gameFinished = true;
            ChangeScene(victorySceneName);
            return;
        }

        
        if (time <= 0f)
        {
            gameFinished = true;

            if (currentKills < requiredKills)
            {
                ChangeScene(defeatSceneName);
            }
        }
    }

    public void RegisterKill()
    {
        if (gameFinished)
        {
            return;
        }

        currentKills++;
    }

    public void ChangeScene(string sceneName)
    {
        SceneManager.LoadScene(sceneName);
    }
}