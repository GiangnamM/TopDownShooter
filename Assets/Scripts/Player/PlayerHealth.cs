using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using System;

public class PlayerHealth : MonoBehaviour
{
    [SerializeField] private float startingHealth;
    private float currentHealth;
      

    public UnityEvent OnDeath;

    public Action<float, float> OnHpChanged;

    void Start()
    {
        currentHealth = startingHealth;

        if (OnHpChanged != null)
        {
            OnHpChanged(currentHealth, startingHealth);
        }
    }
    private void Update()
    {
        if (GameManager.instance.isNewgame == true)
        {
            if (OnHpChanged != null)
            {
                OnHpChanged(startingHealth, startingHealth);
            }
            GameManager.instance.isNewgame = false;
        }
    }

    public void TakeDame(float _damage)
    {
        currentHealth = Mathf.Clamp(currentHealth - _damage, 0, startingHealth);

        if (OnHpChanged != null)
        {
            OnHpChanged(currentHealth, startingHealth);
        }

        if (currentHealth > 0)
        {
            // player hurt 
        }
        else
        {
            currentHealth = 0;
            // player died => Run Events
            OnDeath.Invoke();
        }
        
    }

    public void Death()
    {
        PlayerMovement.instance.m_anim.SetTrigger("Die");
    }
}
