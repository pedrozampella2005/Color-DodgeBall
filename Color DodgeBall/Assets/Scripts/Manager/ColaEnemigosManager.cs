using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Ya cree la logica de la cola , se los dejo comentado por si acaso 
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

    // ACA ESTA LA COLA
    // guarda los enemigos en orden FIFO
    // el primero que entra es el primero que sale
    private Queue<GameObject> colaEnemigos;

    private Coroutine rutinaSpawn;
    private int indiceSpawnActual = 0;
    private int indiceObjetivoActual = 0;

    void Start()
    {
        // Se crea la cola vacia
        InicializarCola();

        // Se cargan en la cola los enemigos del nivel actual
        CargarNivel(nivelActual);

        // Se empieza a procesar la cola: sacar enemigos e instanciarlos
        rutinaSpawn = StartCoroutine(ProcesarCola());
    }

    public void InicializarCola()
    {
        colaEnemigos = new Queue<GameObject>();
    }

    public void Acolar(GameObject enemigoPrefab)
    {
        if (enemigoPrefab != null)
        {
            colaEnemigos.Enqueue(enemigoPrefab);
        }
    }

    public void Desacolar()
    {
        if (!ColaVacia())
        {
            colaEnemigos.Dequeue();
        }
    }

    public bool ColaVacia()
    {
        return colaEnemigos == null || colaEnemigos.Count == 0;
    }

    public GameObject Primero()
    {
        if (!ColaVacia())
        {
            return colaEnemigos.Peek();
        }

        return null;
    }

    public void CargarNivel(int numeroNivel)
    {
        if (colaEnemigos == null)
        {
            InicializarCola();
        }
        else
        {
            colaEnemigos.Clear();
        }

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

        LevelConfig config = niveles[numeroNivel - 1];

        // ACA SE LLENA LA COLA
        // por cada enemigo configurado en el nivel,
        // se agrega la cantidad indicada
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

        Debug.Log("Nivel cargado: " + config.nombreNivel + " | Enemigos en cola: " + colaEnemigos.Count);
    }

    IEnumerator ProcesarCola()
    {
        // ACA SE USA LA COLA
        // mientras haya enemigos, se toma el primero,
        // se instancia, y despues se lo quita de la cola
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

            // ACA SE CONECTA LA COLA CON EL JUEGO
            // el enemigo que salio de la cola ya aparece en escena
            // y se le asigna su objetivo
            Enemy enemy = enemigoInstanciado.GetComponent<Enemy>();
            if (enemy != null && objetivoElegido != null)
            {
                enemy.SetTarget(objetivoElegido);
            }

            Desacolar();

            yield return new WaitForSeconds(tiempoEntreEnemigos);
        }

        Debug.Log("La cola de enemigos quedó vacía.");
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