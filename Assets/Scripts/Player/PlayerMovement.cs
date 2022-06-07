using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerMovement : MonoBehaviour
{
    public enum Direction { NONE, UP, DOWN, LEFT, RIGHT };
    public Direction direction;

    public float speed;

    public Animator anim;
    public List<AnimationClip> animationClips;

    public bool walking;
    [HideInInspector]
    public Vector2 playerFront;

    private PlayerAttack playerAttack;

    [Header("Sigilo/Agacharse")]
    public float sneakSpeed;
    public bool agachado;
    public Image sneakImage;

    private float aux;
    private float aux2;

    [Header("Opciones de la camara")]
    public Camera mainCamera;
    private float normalCameraSize;
    public float sneakCameraSize;
    public float transitionSpeed;
    [Header("Sonido")]
    private AudioSource audioSource;
    public AudioClip playerWalkSound;
    private bool soundStarted;

    private Rigidbody2D rb;

    private KeyCode horizontalKeyPressed;
    private KeyCode verticalKeyPressed;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        anim = gameObject.GetComponentInChildren<Animator>();
        playerAttack = GetComponent<PlayerAttack>();

        audioSource = GetComponent<AudioSource>();

        aux = speed;

        normalCameraSize = mainCamera.orthographicSize;
        aux2 = normalCameraSize;
    }

    void Update()
    {
        playerFront = Vector2.right;

        // Control de inputs
        if (Input.GetKey(KeyCode.D))
        {
            if (Input.GetKey(KeyCode.D)) { transform.rotation = Quaternion.Euler(0, 0, transform.rotation.z + 90); };
            rb.velocity = new Vector2(speed, rb.velocity.y);
            direction = Direction.RIGHT;
            
            horizontalKeyPressed = KeyCode.D;
        }
        if (Input.GetKey(KeyCode.A))
        {
            if (Input.GetKey(KeyCode.A)) {transform.rotation = Quaternion.Euler(0, 0, transform.rotation.z - 90); };
            rb.velocity = new Vector2(-speed, rb.velocity.y);
            direction = Direction.LEFT;
            horizontalKeyPressed = KeyCode.A;
        }
        if (Input.GetKey(KeyCode.W))
        {
            rb.velocity = new Vector2(rb.velocity.x, speed);
            if (Input.GetKey(KeyCode.W)) { transform.rotation = Quaternion.Euler(0, 0, transform.rotation.z - 180); };
            direction = Direction.UP;
            verticalKeyPressed = KeyCode.W;
        }
        if (Input.GetKey(KeyCode.S))
        {
            if (Input.GetKey(KeyCode.S)) { transform.rotation = Quaternion.Euler(0, 0, 0); };
            rb.velocity = new Vector2(rb.velocity.x, -speed);
            direction = Direction.DOWN;
            verticalKeyPressed = KeyCode.S;
        }
        // Comprobar si se ha dejado de pulsar la última tecla pulsada de cada eje
        if (Input.GetKeyUp(horizontalKeyPressed))
        {
            rb.velocity = new Vector2(0, rb.velocity.y);
        }
        if (Input.GetKeyUp(verticalKeyPressed))
        {
            rb.velocity = new Vector2(rb.velocity.x, 0);
        }

        if (walking && !Input.GetKeyDown(KeyCode.E) && !Input.GetKeyDown(KeyCode.Mouse0))
        {
            if (!soundStarted)
            {
                audioSource.clip = playerWalkSound;
                audioSource.volume = 0.4f;
                audioSource.Play();
                soundStarted = true;
            }
        }
        else
        {
            audioSource.Stop();
            soundStarted = false;
        }

        // Agacharse
        if (Input.GetKeyDown(KeyCode.LeftControl))
        {
            agachado = !agachado;
            sneakImage.gameObject.SetActive(agachado);

        }
        if (agachado)
        {
            speed = sneakSpeed;
            if (aux2 < sneakCameraSize)
            {
                mainCamera.orthographicSize = aux2;
                aux2 += Time.smoothDeltaTime * transitionSpeed;
            }
        }
        else
        {
            speed = aux;
            if (aux2 > normalCameraSize)
            {
                mainCamera.orthographicSize = aux2;
                aux2 -= Time.smoothDeltaTime * transitionSpeed;
            }
        }
    }
}