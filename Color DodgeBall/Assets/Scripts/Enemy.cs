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
    private TimerManager timerManager;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
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
        if (collision.gameObject.CompareTag("Blue") && color == EnemyColor.Blue)
        {
            RegistrarKillYDestruir();
        }

        if (collision.gameObject.CompareTag("Orange") && color == EnemyColor.Orange)
        {
            RegistrarKillYDestruir();
        }

        if (collision.gameObject.CompareTag("Red") && color == EnemyColor.Red)
        {
            RegistrarKillYDestruir();
        }

        if (collision.gameObject.CompareTag("DeadPoint"))
        {
            Destroy(gameObject);
        }
    }

    private void RegistrarKillYDestruir()
    {
        if (timerManager != null)
        {
            timerManager.RegisterKill();
        }

        Destroy(gameObject);
    }

    public void SetTarget(Transform newTarget)
    {
        targetPoint = newTarget;
    }
}