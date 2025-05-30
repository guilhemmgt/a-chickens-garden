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
	[SerializeField] private float overshoot = 0.25f;

	private Vector3 startPosition;

    private Vector3 leftPos = new Vector3 (-221, -9, 90);
    private Vector3 rightPos = new Vector3 (355, -9, 90);
    private Vector3 topPos = new Vector3 (69, 156, 90);

    void Start()
    {
        startPosition = menuTransform.position;
        Debug.Log (startPosition);
        // Shop déplacé hors écran à droite
        shopTransform.position = rightPos;

        // TopBar déplacée hors écran en haut
        topBarTransform.position = leftPos;
    }

    public void Move(RectTransform targetTransform, Vector3 targetPosition)
    {
        targetTransform.DOMove (targetPosition, moveDuration).SetEase (moveEase, overshoot);
    }

    [ProButton]
    public void ShowMenu()
    {
        Move(menuTransform, startPosition);
        Move(shopTransform, rightPos);
        Move(topBarTransform, topPos);
    }

    [ProButton]
    public void ShowGame()
    {
        Move(menuTransform, leftPos);
        Move(shopTransform, rightPos);
        Move(topBarTransform, startPosition);
    }

    [ProButton]
    public void ShowShop()
    {
        Move(menuTransform, leftPos);
        Move(shopTransform, startPosition);
        Move(topBarTransform, topPos);
    }
}
