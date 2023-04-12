using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyController : MonoBehaviour
{
    [SerializeField] private float Speed;
    [SerializeField] private float lineOfSite;
    [SerializeField] private Animator m_anim;
    [SerializeField] private bool isDrawZimos;
    [SerializeField] private float enemyDamage = 1;

    public float startingHealth;
    public float currentHealth;

    private void Start()
    {
        currentHealth = startingHealth;
        isDrawZimos = false;
    }

    void Update()
    {
        // follow player
        followPlayer();

        //Flip
        Flip();
    }

    private void followPlayer()
    {
        float distanceFromPlayer = Vector2.Distance(PlayerMovement.instance.transform.position, transform.position);
        if (distanceFromPlayer < lineOfSite)
        {
            transform.position = Vector2.MoveTowards(this.transform.position, PlayerMovement.instance.transform.position, Speed * Time.deltaTime);
        }
    }

    private void Flip()
    {
        if (transform.position.x > PlayerMovement.instance.transform.position.x)
        {
            transform.localScale = new Vector3(-0.16f, 0.16f, 0.16f);
        }
        else
            transform.localScale = new Vector3(0.16f, 0.16f, 0.16f);
    }
    public void TakeDame(float _damage)
    {
        currentHealth = Mathf.Clamp(currentHealth - _damage, 0, startingHealth);

        if (currentHealth > 0)
        {
            // enemy hurt
            m_anim.SetTrigger("hurt");
            Debug.Log("enemy was hit");
        }
        else if (currentHealth == 0)
        {
            // enemy died
            m_anim.SetTrigger("die");
            SpawnManager.Instance.ReleaseEnemy(this);
            GameManager.instance.AddScore(1);
        }
    }
    private void OnDrawGizmosSelected()
    {
        if (isDrawZimos)
        {
            Gizmos.color = Color.green;
            Gizmos.DrawWireSphere(transform.position, lineOfSite);
        }
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Player")
        {
            PlayerHealth playerHealth;
            collision.gameObject.TryGetComponent(out playerHealth);
            playerHealth.TakeDame(enemyDamage);
        }
    }
}
