using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class GamePlayController : MonoBehaviour
{
   
    [SerializeField] private TextMeshProUGUI ScoreText;

    private void OnEnable()
    {
        GameManager.instance.onScoreChanged += OnScoreChanged;
    }

    private void OnDisable()
    {
        GameManager.instance.onScoreChanged -= OnScoreChanged;
    }

    private void OnScoreChanged(int score)
    {
        ScoreText.text = "Score: " + score;
    }

}
