using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;


public class TimerManager : MonoBehaviour
{
    [Header("Time Settings")]
    [SerializeField] private float time = 60f;
    [SerializeField] private int requiredKills = 5;
    [SerializeField] private TextMeshProUGUI timerText;
    [SerializeField] private TextMeshProUGUI killsText;

    private int currentKills = 0;
    private bool gameFinished = false;

    void Start()
    {
        // Guarda el nombre del nivel actual al empezar
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

        if (time <= 0f)
        {
            gameFinished = true;

            if (currentKills < requiredKills)
            {
                ChangeScene("Defeat");
            }
            else
            {
                Debug.Log("El jugador cumplio la cantidad de bajas requerida.");
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