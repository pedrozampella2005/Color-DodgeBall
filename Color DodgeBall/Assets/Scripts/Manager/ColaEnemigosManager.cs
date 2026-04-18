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

    private ColaEnemigosTF colaEnemigos;
    private Coroutine rutinaSpawn;
    private int indiceSpawnActual = 0;
    private int indiceObjetivoActual = 0;

    void Start()
    {
        CargarNivel(nivelActual);
        rutinaSpawn = StartCoroutine(ProcesarCola());
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

        colaEnemigos = new ColaEnemigosTF();
        colaEnemigos.InicializarCola(Mathf.Max(1, CalcularCapacidadNivel(numeroNivel)));

        LevelConfig config = niveles[numeroNivel - 1];

        for (int i = 0; i < config.enemigos.Count; i++)
        {
            EnemyLevelEntry entry = config.enemigos[i];

            if (entry.enemyPrefab != null && entry.cantidad > 0)
            {
                for (int j = 0; j < entry.cantidad; j++)
                {
                    colaEnemigos.Acolar(entry.enemyPrefab);
                }
            }
        }

        Debug.Log("Nivel cargado: " + config.nombreNivel + " | Enemigos en cola: " + colaEnemigos.Cantidad());
    }

    IEnumerator ProcesarCola()
    {
        while (colaEnemigos != null && !colaEnemigos.ColaVacia())
        {
            GameObject enemigoPrefab = colaEnemigos.Primero();

            

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

            colaEnemigos.Desacolar();

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