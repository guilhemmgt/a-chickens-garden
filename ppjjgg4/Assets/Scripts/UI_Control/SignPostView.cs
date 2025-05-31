using DG.Tweening;
using UnityEngine;
using UnityEngine.EventSystems;
public class SignPost : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler {
	[SerializeField] private float offsetDuration;
	[SerializeField] private float offsetOnHover;

	private float startY;

	private void Awake () {
		startY = transform.position.y;
	}

	public void OnPointerEnter (PointerEventData pointerEventData) {
		transform.DOMoveY (startY + offsetOnHover, offsetDuration);
	}

	public void OnPointerExit (PointerEventData pointerEventData) {
		transform.DOMoveY (startY, offsetDuration);
	}
}
