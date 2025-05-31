using UnityEngine;
using DG.Tweening;
using com.cyborgAssets.inspectorButtonPro;

public class UI_Controller : MonoBehaviour
{
    public static UI_Controller Instance;

    [Header("UI Elements")]
    [SerializeField] private RectTransform menuTransform;
    [SerializeField] private RectTransform topBarTransform;
	[SerializeField] private RectTransform shopTransform;
	[SerializeField] private RectTransform toShopSignTransform;

	[Header("Anim settings")]
    [SerializeField] private float moveDuration = 0.5f;
    [SerializeField] private Ease moveEase = Ease.Linear;
	[SerializeField] private float overshoot = 0.25f;

	private Vector3 startPosition;

    private Vector3 leftPos = new Vector3 (-285, 0, 90);
    private Vector3 rightPos = new Vector3 (285, 0, 90);
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
        Move(toShopSignTransform, bottomPos);
        
        GameManager.GameState = GameState.Shop;
        return sample;
	}
}
