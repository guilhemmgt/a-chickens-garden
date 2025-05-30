using UnityEngine;
using DG.Tweening;
using com.cyborgAssets.inspectorButtonPro;

public class UI_Controller : MonoBehaviour
{

    [Header("UI Elements")]
    [SerializeField] private RectTransform menuTransform;
    [SerializeField] private RectTransform topBarTransform;
    [SerializeField] private RectTransform shopTransform;

    [Header("Anim settings")]
    [SerializeField] private float moveDuration = 0.5f;
    [SerializeField] private Ease moveEase = Ease.Linear;

    private Vector3 startPosition;

    private float screenWidth;
    private float screenHeight;

    void Start()
    {
        screenWidth = Camera.main.pixelWidth;
        screenHeight = Camera.main.pixelHeight;

        startPosition = menuTransform.position;

        // Shop déplacé hors écran à droite
        shopTransform.position = new Vector3(startPosition.x + screenWidth, startPosition.y, startPosition.z);

        // TopBar déplacée hors écran en haut
        topBarTransform.position = new Vector3(startPosition.x, startPosition.y + screenHeight, startPosition.z);
    }

    public void Move(RectTransform targetTransform, Vector2 direction)
    {
        Vector2 targetPosition = new Vector2(startPosition.x + screenWidth * direction.x, 
            startPosition.y + screenHeight * direction.y); 
        targetTransform.DOMove(targetPosition, moveDuration).SetEase(moveEase);
    }

    [ProButton]
    public void ShowMenu()
    {
        Move(menuTransform, Vector2.zero);
        Move(shopTransform, Vector2.right);
        Move(topBarTransform, Vector2.up);

        GameManager.GameState = GameState.Menu;
    }

    [ProButton]
    public void ShowGame()
    {
        Move(menuTransform, Vector2.left);
        Move(shopTransform, Vector2.right);
        Move(topBarTransform, Vector2.zero);

        GameManager.GameState = GameState.Planting;
    }

    [ProButton]
    public void ShowShop()
    {
        Move(menuTransform, Vector2.left);
        Move(shopTransform, Vector2.zero);
        Move(topBarTransform, Vector2.zero);

        GameManager.GameState = GameState.Shop;
    }
}
