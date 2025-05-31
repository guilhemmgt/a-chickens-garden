using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class Shovel : MonoBehaviour
{
    [SerializeField] private Sprite shovelSprite;
    [SerializeField] private Sprite cancelSprite; // May be null

    [SerializeField] private Image shovelImage;

    private CanvasGroup canvasGroup;

    void Start()
    {
        shovelImage.sprite = shovelSprite;
        canvasGroup = shovelImage.GetComponent<CanvasGroup>();
    }

    private void LateUpdate()
    {
        if (GameManager.GameState == GameState.Digging)
        {
            if (shovelImage != null && shovelImage.canvas != null)
            {
                shovelImage.enabled = true;

                canvasGroup.blocksRaycasts = false;

                Vector2 mouseScreenPos = Mouse.current.position.ReadValue();
                RectTransform canvasRect = shovelImage.canvas.GetComponent<RectTransform>();

                Vector2 localPoint;
                bool success = RectTransformUtility.ScreenPointToLocalPointInRectangle(
                    canvasRect,
                    mouseScreenPos,
                    shovelImage.canvas.renderMode == RenderMode.ScreenSpaceOverlay ? null : shovelImage.canvas.worldCamera,
                    out localPoint
                );

                if (success)
                {
                    shovelImage.rectTransform.localPosition = localPoint;
                }
            }
        }
        else
        {
            if (shovelImage != null)
            {
                shovelImage.enabled = false;
                canvasGroup.blocksRaycasts = true;
            }
        }
    }


    public void UseShovel()
    {
        if (GameManager.GameState == GameState.Planting)
        {
            // Change the sprite to cancel
            shovelImage.sprite = cancelSprite;
            GameManager.GameState = GameState.Digging;
        }
        else if (GameManager.GameState == GameState.Digging)
        {
            // Change the sprite back to shovel
            shovelImage.sprite = shovelSprite;
            GameManager.GameState = GameState.Planting;
        }
        else
        {
            Debug.LogWarning("Shovel can only be used in Planting or Digging state.");
            return;
        }
    }

    public void DisableShovel()
    {
        Debug.Log("Disabling shovel");
        // Change the sprite back to shovel
        shovelImage.sprite = shovelSprite;
        GameManager.GameState = GameState.Planting;
    }
}
