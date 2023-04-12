using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    
    private static PlayerMovement m_Instance;
    public static PlayerMovement instance
    {
        get
        {
            if (m_Instance == null)
                m_Instance = FindObjectOfType<PlayerMovement>();
            return m_Instance;
        }
    } 

    [SerializeField] private Rigidbody2D rb;
    [SerializeField] private float m_moveSpeed;
    [SerializeField] private float rollBoost = 2f;
    [SerializeField] private float m_RollTime;
    [SerializeField] private float m_FiringCoolDown;
    [SerializeField] private SpriteRenderer spriteRenderer;
    [SerializeField] private Camera cam;
    [SerializeField] private GameObject bulletPrefab;
    [SerializeField] private float bullletForce = 20f;
    [SerializeField] private Weapon weapon;
    [SerializeField] private Bultet bullet;
    public Animator m_anim;

    private float rollTime;
    private float m_TempCoolDown;
    private bool rollOnce = false;
    

    private GameObject firePoint;
    private Vector2 movement;
    private Vector3 mousePos;


    private void Awake()
    {
        if (m_Instance == null)
            m_Instance = this;
        else if (m_Instance != this)
            Destroy(gameObject);
    }

    private void Start()
    {
        m_FiringCoolDown = 0.5f;
        bullletForce = 10f;
        bullet.bulletDamage = 1f;
    }


    // Update is called once per frame
    void Update()
    {
        // Di chuyen
        Movement();

        // Dodge roll
        DodgeRoll();

        // Flip
        Flip();

        //Shoot
        if (Input.GetButton("Fire1"))
        {
            if (m_TempCoolDown <= 0)
            {
                Shoot();               
                m_TempCoolDown = m_FiringCoolDown;
            }
        }
        m_TempCoolDown -= Time.deltaTime;
     
    }

    private void Movement()
    {
        movement.x = Input.GetAxis("Horizontal");
        movement.y = Input.GetAxis("Vertical");

        rb.MovePosition(rb.position + movement * m_moveSpeed * Time.fixedDeltaTime);
        //transform.position += movement * m_moveSpeed * Time.fixedDeltaTime;
        m_anim.SetFloat("Speed", movement.sqrMagnitude);
    }
    private void DodgeRoll()
    {
        if (Input.GetKeyDown(KeyCode.Space) && rollTime <= 0)
        {
            m_anim.SetBool("Roll", true);
            m_moveSpeed += rollBoost;
            rollTime = m_RollTime;
            rollOnce = true;
        }

        if (rollTime <= 0 && rollOnce == true)
        {
            m_anim.SetBool("Roll", false);
            m_moveSpeed -= rollBoost;
            rollOnce = false;
        }
        else
        {
            rollTime -= Time.deltaTime;
        }
    }

    private void Flip()
    {
        if (movement.x != 0)
        {
            if (movement.x > 0)
                spriteRenderer.transform.localScale = new Vector3(1, 1, 1);
            else spriteRenderer.transform.localScale = new Vector3(-1, 1, 1);
        }
    }

    public void Shoot()
    {
        //CurrentGun

        // Spawn Bullet
        firePoint = GameObject.Find("FirePoint");
        mousePos = cam.ScreenToWorldPoint(Input.mousePosition);
        Vector2 lookDir = mousePos - transform.position;
        float angle = Mathf.Atan2(lookDir.y, lookDir.x) * Mathf.Rad2Deg;
        Quaternion rotation = Quaternion.Euler(0, 0, angle);

        // Single Bullet
        if (weapon.currentWeaponIndex != 2)
        {
            Bultet bullet = SpawnManager.Instance.SpawnBullet(firePoint.transform.position, transform);
            SetGunCurrent(bullet);
        Rigidbody2D rb  = bullet.GetComponent<Rigidbody2D>();
        rb.AddForce(firePoint.transform.right * bullletForce, ForceMode2D.Impulse);
        bullet.Fire();
        }
        // ShotGunMultiBullet
        else
        {
            GameObject firePoint1 = GameObject.Find("FirePoint1");
            GameObject firePoint2 = GameObject.Find("FirePoint2");

            Bultet bullet1 = SpawnManager.Instance.SpawnBullet(firePoint.transform.position, transform);
            Rigidbody2D rb1 = bullet1.GetComponent<Rigidbody2D>();
            SetGunCurrent(bullet1);
            rb1.AddForce(firePoint.transform.right * bullletForce, ForceMode2D.Impulse);
            bullet1.Fire();


            Bultet bullet2 = SpawnManager.Instance.SpawnBullet(firePoint1.transform.position, transform);
            Rigidbody2D rb2 = bullet2.GetComponent<Rigidbody2D>();
            SetGunCurrent(bullet2);
            rb2.AddForce(firePoint1.transform.right * bullletForce, ForceMode2D.Impulse);
            bullet2.Fire();

            Bultet bullet3 = SpawnManager.Instance.SpawnBullet(firePoint2.transform.position, transform);
            Rigidbody2D rb3 = bullet3.GetComponent<Rigidbody2D>();
            SetGunCurrent(bullet3);
            rb3.AddForce(firePoint2.transform.right * bullletForce, ForceMode2D.Impulse);
            bullet3.Fire();
        }

        //Spawn muzzle
        SpawnManager.Instance.SpawnMuzzle(firePoint.transform.position, transform, rotation);
      
    }

    public void SetGunCurrent(Bultet _bullet)
    {
        if (weapon.currentWeaponIndex == 0)
        {
            m_FiringCoolDown = 0.5f;
            bullletForce = 10f;
            _bullet.bulletDamage = 1f;
        }
        else
        if (weapon.currentWeaponIndex == 1)
        {
            m_FiringCoolDown = 0.2f;
            bullletForce = 15f;
            _bullet.bulletDamage = 2f;
        }
        else
        if (weapon.currentWeaponIndex == 2)
        {
            m_FiringCoolDown = 1f;
            bullletForce = 20f;
            _bullet.bulletDamage = 2f;
        }
    }
    

}
