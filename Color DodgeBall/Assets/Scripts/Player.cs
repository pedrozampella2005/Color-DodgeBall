using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class Player : MonoBehaviour
{
    [SerializeField] private float speed = 5f;
    [SerializeField] private PlayerInput playerInput;
    [SerializeField] private GameObject blueBallPrefab;
    [SerializeField] private GameObject redBallPrefab;
    [SerializeField] private GameObject orangeBallPrefab;
    [SerializeField] private BoxCollider2D boundary;



    [SerializeField] private Transform stackPanel;
    [SerializeField] private GameObject ballIcon;

    [SerializeField] private UIAnimator animatorUI;
    private BallStack ballStack;
    private Rigidbody2D rb;
    private Vector2 movementInput;
    private Camera cam;
    private bool isOnCulldown = false;
    private bool touchingFloor = false;


    void Start()
    {
        animatorUI = FindAnyObjectByType<UIAnimator>();
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
        rb.linearVelocity = movementInput.normalized * speed;

        Vector3 pos = transform.position;
        Bounds bounds = boundary.bounds;

        pos.x = Mathf.Clamp(pos.x, bounds.min.x, bounds.max.x);
        pos.y = Mathf.Clamp(pos.y, bounds.min.y, bounds.max.y);

        transform.position = pos;
    }


    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Floor"))
        {
            touchingFloor = true;
            Debug.Log("Colision con : " + collision.gameObject.name);   
        }
    }


  






    public void PushBlueBall(InputAction.CallbackContext ctx)
    {
        if (ctx.performed)
        {
            
            Debug.Log("Hola");
            ballStack.Push(blueBallPrefab);
            AddIconToUI(Color.blue);
        }
    }

    public void PushOrangeBall(InputAction.CallbackContext ctx)
    {
        if (ctx.performed)
        {
            Debug.Log("Hola 2");
            ballStack.Push(orangeBallPrefab);
            AddIconToUI(Color.orange);
        }
    }
    public void PushRedBall(InputAction.CallbackContext ctx)
    {
        if (ctx.performed)
        {
            Debug.Log("Hola3");
            ballStack.Push(redBallPrefab);
            AddIconToUI(Color.red);
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
                    RemoveIconFromUI();
                }

                Rigidbody2D ballRb = ballInstance.GetComponent<Rigidbody2D>();

                if(ballRb != null)
                {
                    Vector2 direction = movementInput.normalized;
                    if (direction == Vector2.zero)
                    {
                        direction = transform.right;
                    }
                    ballRb.AddForce(direction * ballComponent.speed, ForceMode2D.Impulse);



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



    private void AddIconToUI(Color color)
    {
        GameObject icon = Instantiate(ballIcon, stackPanel);
        icon.GetComponent<Image>().color = color;


        StartCoroutine(animatorUI.Appear(icon, 0.4f));
    }


    private void RemoveIconFromUI()
    {
        if(stackPanel.childCount > 0)
        {
            GameObject last = stackPanel.GetChild(stackPanel.childCount - 1).gameObject;
            StartCoroutine(animatorUI.Disappear(last, 0.4f));
        }

    }
}