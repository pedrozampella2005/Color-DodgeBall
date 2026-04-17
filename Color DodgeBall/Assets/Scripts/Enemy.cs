//Te dejo cambios

using UnityEngine;
public enum EnemyColor
{
    Blue,
    Orange,
    Red
}

public class Enemy : MonoBehaviour
{
    [SerializeField] private float speed = 3f;
    [SerializeField] private EnemyColor color;

    private Rigidbody2D rb;
    private Transform targetPoint;
    private bool reachedTarget = false;

    // Se cambio:
    //  referencia al TimerManager para avisarle cuando el jugador mata
    // un enemigo correcto
    private TimerManager timerManager;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();

        // Se cambio:
        // busca el timer una sola vez al iniciar
        timerManager = FindAnyObjectByType<TimerManager>();
    }

    void FixedUpdate()
    {
        if (targetPoint == null || reachedTarget)
        {
            if (rb != null)
            {
                rb.linearVelocity = Vector2.zero;
            }
            return;
        }

        Vector2 direction = ((Vector2)targetPoint.position - rb.position).normalized;
        rb.linearVelocity = direction * speed;

        float distance = Vector2.Distance(rb.position, targetPoint.position);

        if (distance < 0.15f)
        {
            reachedTarget = true;
            rb.linearVelocity = Vector2.zero;
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        // se cambio:
        // cuando la pelota coincide con el color del enemigo:
        // se avisa al TimerManager que sume una baja
        // se destrulle el enemigo
        if (collision.gameObject.CompareTag("Blue") && color == EnemyColor.Blue)
        {
            if (timerManager != null)
            {
                timerManager.RegisterKill();
            }

            Destroy(gameObject);
            Debug.Log("Destruido azul");
        }

        if (collision.gameObject.CompareTag("Orange") && color == EnemyColor.Orange)
        {
            if (timerManager != null)
            {
                timerManager.RegisterKill();
            }

            Destroy(gameObject);
            Debug.Log("Destruido naranja");
        }

        if (collision.gameObject.CompareTag("Red") && color == EnemyColor.Red)
        {
            if (timerManager != null)
            {
                timerManager.RegisterKill();
            }

            Destroy(gameObject);
            Debug.Log("Destruido rojo");
        }

        if (collision.gameObject.CompareTag("DeadPoint"))
        {
            Destroy(gameObject);
        }
    }

    public void SetTarget(Transform newTarget)
    {
        targetPoint = newTarget;
    }
}