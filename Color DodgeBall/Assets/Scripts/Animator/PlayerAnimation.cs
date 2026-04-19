using UnityEngine;

public class PlayerAnimation : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private Animator animator;
    [SerializeField] private Rigidbody2D rb;

    [Header("Idle Animations")]
    [SerializeField] private string idleDown = "Idle Player";
    [SerializeField] private string idleLeft = "Idle Left Player";
    [SerializeField] private string idleRight = "Idle Right Player";
    [SerializeField] private string idleUp = "Idle Up Player";

    [Header("Move Animations")]
    [SerializeField] private string moveDown = "Move Player";
    [SerializeField] private string moveLeft = "Move Left Player";
    [SerializeField] private string moveRight = "Move Right Player";
    [SerializeField] private string moveUp = "Move Up Player";

    [Header("Settings")]
    [SerializeField] private float movementThreshold = 0.05f;

    private enum Direction
    {
        Down,
        Left,
        Right,
        Up
    }

    private Direction lastDirection = Direction.Down;
    private string currentAnimation;

    void Reset()
    {
        animator = GetComponent<Animator>();
        rb = GetComponent<Rigidbody2D>();
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
    }

    void Start()
    {
        PlayAnimation(idleDown);
    }

    void Update()
    {
        if (animator == null || rb == null)
        {
            return;
        }

        Vector2 velocity = rb.linearVelocity;
        bool isMoving = velocity.sqrMagnitude > movementThreshold * movementThreshold;

        if (isMoving)
        {
            // Si el movimiento horizontal es mayor, usa Left o Right
            if (Mathf.Abs(velocity.x) > Mathf.Abs(velocity.y))
            {
                if (velocity.x < 0f)
                {
                    lastDirection = Direction.Left;
                    PlayAnimation(moveLeft);
                }
                else
                {
                    lastDirection = Direction.Right;
                    PlayAnimation(moveRight);
                }
            }
            // Si el movimiento vertical es mayor, usa Up o Down
            else
            {
                if (velocity.y > 0f)
                {
                    lastDirection = Direction.Up;
                    PlayAnimation(moveUp);
                }
                else
                {
                    lastDirection = Direction.Down;
                    PlayAnimation(moveDown);
                }
            }
        }
        else
        {
            // Cuando deja de moverse, vuelve al idle de la última dirección
            switch (lastDirection)
            {
                case Direction.Left:
                    PlayAnimation(idleLeft);
                    break;

                case Direction.Right:
                    PlayAnimation(idleRight);
                    break;

                case Direction.Up:
                    PlayAnimation(idleUp);
                    break;

                default:
                    PlayAnimation(idleDown);
                    break;
            }
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
