using UnityEngine;
using TMPro;
using DG.Tweening;

public class PreviewController : MonoBehaviour
{
    public static PreviewController Instance { get; private set; }

    [SerializeField] private GameObject bubblePrefab;
    private GameObject currentBubble;
    private CanvasGroup bubbleCanvasGroup;
    private RectTransform canvasRect;
    private Camera mainCamera;

    private void Awake()
    {
        Instance = this;
        canvasRect = GetComponentInParent<Canvas>().GetComponent<RectTransform>();
        mainCamera = Camera.main;
    }

    public void ShowBubble(Vector3 worldPosition, string infoText)
    {
        if (currentBubble == null)
        {
            currentBubble = Instantiate(bubblePrefab, transform);
            bubbleCanvasGroup = currentBubble.GetComponent<CanvasGroup>();

            if (bubbleCanvasGroup == null)
                bubbleCanvasGroup = currentBubble.AddComponent<CanvasGroup>();
        }

        currentBubble.SetActive(true);
        currentBubble.GetComponentInChildren<TMP_Text>().text = infoText;

        Vector2 screenPoint = RectTransformUtility.WorldToScreenPoint(mainCamera, worldPosition);
        currentBubble.GetComponent<RectTransform>().position = screenPoint + new Vector2(0, 80);

        // Reset and fade in
        bubbleCanvasGroup.alpha = 0;
        bubbleCanvasGroup.DOKill(); // Stop any existing tweens
        bubbleCanvasGroup.DOFade(1f, 0.3f);
    }

    public void UpdateInfo(string infoText)
    {
        if (currentBubble != null)
        {
            TMP_Text textComponent = currentBubble.GetComponentInChildren<TMP_Text>();
            if (textComponent != null)
            {
                textComponent.text = infoText;
            }
        }
    }


    public void HideBubble()
    {
        if (currentBubble != null && bubbleCanvasGroup != null)
        {
            bubbleCanvasGroup.DOKill();
            bubbleCanvasGroup.DOFade(0f, 0.3f).OnComplete(() =>
            {
                currentBubble.SetActive(false);
            });
        }
    }
}
