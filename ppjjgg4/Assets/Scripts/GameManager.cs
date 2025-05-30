using com.cyborgAssets.inspectorButtonPro;
using System;
using TMPro;
using UnityEngine;

public enum GameState
{
    Menu,
    Planting,
    Shop,
}

public class GameManager : MonoBehaviour
{
    public static event Action OnDayEnded;
    public static GameManager Instance;

    public static GameState GameState { get; set; } = GameState.Menu;

    [SerializeField] private TextMeshProUGUI tmp;
    private int day;
    public int score;

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
        UpdateScore();
    }

    public void InitGame()
    {
        day = 1;
        score = 0;
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
        TopBarView.Instance.SetDay(day);
        this.score = newScore;
        //TopBarView.Instance.SetEventSprite (...); // TODO : ajout �vents
    }
}
