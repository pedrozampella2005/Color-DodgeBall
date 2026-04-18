using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class TimerManager : MonoBehaviour
{
    [Header("Time Settings")]
    [SerializeField] private float time = 60f;
    [SerializeField] private int requiredKills;
    [SerializeField] private TextMeshProUGUI timerText;
    [SerializeField] private TextMeshProUGUI killsText;

    
    // ahora se  cuenta enemigos correctos eliminó el jugador
    private int currentKills = 0;

    // CAMBIO
    // esta bandera evita que la lógica de derrota/victoria se ejecute más de una vez
    private bool gameFinished = false;

    void Update()
    {
        if (gameFinished)
        {
            return;
        }

        time -= Time.deltaTime;

        // CAMBIO
        // se evita que el tiempo siga bajando a negativos
        if (time < 0f)
        {
            time = 0f;
        }

        if (timerText != null)
        {
            timerText.text = time.ToString("F2");
        }

        // CAMBIO
        // se muestra progreso de bajas en UI
        //if (killsText != null)
        //{
        //    killsText.text = currentKills + " / " + requiredKills;
        //}

        if (time <= 20f && timerText != null)
        {
            timerText.color = Color.red;
        }

        // CAMBIO RECONTRA IMPORTANTE 
        // antes se comparaba  con == 0 y podía fallar por ser float
        // ahora se usa <= 0f
        if (time <= 0f)
        {
            gameFinished = true;

            // CAMBIO:
            // la derrota ahora depende de si no llegaste a la cantidad de bajas pedida
            if (currentKills < requiredKills)
            {
                ChangeScene("Defeat");
            }
           
        }


        if(time > 0f)
        {
            if(currentKills == requiredKills) 
            {
                ChangeScene("Level 2");
            };
        }
    }

    // CAMBIO
    // este metodo lo llama el enemigo cuando muere correctamente
    public void RegisterKill()
    {
        if (gameFinished)
        {
            return;
        }

        currentKills++;
        Debug.Log("currentKills:" + currentKills);
        if (killsText != null)
        {
            killsText.text = currentKills + " / " + requiredKills;
        }

    }

    public void ChangeScene(string sceneName)
    {
        SceneManager.LoadScene(sceneName);
    }
}