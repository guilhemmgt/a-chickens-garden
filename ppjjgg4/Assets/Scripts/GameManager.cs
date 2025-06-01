using com.cyborgAssets.inspectorButtonPro;
using DG.Tweening;
using System;
using TMPro;
using UnityEngine;

public enum GameState
{
    Menu,
    Planting,
    Digging,
    Herbier,
    Shop,
}

public class GameManager : MonoBehaviour
{
    public static event Action OnDayEnded;
    public static event Action<int> OnScoreUpdate;
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
    public void EndDay() {
		ChoiceHandler.Instance.SetCurrentPlant (null);
		Chicken.Instance.FadeIn ().OnComplete (() => {
            day++;
            OnDayEnded?.Invoke ();
            UpdateScore ();
			Chicken.Instance.FadeOut ();
			if (Shovel.Instance.IsDigging ())
				Shovel.Instance.UseShovel ();
		});
	}

    public void UpdateScore()
    {
        int newScore = Garden.Instance.GetScore();

        TopBarView.Instance.SetScore(newScore);
        TopBarView.Instance.SetDay(day);
        this.score = newScore;
        OnScoreUpdate?.Invoke(newScore);
        //TopBarView.Instance.SetEventSprite (...); // TODO : ajout �vents
    }
}
