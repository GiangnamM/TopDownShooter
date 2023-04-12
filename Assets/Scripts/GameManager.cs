using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using TMPro;
public enum GameState
{
    Home,
    Gameplay,
    Pause,
    Gameover
}
public class GameManager : MonoBehaviour
{
    private static GameManager m_Instance;
    public static GameManager instance
    {
        get
        {
            if (m_Instance == null)
                m_Instance = FindObjectOfType<GameManager>();
            return m_Instance;
        }
    }

    public  Action<int> onScoreChanged;

    
    [SerializeField] private GameObject m_HomePanel;
    [SerializeField] private GameObject m_GameplayPanel;
    [SerializeField] private GameObject m_PausePanel;
    [SerializeField] private GameObject m_GameoverPanel;
    [SerializeField] private GameObject m_UpgradePanel;
   
    [SerializeField] private int m_scoretoUpgrade;
    [SerializeField] private int indexCurrentGun;
    public bool isNewgame;

    private GameState m_GameState;

    private int m_Score;

    private void Awake()
    {
        if (m_Instance == null)
            m_Instance = this;
        else if (m_Instance != this)
            Destroy(gameObject);
    }

    private void Start()
    {
        m_HomePanel.gameObject.SetActive(false);
        m_GameplayPanel.gameObject.SetActive(false);
        m_PausePanel.gameObject.SetActive(false);
        m_GameoverPanel.gameObject.SetActive(false);
        SetState(GameState.Home);
        m_Score = 0;
        if (onScoreChanged != null)
            onScoreChanged(m_Score);
        isNewgame = false;
        indexCurrentGun = 0;
    }

    private void SetState(GameState state)
    {
        m_GameState = state;
        m_HomePanel.gameObject.SetActive(m_GameState == GameState.Home);
        m_GameplayPanel.gameObject.SetActive(m_GameState == GameState.Gameplay);
        m_PausePanel.gameObject.SetActive(m_GameState == GameState.Pause);
        m_GameoverPanel.gameObject.SetActive(m_GameState == GameState.Gameover);

        if (m_GameState == GameState.Pause)
            Time.timeScale = 0;
        else Time.timeScale = 1;
    }

    public void NewGame()
    {
        SpawnManager.Instance.StartBattle();
        SetState(GameState.Gameplay);
        isNewgame = true;
        m_Score = 0;
        if (onScoreChanged != null)
            onScoreChanged(m_Score);
        indexCurrentGun = 0;
        Weapon weapon = FindObjectOfType<Weapon>();
        weapon.UpdateGun(indexCurrentGun);

    }

    public void PauseGame()
    {
        SetState(GameState.Pause);
    }

    public void ResumeGame()
    {
        SetState(GameState.Gameplay);
    }

    public void Home()
    {        
        SetState(GameState.Home);       
        SpawnManager.Instance.isActive = false;
        SpawnManager.Instance.Clear();
    }
    public void Gameover(bool win)
    {        
        SetState(GameState.Gameover);      
    }

    public void TurnOffUpgradePanel()
    {
        m_UpgradePanel.SetActive(false);
        Time.timeScale = 1;
    }

    public void TurnOnUpgradePanel()
    {
        m_UpgradePanel.SetActive(true);
        Time.timeScale = 0;
    }

    public void UpgradeGun()
    {
        if (m_Score >= m_scoretoUpgrade)
        {
            indexCurrentGun++;
            Weapon weapon = FindObjectOfType<Weapon>();
            weapon.UpdateGun(indexCurrentGun);
            if (weapon.isChangeGun == true)
            {
                m_Score -= m_scoretoUpgrade;
                if (onScoreChanged != null)
                    onScoreChanged(m_Score);
            }
        }
    }

    public void AddScore(int value)
    {
        m_Score += value;
        if (onScoreChanged != null)
        {
            onScoreChanged(m_Score);
        }
    }

    

}
