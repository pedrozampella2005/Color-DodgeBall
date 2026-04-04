using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class EnemyLevelEntry
{
    public GameObject enemyPrefab;
    public int cantidad = 1;
}

[System.Serializable]
public class LevelConfig
{
    public string nombreNivel = "Nivel";
    public List<EnemyLevelEntry> enemigos = new List<EnemyLevelEntry>();
}

// TDA Cola 
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

public class ColaEnemigosManager : MonoBehaviour
{
    [Header("Puntos de spawn")]
    [SerializeField] private List<Transform> puntosSpawn = new List<Transform>();

    [Header("Puntos objetivo")]
    [SerializeField] private List<Transform> puntosObjetivo = new List<Transform>();

    [Header("Tiempo entre enemigos")]
    [SerializeField] private float tiempoEntreEnemigos = 2f;

    [Header("Nivel actual")]
    [SerializeField] private int nivelActual = 1;

    [Header("Configuracion de niveles")]
    [SerializeField] private List<LevelConfig> niveles = new List<LevelConfig>();

    [Header("Modo de eleccion")]
    [SerializeField] private bool usarSpawnAleatorio = false;
    [SerializeField] private bool usarObjetivoAleatorio = false;

    //  aca el TDA Cola
    private ColaEnemigosTF colaEnemigos;

    private Coroutine rutinaSpawn;
    private int indiceSpawnActual = 0;
    private int indiceObjetivoActual = 0;

    void Start()
    {
        InicializarCola();
        CargarNivel(nivelActual);
        rutinaSpawn = StartCoroutine(ProcesarCola());
    }

    public void InicializarCola()
    {
        colaEnemigos = new ColaEnemigosTF();
        colaEnemigos.InicializarCola(Mathf.Max(1, CalcularCapacidadNivel(nivelActual)));
    }

    public void Acolar(GameObject enemigoPrefab)
    {
        if (colaEnemigos != null && enemigoPrefab != null)
        {
            colaEnemigos.Acolar(enemigoPrefab);
        }
    }

    public void Desacolar()
    {
        if (colaEnemigos != null && !colaEnemigos.ColaVacia())
        {
            colaEnemigos.Desacolar();
        }
    }

    public bool ColaVacia()
    {
        return colaEnemigos == null || colaEnemigos.ColaVacia();
    }

    public GameObject Primero()
    {
        if (!ColaVacia())
        {
            return colaEnemigos.Primero();
        }

        return null;
    }

    public void CargarNivel(int numeroNivel)
    {
        if (niveles == null || niveles.Count == 0)
        {
            Debug.LogWarning("No hay niveles configurados.");
            return;
        }

        if (numeroNivel < 1 || numeroNivel > niveles.Count)
        {
            Debug.LogWarning("El nivel pedido no existe.");
            return;
        }

        // Se reinicializa la cola con la capacidad exacta del nivel
        colaEnemigos = new ColaEnemigosTF();
        colaEnemigos.InicializarCola(Mathf.Max(1, CalcularCapacidadNivel(numeroNivel)));

        LevelConfig config = niveles[numeroNivel - 1];

        // Aca se carga la cola con los enemigos del nivel
        for (int i = 0; i < config.enemigos.Count; i++)
        {
            EnemyLevelEntry entry = config.enemigos[i];

            if (entry.enemyPrefab != null && entry.cantidad > 0)
            {
                for (int j = 0; j < entry.cantidad; j++)
                {
                    Acolar(entry.enemyPrefab);
                }
            }
        }

        Debug.Log("Nivel cargado: " + config.nombreNivel + " | Enemigos en cola: " + colaEnemigos.Cantidad());
    }

    IEnumerator ProcesarCola()
    {
        // Aca se usa la cola:
        // se toma el primero, se instancia, y luego se desacola
        while (!ColaVacia())
        {
            GameObject enemigoPrefab = Primero();

            Transform spawnElegido = ObtenerSpawn();
            Transform objetivoElegido = ObtenerObjetivo();

            if (spawnElegido == null)
            {
                Debug.LogWarning("No hay puntos de spawn cargados.");
                yield break;
            }

            GameObject enemigoInstanciado = Instantiate(enemigoPrefab, spawnElegido.position, Quaternion.identity);

            Enemy enemy = enemigoInstanciado.GetComponent<Enemy>();
            if (enemy != null && objetivoElegido != null)
            {
                enemy.SetTarget(objetivoElegido);
            }

            Desacolar();

            yield return new WaitForSeconds(tiempoEntreEnemigos);
        }

        Debug.Log("La cola de enemigos quedo vacia.");
    }

    private Transform ObtenerSpawn()
    {
        if (puntosSpawn == null || puntosSpawn.Count == 0)
        {
            return null;
        }

        if (usarSpawnAleatorio)
        {
            int indiceRandom = Random.Range(0, puntosSpawn.Count);
            return puntosSpawn[indiceRandom];
        }

        if (indiceSpawnActual >= puntosSpawn.Count)
        {
            indiceSpawnActual = 0;
        }

        Transform spawn = puntosSpawn[indiceSpawnActual];
        indiceSpawnActual++;

        if (indiceSpawnActual >= puntosSpawn.Count)
        {
            indiceSpawnActual = 0;
        }

        return spawn;
    }

    private Transform ObtenerObjetivo()
    {
        if (puntosObjetivo == null || puntosObjetivo.Count == 0)
        {
            return null;
        }

        if (usarObjetivoAleatorio)
        {
            int indiceRandom = Random.Range(0, puntosObjetivo.Count);
            return puntosObjetivo[indiceRandom];
        }

        if (indiceObjetivoActual >= puntosObjetivo.Count)
        {
            indiceObjetivoActual = 0;
        }

        Transform objetivo = puntosObjetivo[indiceObjetivoActual];
        indiceObjetivoActual++;

        if (indiceObjetivoActual >= puntosObjetivo.Count)
        {
            indiceObjetivoActual = 0;
        }

        return objetivo;
    }

    private int CalcularCapacidadNivel(int numeroNivel)
    {
        if (niveles == null || niveles.Count == 0)
        {
            return 1;
        }

        if (numeroNivel < 1 || numeroNivel > niveles.Count)
        {
            return 1;
        }

        int total = 0;
        LevelConfig config = niveles[numeroNivel - 1];

        for (int i = 0; i < config.enemigos.Count; i++)
        {
            if (config.enemigos[i] != null && config.enemigos[i].cantidad > 0)
            {
                total += config.enemigos[i].cantidad;
            }
        }

        return Mathf.Max(1, total);
    }

    public void ReiniciarNivel()
    {
        if (rutinaSpawn != null)
        {
            StopCoroutine(rutinaSpawn);
        }

        indiceSpawnActual = 0;
        indiceObjetivoActual = 0;

        CargarNivel(nivelActual);
        rutinaSpawn = StartCoroutine(ProcesarCola());
    }

    public void CambiarNivel(int nuevoNivel)
    {
        nivelActual = nuevoNivel;
        ReiniciarNivel();
    }
}