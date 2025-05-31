using UnityEngine;
using DG.Tweening;
using com.cyborgAssets.inspectorButtonPro;

public class UI_Controller : MonoBehaviour
{

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

    void Start()
    {
        startPosition = menuTransform.position;

        ShowMenu (true);

	}

    public void Move (RectTransform targetTransform, Vector3 targetPosition, bool instantSpeed = false)
    {
        if (instantSpeed)
            targetTransform.position = targetPosition;
        else
            targetTransform.DOMove (targetPosition, moveDuration).SetEase (moveEase, overshoot);
    }

    [ProButton]
    public void ShowMenu (bool instantSpeed = false)
    {
        GameManager.GameState = GameState.Menu;
        Move(menuTransform, startPosition, instantSpeed);
        Move(shopTransform, rightPos, instantSpeed);
        Move(topBarTransform, topPos, instantSpeed);
        Move(toShopSignTransform, bottomPos, instantSpeed);
    }

    [ProButton]
    public void ShowGame()
    {
        Move(menuTransform, leftPos);
        Move(shopTransform, rightPos);
        Move(topBarTransform, startPosition);
        Move(toShopSignTransform, startPosition);
        
        GameManager.GameState = GameState.Planting;
	}

    [ProButton]
    public void ShowShop()
    {
        Move(menuTransform, leftPos);
        Move(shopTransform, startPosition);
        Move(topBarTransform, startPosition);
        Move(toShopSignTransform, bottomPos);
        
        GameManager.GameState = GameState.Shop;
	}
}
