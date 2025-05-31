using DG.Tweening;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ButtonMagnifier : MonoBehaviour, ISelectHandler, IDeselectHandler, IPointerEnterHandler, IPointerExitHandler
{
    Button button;

    [SerializeField] private float scaleFactor = 1.3f;
    [SerializeField] private float timeToAnim = 0.3f;

    private void Start()
    {
        button = GetComponent<Button>();
        button.onClick.AddListener(() => AudioController.Instance.PlayButtonClickSound());
    }
    public void OnDeselect(BaseEventData eventData)
    {
        button.transform.DOScale(Vector3.one, timeToAnim);
        return;
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        button.Select();
        return;
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        return;
    }

    public void OnSelect(BaseEventData eventData)
    {
        button.transform.DOScale(scaleFactor * Vector3.one, timeToAnim);
    }
}