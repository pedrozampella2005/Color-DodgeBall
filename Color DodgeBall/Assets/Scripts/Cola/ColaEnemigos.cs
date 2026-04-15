using UnityEngine;

public interface ColaEnemigosTDA
{
    void InicializarCola(int capacidad);
    void Acolar(GameObject x);
    void Desacolar();
    bool ColaVacia();
    GameObject Primero();
    int Cantidad();
}

public class ColaEnemigosTF : ColaEnemigosTDA
{
    private GameObject[] a;
    private int indice;

    public void InicializarCola(int capacidad)
    {
        if (capacidad < 1)
        {
            capacidad = 1;
        }

        a = new GameObject[capacidad];
        indice = 0;
    }

    public void Acolar(GameObject x)
    {
        if (indice >= a.Length)
        {
            Debug.LogWarning("La cola de enemigos esta llena.");
            return;
        }

        for (int i = indice - 1; i >= 0; i--)
        {
            a[i + 1] = a[i];
        }

        a[0] = x;
        indice++;
    }

    public void Desacolar()
    {
        if (!ColaVacia())
        {
            a[indice - 1] = null;
            indice--;
        }
    }

    public bool ColaVacia()
    {
        return indice == 0;
    }

    public GameObject Primero()
    {
        if (!ColaVacia())
        {
            return a[indice - 1];
        }

        return null;
    }

    public int Cantidad()
    {
        return indice;
    }
}
