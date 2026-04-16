using System.Linq;
using TMPro;
using UnityEditor.SearchService;
using UnityEngine;
using UnityEngine.SceneManagement;

public class TimerManager : MonoBehaviour
{
    [Header("Time Settings")]
    [SerializeField] public float time;
    [SerializeField] private LevelConfig levelConfig;
    [SerializeField] private TextMeshProUGUI text;

    void Update()
    {
      


            if (levelConfig.enemigos.Count > 0)
            {
                time -= Time.deltaTime;
                text.text = time.ToString("F2"); // mostrar con 2 decimales

                if(time <= 20)
                {
                    text.color = Color.softRed;
                    
                }


                if(time == 0)
                {
                    ChangeScene("Defeat");
                }
            }

        
    }


    public void ChangeScene(string sceneName)
    {

        SceneManager.LoadScene(sceneName);

    }






}
