using UnityEngine;

using System.Collections.Generic;
using UnityEngine;

public static class GameFlowData
{
    public static string lastLevelSceneName = "";

    public static string nombreJugadorActual = "Jugador";

    public static List<ResultadoNivel> resultadosSesion;
    public static ArbolTiemposABB arbolTiempos;

    private static float tiempoPartidaActual = 0f;
    private static float tiempoRestantePartidaActual = 0f;
    private static int enemigosEliminadosPartidaActual = 0;
    private static int puntajePartidaActual = 0;

    static GameFlowData()
    {
        InicializarRanking();
        ResetSession();
    }

    private static void InicializarRanking()
    {
        if (resultadosSesion == null)
        {
            resultadosSesion = new List<ResultadoNivel>();
        }

        if (arbolTiempos == null)
        {
            arbolTiempos = new ArbolTiemposABB();
            arbolTiempos.InicializarArbol();
        }
    }

    public static void SetNombreJugador(string nombre)
    {
        if (string.IsNullOrWhiteSpace(nombre))
        {
            nombreJugadorActual = "Jugador";
        }
        else
        {
            nombreJugadorActual = nombre;
        }
    }

    public static void ResetSession()
    {
        InicializarRanking();

        lastLevelSceneName = "";

        tiempoPartidaActual = 0f;
        tiempoRestantePartidaActual = 0f;
        enemigosEliminadosPartidaActual = 0;
        puntajePartidaActual = 0;
    }

    public static void RegistrarNivelCompletado(float tiempoUsado, float tiempoRestante, int enemigosEliminados)
    {
        tiempoPartidaActual += tiempoUsado;
        tiempoRestantePartidaActual += tiempoRestante;
        enemigosEliminadosPartidaActual += enemigosEliminados;

        int puntajeNivel = enemigosEliminados * 100 + Mathf.RoundToInt(tiempoRestante);
        puntajePartidaActual += puntajeNivel;
    }

    public static void RegistrarResultadoFinal(int nivel, string nombreEscena)
    {
        ResultadoNivel resultado = new ResultadoNivel(
            nombreJugadorActual,
            nivel,
            nombreEscena,
            tiempoPartidaActual,
            tiempoRestantePartidaActual,
            enemigosEliminadosPartidaActual,
            puntajePartidaActual
        );

        resultadosSesion.Add(resultado);
        arbolTiempos.Agregar(resultado);
    }

    public static void BorrarRanking()
    {
        resultadosSesion = new List<ResultadoNivel>();

        arbolTiempos = new ArbolTiemposABB();
        arbolTiempos.InicializarArbol();

        ResetSession();
    }
}





//[System.Serializable]
//public class ResultadoNivel
//{
//    public int nivel;
//    public string nombreEscena;
//    public float tiempoCompletado;
//    public int enemigosEliminados;

//    public ResultadoNivel(int nivel, string nombreEscena, float tiempoCompletado, int enemigosEliminados)
//    {
//        this.nivel = nivel;
//        this.nombreEscena = nombreEscena;
//        this.tiempoCompletado = tiempoCompletado;
//        this.enemigosEliminados = enemigosEliminados;
//    }
//}