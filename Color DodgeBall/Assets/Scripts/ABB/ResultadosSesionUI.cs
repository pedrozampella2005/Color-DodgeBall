using System.Collections.Generic;
using TMPro;
using UnityEngine;

using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ResultadosSesionUI : MonoBehaviour
{
    [Header("UI")]
    [SerializeField] private TextMeshProUGUI abbText;
    [SerializeField] private TextMeshProUGUI quickSortText;

    [Header("Cantidad a mostrar")]
    [SerializeField] private int cantidadMaxima = 5;

    void Start()
    {
        MostrarMejoresTiempos();
        MostrarRanking();
    }

    private void MostrarMejoresTiempos()
    {
        if (abbText == null)
        {
            return;
        }

        List<ResultadoNivel> mejoresTiempos = new List<ResultadoNivel>();
        GameFlowData.arbolTiempos.RecorridoInOrder(mejoresTiempos);

        if (mejoresTiempos.Count == 0)
        {
            abbText.text = "MEJORES TIEMPOS\nSin resultados.";
            return;
        }

        string texto = "MEJORES TIEMPOS\n";

        int limite = Mathf.Min(cantidadMaxima, mejoresTiempos.Count);

        for (int i = 0; i < limite; i++)
        {
            ResultadoNivel resultado = mejoresTiempos[i];

            texto += (i + 1) + ") " +
                     resultado.nombreJugador +
                     " | " +
                     resultado.tiempoCompletado.ToString("F2") +
                     "s | " +
                     resultado.puntaje +
                     " pts\n";
        }

        abbText.text = texto;
    }

    private void MostrarRanking()
    {
        if (quickSortText == null)
        {
            return;
        }

        if (GameFlowData.resultadosSesion.Count == 0)
        {
            quickSortText.text = "RANKING\nSin resultados.";
            return;
        }

        ResultadoNivel[] ranking = GameFlowData.resultadosSesion.ToArray();
        QuickSortResultados.OrdenarPorPuntajeDesc(ranking);

        string texto = "RANKING\n";

        int limite = Mathf.Min(cantidadMaxima, ranking.Length);

        for (int i = 0; i < limite; i++)
        {
            ResultadoNivel resultado = ranking[i];

            texto += (i + 1) + ") " +
                     resultado.nombreJugador +
                     " | " +
                     resultado.puntaje +
                     " pts | " +
                     resultado.enemigosEliminados +
                     " kills\n";
        }

        quickSortText.text = texto;
    }
}