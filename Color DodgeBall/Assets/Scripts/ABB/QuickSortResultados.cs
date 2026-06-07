public static class QuickSortResultados
{
    public static void OrdenarPorPuntajeDesc(ResultadoNivel[] arreglo)
    {
        if (arreglo == null || arreglo.Length <= 1)
        {
            return;
        }

        QuickSort(arreglo, 0, arreglo.Length - 1);
    }

    private static void QuickSort(ResultadoNivel[] arreglo, int izquierda, int derecha)
    {
        if (izquierda < derecha)
        {
            int pivote = Particionar(arreglo, izquierda, derecha);

            QuickSort(arreglo, izquierda, pivote - 1);
            QuickSort(arreglo, pivote + 1, derecha);
        }
    }

    private static int Particionar(ResultadoNivel[] arreglo, int izquierda, int derecha)
    {
        ResultadoNivel pivote = arreglo[derecha];
        int i = izquierda - 1;

        for (int j = izquierda; j < derecha; j++)
        {
            if (EsMejorResultado(arreglo[j], pivote))
            {
                i++;
                Intercambiar(arreglo, i, j);
            }
        }

        Intercambiar(arreglo, i + 1, derecha);
        return i + 1;
    }

    private static bool EsMejorResultado(ResultadoNivel actual, ResultadoNivel pivote)
    {
        if (actual.puntaje > pivote.puntaje)
        {
            return true;
        }

        if (actual.puntaje == pivote.puntaje && actual.tiempoCompletado < pivote.tiempoCompletado)
        {
            return true;
        }

        return false;
    }

    private static void Intercambiar(ResultadoNivel[] arreglo, int i, int j)
    {
        ResultadoNivel aux = arreglo[i];
        arreglo[i] = arreglo[j];
        arreglo[j] = aux;
    }
}