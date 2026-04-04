using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

public class Player : MonoBehaviour
{
    [SerializeField] private float speed = 5f;
    [SerializeField] private PlayerInput playerInput;
    [SerializeField] private GameObject blueBallPrefab;
    [SerializeField] private GameObject redBallPrefab;
    [SerializeField] private GameObject orangeBallPrefab;
   private BallStack ballStack;
    private Rigidbody2D rb;
    private Vector2 movementInput;
    private Camera cam;
    private bool isOnCulldown = false;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        playerInput = GetComponent<PlayerInput>();
        cam = Camera.main;
        ballStack = new BallStack();
        ballStack.Init(3);
    }

    void Update()
    {
        movementInput = playerInput.actions["Move"].ReadValue<Vector2>();


    }

    void FixedUpdate()
    {

        if (movementInput != Vector2.zero)
        {

            rb.linearVelocity = movementInput * speed;

        }

        else
        {
            rb.linearVelocity = Vector2.zero;
        }

    }


    public void PushBlueBall(InputAction.CallbackContext ctx)
    {
        if (ctx.performed)
        {
            
            Debug.Log("Hola");
            ballStack.Push(blueBallPrefab);
        }
    }

    public void PushOrangeBall(InputAction.CallbackContext ctx)
    {
        if (ctx.performed)
        {
            Debug.Log("Hola 2");
            ballStack.Push(orangeBallPrefab);
        }
    }
    public void PushRedBall(InputAction.CallbackContext ctx)
    {
        if (ctx.performed)
        {
            Debug.Log("Hola3");
            ballStack.Push(redBallPrefab);
        }
    }

   public void PopBall(InputAction.CallbackContext ctx)
    {

        if (ctx.performed && !isOnCulldown)
        {
            GameObject ball = ballStack.Pop();
            if (ball != null)
            {
                GameObject ballInstance = Instantiate(ball, transform.position, Quaternion.identity);
                

                Debug.Log("Pelota lanzada: " + ball.name);

                Ball ballComponent = ballInstance.GetComponent<Ball>();
                

                

                if (ballComponent != null)
                {
                    StartCoroutine(CullDown(ballComponent.culldownTime));
                    Debug.Log("Pelota agarrada de color: " + ballComponent.colorName);
                    Debug.Log("Culldown iniciado para la pelota: " + ball.name + "Tiempo de CullDown:  " + ballComponent.culldownTime);
                }

                Rigidbody2D ballRb = ballInstance.GetComponent<Rigidbody2D>();

                if(ballRb != null)
                {
                    ballRb.AddForce(Vector2.right * ballComponent.speed, ForceMode2D.Impulse);
                    
                }
            }
        }


    }


    private IEnumerator CullDown(float time)
    {
        isOnCulldown = true;
        yield return new WaitForSeconds(time);
        isOnCulldown = false;
    }
}