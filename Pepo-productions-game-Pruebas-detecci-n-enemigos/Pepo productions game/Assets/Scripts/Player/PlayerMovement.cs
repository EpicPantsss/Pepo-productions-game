using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerMovement : MonoBehaviour
{
    enum Direction { NONE, UP, DOWN, LEFT, RIGHT };
    Direction direction;

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

    void Start()
    {
        anim = gameObject.GetComponentInChildren<Animator>();
        playerAttack = GetComponent<PlayerAttack>();

        audioSource = GetComponent<AudioSource>();

        aux = speed;

        normalCameraSize = mainCamera.orthographicSize;
        aux2 = normalCameraSize;
    }

    void Update()
    {
        walking = playerAttack.shooting;

        playerFront = Vector2.right;

        if (Input.GetKey(KeyCode.D))
        {
            transform.position = new Vector2(transform.position.x + speed * Time.deltaTime, transform.position.y);
            direction = Direction.RIGHT;
            walking = true;
        }
        if (Input.GetKey(KeyCode.A))
        {
            transform.position = new Vector2(transform.position.x - speed * Time.deltaTime, transform.position.y);
            direction = Direction.LEFT;
            walking = true;
        }
        if (Input.GetKey(KeyCode.W))
        {
            transform.position = new Vector2(transform.position.x, transform.position.y + speed * Time.deltaTime);
            direction = Direction.UP;
            walking = true;
        }
        if (Input.GetKey(KeyCode.S))
        {
            transform.position = new Vector2(transform.position.x, transform.position.y - speed * Time.deltaTime);
            direction = Direction.DOWN;
            walking = true;
        }
        if ((Input.GetKeyUp(KeyCode.W)
            && Input.GetKeyUp(KeyCode.A)
            && Input.GetKeyUp(KeyCode.D)
            && Input.GetKeyUp(KeyCode.S)))
        {
            walking = false;
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
            direction = Direction.NONE;
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
