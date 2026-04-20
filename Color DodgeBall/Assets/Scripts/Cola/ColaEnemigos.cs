using UnityEngine;
//La interfaz
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
    private GameObject[] a; // es el arreglo donde se gusrda los enemigos
    private int indice;

    public void InicializarCola(int capacidad)
    {
        if (capacidad < 1)
        {
            capacidad = 1;
        }

        a = new GameObject[capacidad];//se crea una cola para cierta cantidad de enemigos y arranca con 0
        indice = 0;
    }

    //Aca entra los enemigos 
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

        a[0] = x;//Entra el enemigo
        indice++;//cantidad
    }

    //sale enemigo de la cola
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

    //Sabe cual va a salir primero
    public GameObject Primero()
    {
        if (!ColaVacia())
        {
            return a[indice - 1];//mueve el mas viejo elemento a la derecha para q salga el primero 
        }

        return null;
    }

    public int Cantidad()
    {
        return indice;
    }
}