using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerAttack : MonoBehaviour
{
    // Variable para guardar la posición del mouse
    public Vector2 mousePosition;


    #region Variables de disparo
    // =======================================
    // Variables para poder disparar =========
    public GameObject bullet;
    private int ammo;
    public AudioClip shootSound;
    public AudioClip reloadSound;
    [HideInInspector]
    public Quaternion rotationAngle;
    /// Tipos de munición
    [HideInInspector]
    private int ammoType;
    private int[] ammoInventory;
    private int[] currentAmmoValue;
    
        /// Variables donde se guardan las balas a disparar al inicio del código
    private GameObject[] bulletRepository;
    [HideInInspector]
    public Bullet[] bulletRepositoryScripts;

        /// Variable donde se marcan las balas que se crearán al inicio del código
    public int bulletsToInit;
    private int actualBullet;
    // =======================================
    #endregion

    #region Definitivas y pasivas
    // Habilidades definitivas y pasivas
    [Header("Habilidad definitiva")]
    public GameObject definitiveAttack;
    public bool definitiveCharged;
    public Image definitiveImage;
    public int definitiveCharge;
    private Animator definitiveAnim;
    [Header("Habilidad pasiva")]
    public GameObject passiveAbility;
    #endregion

    // Stats del arma que estás usando, se actualizan al cambiar de arma
    [HideInInspector]
    public int bulletDamage;
    [HideInInspector]
    public int bulletSpeed;
    [HideInInspector]
    public float fireRate;

    // Texto en el que aparece el número de balas
    public Text ammoText;

    /// Referencias a otros scripts del player
    private WeaponManager weaponManager;

    public bool shooting;
    public bool reloading;

    private float timer;
    private float weaponTimer;

    // Animación
    [Header("Animación del ataque")]
    public List<string> animations;

    // Lista de las armas disponibles
    [Header("Armas del personaje")]
    public List<WeaponInfo> weapons;

    private GameManager gameManager;

    // Audio
    private AudioSource audioSource;

    // ============================
    // Modo de ataque
    private int attackMode = 0;
        // Ataque cuepro a cuerpo
    private MeleeAttack meleeAttack;

    private int lastWeaponInserted;
    private bool weaponPicked;

    void Awake()
    {
        bulletRepository = new GameObject[bulletsToInit];
        bulletRepositoryScripts = new Bullet[bulletsToInit];

       // gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();

        ammoInventory = new int[(int)WeaponManager.AmmoTypes.LAST_NO_USE];
        currentAmmoValue = new int[ammoInventory.Length];

        GenerateBullets();
    }

    void Start()
    {
        // Ponemos el número de balas que tenemos en el texto
        ammoText.text = "" + ammo + " | " + ammoInventory[ammoType];

        actualBullet = bulletsToInit;

        audioSource = transform.GetChild(1).GetComponent<AudioSource>();

        definitiveAnim = GameObject.Find("Definitive image").GetComponent<Animator>();

        weaponManager = GetComponent<WeaponManager>();

        meleeAttack = GetComponentInChildren<MeleeAttack>();

        animations.Capacity = 2;

       // definitiveAttack = gameManager.definitive;
       // passiveAbility = gameManager.passive;

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
        rotationAngle = Quaternion.Lerp(transform.rotation, Quaternion.Euler(0, 0, distanceToRotate), 1);

        // Función que calcula cuánto necesitas rotar
        float getAngle(Vector2 position, Vector2 mousePosition)
        {
            float x = mousePosition.x - position.x;
            float y = mousePosition.y - position.y;

            return Mathf.Rad2Deg * Mathf.Atan2(y, x);
        }
        #endregion

        #region Ataque del jugador

        if (Input.GetKey(KeyCode.Mouse0) && !shooting && !reloading)
        {
            shooting = true;

            switch (attackMode)
            {
                case 0:// Puños
                    meleeAttack.Attack();
                    break;


                case 1:// Arma
                    if (ammo <= 0 || (ammo <= 0 && ammoInventory[ammoType] <= 0)) { return; }
                    if (actualBullet <= 0)
                    {
                        actualBullet = bulletsToInit;
                    }


                    audioSource.clip = shootSound;
                    audioSource.Play();


                    ammo--;
                    // Guardar el valor de la munición que tienes
                    currentAmmoValue[ammoType] = ammo;
                    // Cambias el texto al número de balas actual
                    ammoText.text = "" + ammo + " | " + ammoInventory[ammoType];

                    // Vuelves activo el gameObject de la bala para que se active su script
                    bulletRepository[actualBullet - 1].SetActive(true);

                    // Y aquí llamas a la función que le da movimiento a la bala
                    bulletRepositoryScripts[actualBullet - 1].StartMovement();

                    actualBullet--;
                    break;
            }
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
        // Recargar
        if (Input.GetKeyDown(KeyCode.R) && !shooting)
        {
            if (ammoInventory[ammoType] <= 0) { return; }

            reloading = true;
            // Suma a ammo y resta a la munición que tengas, hasta que tengas el cargador lleno o se acabe la munición
            for (int i = 0;
                ammo < weapons[weaponManager.currentWeapon].weaponAmmo && ammoInventory[ammoType] > 0;
                i++)
            {
                ammo++;
                ammoInventory[ammoType]--;
            }
            // Guardar el valor de la munición que tienes
            currentAmmoValue[ammoType] = ammo;

            audioSource.clip = reloadSound;
            audioSource.Play();
            // Actualizar el texto
            ammoText.text = "" + ammo + " | " + ammoInventory[ammoType];
            // Poner que la próxima bala que se dispare sea la última, para evitar problemas
            actualBullet = bulletsToInit;

        }
        if (reloading && !audioSource.isPlaying)
        {
            reloading = false;
        }

        // Cambiar el modo de ataque
        if (Input.GetKeyDown(KeyCode.Q))
        {
            ChangeAttackMode();
        }

        #region Habilidad definitiva
        // Habilidad definitiva
        if (Input.GetKeyDown(KeyCode.Z) && definitiveCharge >= 100)
        {
            Instantiate(definitiveAttack, transform.position, transform.rotation);
            definitiveImage.fillAmount = definitiveCharge * 0.01f;
            definitiveCharge = 0;
            definitiveAnim.SetFloat("DefinitiveCharge", definitiveCharge);
        }
        #endregion
        #endregion

        if (weaponPicked)
        {
            weaponTimer += Time.deltaTime;
            if (weaponTimer >= 0.1f)
            {
                weaponPicked = false;
                weaponTimer = 0;
            }
        }
    }

    void ChangeAttackMode()
    {
        if (attackMode == 0)
        {
            attackMode = 1;
            ChangeWeapon(weaponManager.currentWeapon);
        }
        else
        {
            attackMode = 0;
            ammoText.text = "Punch";
            // Stats del arma
            fireRate = meleeAttack.meleeWeaponInfo.attackRecoil;
            shootSound = meleeAttack.meleeWeaponInfo.attackSound;
            reloadSound = meleeAttack.meleeWeaponInfo.recoverSound;

            animations[0] = meleeAttack.meleeWeaponInfo.animationNames[0];
            animations[1] = meleeAttack.meleeWeaponInfo.animationNames[1];
        }
    }
    public void ChangeWeapon(int weaponID)
    {
        if (weaponID >= weapons.Count || weaponID < 0 || weapons.Count <= 0) { return; }

        GetWeaponStats(weaponID);
    }
    void GetWeaponStats(int weaponID)
    {
        // Tipo de munición
        ammoType = (int)weapons[weaponID].ammoType;
        // Munición de la arma
        ammo = currentAmmoValue[ammoType];
        ammoText.text = "" + ammo + " | " + ammoInventory[ammoType];
        // Velocidad de disparo
        fireRate = weapons[weaponID].fireRecoil;
        // Sonidos
        shootSound = weapons[weaponID].fireSound;
        reloadSound = weapons[weaponID].reloadSound;
        // Animaciones
        animations[0] = weapons[weaponID].animationNames[0];
        animations[1] = weapons[weaponID].animationNames[1];

        for (int i = 0; i < bulletsToInit; i++)
        {
            // Velocidad de la bala y daño del arma
            bulletRepositoryScripts[i].bulletSpeed = weapons[weaponID].bulletSpeed;
            bulletRepositoryScripts[i].bulletDamage = weapons[weaponID].weaponDamage;
        }
    }
    void AddWeapon(string weaponName)
    {
        weaponPicked = true;

        GameObject weapon = Resources.Load("Prefabs/Weapons/" + weaponName) as GameObject;
        WeaponInfo weaponToAdd = weapon.GetComponent<WeaponInfo>();

        weapons.Add(weaponToAdd);

        lastWeaponInserted++;

        weaponManager.weaponsOnInventory++;
    }

    IEnumerator DefinitiveCharge()
    {
        // Carga de la definitiva con el tiempo
        if (definitiveCharge < 100)
            definitiveCharge += 25;
        definitiveAnim.SetFloat("DefinitiveCharge", definitiveCharge);
        yield return new WaitForSeconds(1);
        StartCoroutine(DefinitiveCharge());
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("AmmoBox"))
        {
            int type = (int)other.gameObject.GetComponent<AmmoBox>().ammoType;

            ammoInventory[type] += weaponManager.allWeapons[type].weaponAmmo;
            ammoText.text = "" + ammo + " | " + ammoInventory[type];
        }
        if (other.CompareTag("Weapon") && !weaponPicked)
        {
            AddWeapon(other.name);
            Destroy(other.gameObject);
        }
    }
}