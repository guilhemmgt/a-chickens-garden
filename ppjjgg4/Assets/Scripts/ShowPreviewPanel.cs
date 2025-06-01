using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ShowPreviewPanel : MonoBehaviour, IPointerEnterHandler
{
    private Image image;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        image = GetComponent<Image>();

        if (image == null)
        {
            Debug.LogError("Image component not found on the GameObject.");
        }
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        PreviewController.Instance.ShowBubble(transform.position + (20) * Vector3.right, ChoiceHandler.Instance.GetCurrentPlant().GetShopDescription());
    }
}