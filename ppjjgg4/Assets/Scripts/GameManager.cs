using com.cyborgAssets.inspectorButtonPro;
using System;
using TMPro;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static event Action OnDayEnded;
    public static GameManager Instance;

    [SerializeField] private TextMeshProUGUI tmp;
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
        InitGame();
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

        tmp.text = $"Day {day} - Score: {newScore}"; //TODO: temporary placement for day display
        Debug.Log($"Day {day} - Score: {newScore}");



    }
}
