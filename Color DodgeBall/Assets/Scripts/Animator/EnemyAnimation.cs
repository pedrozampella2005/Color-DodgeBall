using UnityEngine;

public class EnemyAnimation : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private Animator animator;
    [SerializeField] private Rigidbody2D rb;
    [SerializeField] private SpriteRenderer spriteRenderer;

    [Header("Move Animations")]
    [SerializeField] private string moveDown = "Move Enemy";
    [SerializeField] private string moveRight = "Move Right Enemy";
    [SerializeField] private string moveUp = "Move Up Enemy";

    [Header("Reuse Right Animation For Left")]
    [SerializeField] private bool useRightAnimationForLeft = true;

    [Header("Settings")]
    [SerializeField] private float movementThreshold = 0.05f;

    private string currentAnimation;

    void Reset()
    {
        animator = GetComponent<Animator>();
        rb = GetComponent<Rigidbody2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    void Awake()
    {
        if (animator == null)
        {
            animator = GetComponent<Animator>();
        }

        if (rb == null)
        {
            rb = GetComponent<Rigidbody2D>();
        }

        if (spriteRenderer == null)
        {
            spriteRenderer = GetComponent<SpriteRenderer>();
        }
    }

    void Update()
    {
        if (animator == null || rb == null)
        {
            return;
        }

        Vector2 velocity = rb.linearVelocity;

        // Como el enemigo no tiene idle, si no se mueve no cambiamos animación
        if (velocity.sqrMagnitude <= movementThreshold * movementThreshold)
        {
            return;
        }

        // Si se mueve más en X que en Y, usamos izquierda/derecha
        if (Mathf.Abs(velocity.x) > Mathf.Abs(velocity.y))
        {
            if (velocity.x < 0f)
            {
                // IZQUIERDA:
                // reutiliza la animación de derecha y espeja el sprite
                if (useRightAnimationForLeft)
                {
                    SetFlip(true);
                    PlayAnimation(moveRight);
                }
                else
                {
                    SetFlip(false);
                    PlayAnimation("Move Left Enemy");
                }
            }
            else
            {
                // DERECHA
                SetFlip(false);
                PlayAnimation(moveRight);
            }
        }
        else
        {
            // Para arriba y abajo dejamos el sprite normal
            SetFlip(false);

            if (velocity.y > 0f)
            {
                PlayAnimation(moveUp);
            }
            else
            {
                PlayAnimation(moveDown);
            }
        }
    }

    private void SetFlip(bool value)
    {
        if (spriteRenderer != null)
        {
            spriteRenderer.flipX = value;
        }
    }

    private void PlayAnimation(string animationName)
    {
        if (string.IsNullOrEmpty(animationName))
        {
            return;
        }

        if (currentAnimation == animationName)
        {
            return;
        }

        animator.Play(animationName);
        currentAnimation = animationName;
    }
}