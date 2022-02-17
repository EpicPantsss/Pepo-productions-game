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
    public int ammo;

        /// Variables donde se guardan las balas a disparar al inicio del código
    private GameObject[] bulletRepository;
    private Bullet[] bulletRepositoryScripts;

        /// Variable donde se marcan las balas que se crearán al inicio del código
    public int bulletsToInit;
    private int actualBullet;
    // =======================================


    public int bulletDamage;
    public int bulletSpeed;

    // Texto en el que aparece el número de balas
    public Text ammoText;

    /// Referencias a otros scripts del player
    private WeaponManager weaponManager;

    public bool shooting;

    float timer;

    void Awake()
    {
        bulletRepository = new GameObject[bulletsToInit];
        bulletRepositoryScripts = new Bullet[bulletsToInit];
        // La munición es el número total de balas que se pueden disparar
        ammo = bulletsToInit;

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

    void Start()
    {
        // Ponemos el número de balas que tenemos en el texto
        ammoText.text = "" + ammo;

        weaponManager = GetComponent<WeaponManager>();
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
        if (Input.GetKeyDown(KeyCode.E) || Input.GetKeyDown(KeyCode.Mouse0))
        {
            shooting = true;
            if (ammo <= 0) { return; }
            actualBullet = ammo;

            ammo--;
            // Cambias el texto al número de balas actual
            ammoText.text = "" + ammo;

            // Vuelves activo el gameObject de la bala para que se active su script
            bulletRepository[actualBullet - 1].SetActive(true);

            // Y aquí llamas a la función que le da movimiento a la bala
            bulletRepositoryScripts[actualBullet - 1].StartMovement();
        }
        if (shooting)
        {
            timer += Time.deltaTime;

            if (timer > 1.5f)
            {
                timer = 0;
                shooting = false;
            }
        }
        if (Input.GetKeyDown(KeyCode.R))
        {
            ammo = bulletsToInit;

            ammoText.text = "" + ammo;
        }
        #endregion

    }

    public void ChangeWeapon(int weaponID)
    {
        ammo = weaponManager.weapons[weaponID].weaponAmmo;
        for (int i = 0; i < bulletsToInit; i++)
        {
            bulletRepositoryScripts[i].bulletSpeed = weaponManager.weapons[weaponID].bulletSpeed;
        }
    }
}
