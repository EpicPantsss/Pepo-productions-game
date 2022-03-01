using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerAttack : MonoBehaviour
{
    // Variable para guardar la posición del mouse
    public Vector2 mousePosition;


    // =======================================
    // Variables para poder disparar =========
    public GameObject bullet;
    private int ammo;
    public AudioClip shootSound;
    public AudioClip reloadSound;
    
        /// Variables donde se guardan las balas a disparar al inicio del código
    private GameObject[] bulletRepository;
    private Bullet[] bulletRepositoryScripts;

        /// Variable donde se marcan las balas que se crearán al inicio del código
    public int bulletsToInit;
    private int actualBullet;
    private int aux;
    // =======================================

    [Header("Habilidad definitiva")]
    public GameObject definitiveAttack;
    public bool definitiveCharged;
    public Image definitiveImage;
    public int definitiveCharge;
    private Animator definitiveAnim;
    [Header("Habilidad pasiva")]
    public GameObject passiveAbility;

    [HideInInspector]
    public int bulletDamage = 1;
    [HideInInspector]
    public int bulletSpeed;
    [HideInInspector]
    public float fireRate;

    // Texto en el que aparece el número de balas
    public Text ammoText;

    /// Referencias a otros scripts del player
    private WeaponManager weaponManager;
    private PlayerMovement playerMovement;

    public bool shooting;
    public bool reloading;

    private float timer;

    // Animación
    [Header("Animación del ataque")]
    public List<string> animations;

    // Lista de las armas disponibles
    [Header("Armas del personaje")]
    public List<WeaponInfo> weapons;

    private GameManager gameManager;

    // Audio
    private AudioSource audioSource;

    void Awake()
    {
        bulletRepository = new GameObject[bulletsToInit];
        bulletRepositoryScripts = new Bullet[bulletsToInit];

        gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();

        GenerateBullets();
    }

    void Start()
    {
        // Ponemos el número de balas que tenemos en el texto
        ammoText.text = "" + ammo;

        actualBullet = bulletsToInit;

        audioSource = transform.GetChild(1).GetComponent<AudioSource>();

        playerMovement = GetComponent<PlayerMovement>();

        definitiveAnim = GameObject.Find("Definitive image").GetComponent<Animator>();

        weaponManager = GetComponent<WeaponManager>();
        weaponManager.weaponsOnInventory = weapons.Capacity - 1;
        animations.Capacity = 2;
        ChangeWeapon(0);

        definitiveAttack = gameManager.definitive;
        passiveAbility = gameManager.passive;

        StartCoroutine(DefinitiveCharge());
    }

    private void GenerateBullets()
    {
        for (int i = 0; i < bulletsToInit; i++)
        {
            /// En esta parte creas el objeto de la bala, y lo haces hijo del jugador. También se desactiva para que no se vea 
            bulletRepository[i] = Instantiate(bullet, transform.position, Quaternion.identity);
            bulletRepository[i].transform.SetParent(this.gameObject.transform);
            bulletRepository[i].SetActive(false);

            /// Aquí guardas el script de la bala que acabas de crear, para después no tener que hacer un GetComponent al disparar (y así ahorrar recursos)
            bulletRepositoryScripts[i] = bulletRepository[i].GetComponent<Bullet>();
        }
    }

    void Update()
    {
        #region Apuntado del jugador al ratón
        // Aquí guardas la posición del mouse en el mapa
        mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);

        // Aquí calculas cuánto hay que rotar para que el objeto mire al mouse
        float distanceToRotate = getAngle(transform.position, mousePosition);
        // Aplicas la rotación
        transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.Euler(0, 0, distanceToRotate), 1);

        // Función que calcula cuánto necesitas rotar
        float getAngle(Vector2 position, Vector2 mousePosition)
        {
            float x = mousePosition.x - position.x;
            float y = mousePosition.y - position.y;

            return Mathf.Rad2Deg * Mathf.Atan2(y, x);
        }
        #endregion

        #region Disparo del jugador
        if ((Input.GetKey(KeyCode.E) || Input.GetKey(KeyCode.Mouse0)) && !shooting && !reloading)
        {
            shooting = true;

            if (ammo <= 0) { return; }
            if (actualBullet <= 0)
            {
                actualBullet = bulletsToInit;
            }


            audioSource.clip = shootSound;
            audioSource.Play();


            ammo--;
            // Cambias el texto al número de balas actual
            ammoText.text = "" + ammo;

            // Vuelves activo el gameObject de la bala para que se active su script
            bulletRepository[actualBullet - 1].SetActive(true);

            // Y aquí llamas a la función que le da movimiento a la bala
            bulletRepositoryScripts[actualBullet - 1].StartMovement();

            actualBullet--;
        }
        if (shooting)
        {
            timer += Time.deltaTime;

            if (timer >= fireRate)
            {
                timer = 0;
                shooting = false;
            }
        }
        if (Input.GetKeyDown(KeyCode.R) && !shooting)
        {
            reloading = true;
            audioSource.clip = reloadSound;
            audioSource.Play();
            ammo = weapons[weaponManager.currentWeapon].weaponAmmo;
            actualBullet = bulletsToInit;

            ammoText.text = "" + ammo;
        }
        if (reloading && !audioSource.isPlaying)
        {
            reloading = false;
        }
        #region Habilidad definitiva
        // Habilidad definitiva
        if (Input.GetKeyDown(KeyCode.Q) && definitiveCharge >= 100)
        {
            Instantiate(definitiveAttack, transform.position, transform.rotation);
            definitiveImage.fillAmount = definitiveCharge * 0.01f;
            definitiveCharge = 0;
            definitiveAnim.SetFloat("DefinitiveCharge", definitiveCharge);
        }
        #endregion
        #endregion
    }

    public void ChangeWeapon(int weaponID)
    {
        if (weaponID > weapons.Capacity || weaponID < 0) { return; }

        ammo = weapons[weaponID].weaponAmmo;
        ammoText.text = "" + ammo;

        fireRate = weapons[weaponID].fireRecoil;
        shootSound = weapons[weaponID].fireSound;
        reloadSound = weapons[weaponID].reloadSound;

        animations[0] = weapons[weaponID].animationNames[0];
        animations[1] = weapons[weaponID].animationNames[1];

        for (int i = 0; i < bulletsToInit; i++)
        {
            bulletRepositoryScripts[i].bulletSpeed = weapons[weaponID].bulletSpeed;
            bulletRepositoryScripts[i].bulletDamage = weapons[weaponID].weaponDamage;
        }
    }

    IEnumerator DefinitiveCharge()
    {
        if (definitiveCharge < 100)
            definitiveCharge += 25;
        definitiveAnim.SetFloat("DefinitiveCharge", definitiveCharge);
        yield return new WaitForSeconds(1);
        StartCoroutine(DefinitiveCharge());
    }
}
