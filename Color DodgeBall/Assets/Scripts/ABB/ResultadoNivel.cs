using System;

[Serializable]
public class ResultadoNivel
{
    public string nombreJugador;
    public int nivel;
    public string nombreEscena;
    public float tiempoCompletado;
    public float tiempoRestante;
    public int enemigosEliminados;
    public int puntaje;

    public ResultadoNivel(
        string nombreJugador,
        int nivel,
        string nombreEscena,
        float tiempoCompletado,
        float tiempoRestante,
        int enemigosEliminados,
        int puntaje)
    {
        this.nombreJugador = nombreJugador;
        this.nivel = nivel;
        this.nombreEscena = nombreEscena;
        this.tiempoCompletado = tiempoCompletado;
        this.tiempoRestante = tiempoRestante;
        this.enemigosEliminados = enemigosEliminados;
        this.puntaje = puntaje;
    }
}