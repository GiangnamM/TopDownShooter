 using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bultet : MonoBehaviour
{
    [SerializeField] private GameObject hitEffect;
    [SerializeField] private float TimeDestroyObj;
    public float bulletDamage;
    private Rigidbody2D rb;
    private float m_time;
    public float bullletForce = 20f;
    
    private void Start()
    {
        m_time = TimeDestroyObj;
        rb = GetComponent<Rigidbody2D>();        
    }
    private void Update()
    {
        
        m_time -= Time.deltaTime;
        if (m_time <= 0)
        {
            Release();        
        }
    }

    public void Fire()
    {
        m_time = TimeDestroyObj;
    }

    public void Release()
    {
        SpawnManager.Instance.ReleaseBullet(this);
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        //if (collision.tag != "Player")
        //{
        //    //SpawnManager.Instance.ReleaseBullet(this);
        //}
        if (collision.gameObject.CompareTag("enemy"))
        {
            SpawnManager.Instance.ReleaseBullet(this);

            GameObject effect = Instantiate(hitEffect, transform.position, Quaternion.identity);
            Destroy(effect, 0.1f);

            EnemyController enemyController; 
            collision.gameObject.TryGetComponent(out enemyController);          
            enemyController.TakeDame(bulletDamage);
        }
    }
}
