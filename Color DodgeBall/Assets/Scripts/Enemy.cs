using NUnit.Framework;
using UnityEngine;

public enum EnemyColor { Blue, Orange, Red};
public class Enemy : MonoBehaviour
{
    [SerializeField] private float speed = 3f;

  
    private Rigidbody2D rb;
    private Transform targetPoint;
    [SerializeField] private EnemyColor color;
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


    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.CompareTag("Blue") && color == EnemyColor.Blue)
        {
            Destroy(gameObject);
            Debug.Log("Destruido azul");
        }
        if(collision.gameObject.CompareTag("Orange") && color == EnemyColor.Orange)
        {
            Destroy(gameObject);
            Debug.Log("Destruido naranja");
        }
        if(collision.gameObject.CompareTag("Red") && color == EnemyColor.Red)
        {
            Destroy(gameObject);
            Debug.Log("Destruido rojo");
        }


        if(collision.gameObject.CompareTag("DeadPoint"))
        {
            Destroy(gameObject);
        }
    }
    public void SetTarget(Transform newTarget)
    {
        targetPoint = newTarget;
    }
}
