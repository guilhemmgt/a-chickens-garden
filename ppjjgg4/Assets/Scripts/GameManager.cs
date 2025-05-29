using System;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static event Action OnDayEnded;
    public static GameManager Instance;
    private int day;
    private int money;
    private int score;

    private void Awake()
    {
        if (Instance != null)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
        InitGame();
    }

    public void InitGame()
    {
        day = 0;
        money = 0;
        score = 0;
    }

    public void EndDay()
    {
        day++;
        OnDayEnded?.Invoke();
    }

    public void UpdateScore()
    {
        
    }
}
