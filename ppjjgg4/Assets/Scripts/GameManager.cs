using com.cyborgAssets.inspectorButtonPro;
using System;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static event Action OnDayEnded;
    public static GameManager Instance;

    private int day;
    private int score;

    private void Awake()
    {
        if (Instance != null)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
        
    }

    private void Start()
    {
        InitGame();
    }

    public void InitGame()
    {
        day = 1;
        score = 0;
        UpdateScore();
    }

    [ProButton]
    public void EndDay()
    {
        day++;
        OnDayEnded?.Invoke();
        UpdateScore();
    }

    public void UpdateScore()
    {
        int newScore = Garden.Instance.GetScore();

        TopBarView.Instance.SetScore(newScore);
        TopBarView.Instance.SetDay (day);
        //TopBarView.Instance.SetEventSprite (...); // TODO : ajout �vents
    }
}
