using UnityEngine;
using DG.Tweening;
using com.cyborgAssets.inspectorButtonPro;
using System;

public class UI_Controller : MonoBehaviour
{

    public Action OnHerbierShow = new Action(() => { });
    public static UI_Controller Instance;

    [Header("UI Elements")]
    [SerializeField] private RectTransform menuTransform;
    [SerializeField] private RectTransform topBarTransform;
	[SerializeField] private RectTransform shopTransform;
	[SerializeField] private RectTransform toShopSignTransform;
	[SerializeField] private RectTransform herbariumTransform;
	[SerializeField] private RectTransform tutorialTransform;
	[SerializeField] private RectTransform creditsTransform;

	[Header("Anim settings")]
    [SerializeField] public float moveDuration = 0.5f;
    [SerializeField] public Ease moveEase = Ease.Linear;
	[SerializeField] public float overshoot = 0.25f;

    private bool herbariumShown = false;

	private Vector3 startPosition;

    private Vector3 leftPos = new Vector3 (-290, 0, 90);
    private Vector3 rightPos = new Vector3 (290, 0, 90);
    private Vector3 topPos = new Vector3 (0, 160, 90);
    private Vector3 bottomPos = new Vector3 (0, -160, 90);

	private void Awake () {
        Instance = this;
	}

	void Start()
    {
        startPosition = menuTransform.position;

        ShowMenu (true);
	}

    public Tween Move (RectTransform targetTransform, Vector3 targetPosition, bool instantSpeed = false)
    {
        if (instantSpeed) {
            targetTransform.position = targetPosition;
            return DOTween.Sequence();
        } else {
            return targetTransform.DOMove (targetPosition, moveDuration).SetEase (moveEase, overshoot);
        }
    }

    [ProButton]
	public void ShowMenu (bool instantSpeed = false) {
		ShowMenuTween (instantSpeed);
	}
	public Tween ShowMenuTween (bool instantSpeed = false)
    {
		if (Shovel.Instance.IsDigging ())
			Shovel.Instance.UseShovel ();
		GameManager.GameState = GameState.Menu;
        Move(menuTransform, startPosition, instantSpeed);
        Move(shopTransform, rightPos, instantSpeed);
        Move(topBarTransform, topPos, instantSpeed);
        Move (herbariumTransform, topPos, instantSpeed);
        Move (tutorialTransform, topPos, instantSpeed);
		Move (creditsTransform, topPos, instantSpeed);
		herbariumShown = false;

		return Move(toShopSignTransform, bottomPos, instantSpeed);
    }

    [ProButton]
    public void ShowGame() {
		ShowGameTween ();
    }
    public Tween ShowGameTween()
    {
		if (Shovel.Instance.IsDigging ())
			Shovel.Instance.UseShovel ();
		Tween sample = Move(menuTransform, bottomPos);
        Move(shopTransform, rightPos);
        Move(topBarTransform, startPosition);
        Move(toShopSignTransform, startPosition);
		Move (herbariumTransform, topPos);
		herbariumShown = false;

		GameManager.GameState = GameState.Planting;
        return sample;
	}

    [ProButton]
	public void ShowShop () {
		ShowShopTween ();
	}
	public Tween ShowShopTween ()
    {
		if (Shovel.Instance.IsDigging ())
			Shovel.Instance.UseShovel ();
		Tween sample = Move(menuTransform, bottomPos);
        Move(shopTransform, startPosition);
        Move(topBarTransform, startPosition);
        Move (toShopSignTransform, bottomPos);
		Move (herbariumTransform, topPos);
		herbariumShown = false;

		GameManager.GameState = GameState.Shop;
        return sample;
	}

    [ProButton]
    public void ShowHerbarium() {
        ShowHerbariumTween ();
    }
    public Tween ShowHerbariumTween() {
		if (Shovel.Instance.IsDigging ())
			Shovel.Instance.UseShovel ();
		Tween sample = Move (menuTransform, bottomPos);
		Move (topBarTransform, startPosition);
        Move (toShopSignTransform, bottomPos);
        Move (herbariumTransform, startPosition);
        herbariumShown = true;
		OnHerbierShow?.Invoke ();

		return sample;
	}
    public void ToggleHerbarium() {
        if (herbariumShown) {
            if (GameManager.GameState == GameState.Shop)
                ShowShop ();
            else
                ShowGame ();
        } else
            ShowHerbarium ();
    }

    [ProButton]
    public void ShowTutorial() {
        Move (menuTransform, bottomPos);
        Move(tutorialTransform, startPosition);
    }

    [ProButton]
    public void ShowCredits () {
		Move (menuTransform, bottomPos);
		Move (creditsTransform, startPosition);
	}
}
