using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;

public class HealthBar : MonoBehaviour
{
    public Image fillBar;

    [SerializeField] private PlayerHealth playerHealth;
   

    private void OnEnable()
    {
        playerHealth.OnHpChanged += OnHpChanged;
    }

    private void OnDisable()    
    {
        playerHealth.OnHpChanged += OnHpChanged;
    }
    private void OnHpChanged(float currentHp, float startHp)
    {
        fillBar.fillAmount = (float)currentHp / (float)startHp;
    }

    
}
