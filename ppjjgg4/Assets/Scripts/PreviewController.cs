using UnityEngine;
using TMPro;
using DG.Tweening;

public class PreviewController : MonoBehaviour
{
    public static PreviewController Instance { get; private set; }

    [SerializeField] private GameObject bubbleObject;
    private CanvasGroup bubbleCanvasGroup;
    private RectTransform canvasRect;
    private Camera mainCamera;

    private void Awake()
    {
        Instance = this;
        canvasRect = GetComponentInParent<Canvas>().GetComponent<RectTransform>();
        mainCamera = Camera.main;

        // Obtenir le CanvasGroup existant ou en ajouter un
        if (bubbleObject != null)
        {
            bubbleCanvasGroup = bubbleObject.GetComponent<CanvasGroup>();
            if (bubbleCanvasGroup == null)
                bubbleCanvasGroup = bubbleObject.AddComponent<CanvasGroup>();
        }
    }

    public void ShowBubble(Vector3 worldPosition, string infoText)
    {
        Debug.Log($"Showing bubble at {worldPosition} with text: {infoText}");

        if (bubbleObject == null) return;

        bubbleObject.SetActive(true);
        bubbleObject.GetComponentInChildren<TMP_Text>().text = infoText;

        PlaceBubble(worldPosition);

        // Fade in
        bubbleCanvasGroup.alpha = 0;
        bubbleCanvasGroup.DOKill();
        bubbleCanvasGroup.DOFade(1f, 0.3f);
    }

    public void PlaceBubble(Vector3 worldPosition)
    {
        // Conversion position monde -> position �cran -> position locale canvas
        Vector2 screenPoint = RectTransformUtility.WorldToScreenPoint(mainCamera, worldPosition);
        RectTransform bubbleRect = bubbleObject.GetComponent<RectTransform>();
        Vector2 anchoredPos;

        if (RectTransformUtility.ScreenPointToLocalPointInRectangle(canvasRect, screenPoint, mainCamera, out anchoredPos))
        {
            bubbleRect.anchoredPosition = anchoredPos + new Vector2(0f, 150f); // D�calage vertical
        }
    }

    public void UpdateInfo(string infoText)
    {
        if (bubbleObject != null)
        {
            TMP_Text textComponent = bubbleObject.GetComponentInChildren<TMP_Text>();
            if (textComponent != null)
            {
                textComponent.text = infoText;
            }
        }
    }

    public void HideBubble()
    {
        if (bubbleObject != null && bubbleCanvasGroup != null)
        {
            bubbleCanvasGroup.DOKill();
            bubbleCanvasGroup.DOFade(0f, 0.3f).OnComplete(() =>
            {
                bubbleObject.SetActive(false);
            });
        }
    }
}
