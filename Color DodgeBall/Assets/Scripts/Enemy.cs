using UnityEngine;

//no hay coliccion contra las pelotas de colores
public class Enemy : MonoBehaviour
{
    [SerializeField] private float speed = 3f;

    private Rigidbody2D rb;
    private Transform targetPoint;
    private bool reachedTarget = false;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
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

    public void SetTarget(Transform newTarget)
    {
        targetPoint = newTarget;
    }
}
