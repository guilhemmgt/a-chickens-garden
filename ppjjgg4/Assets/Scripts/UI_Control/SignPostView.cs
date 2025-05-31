using DG.Tweening;
using UnityEngine;
using UnityEngine.EventSystems;
public class SignPost : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler {
	[SerializeField] private float offsetDuration;
	[SerializeField] private float offsetOnHover;

	private float startY;

	private void Awake () {
		startY = transform.GetChild(0).localPosition.y;
	}

	public void OnPointerEnter (PointerEventData pointerEventData) {
		transform.GetChild (0).DOLocalMoveY(startY + offsetOnHover, offsetDuration);
	}

	public void OnPointerExit (PointerEventData pointerEventData) {
		transform.GetChild (0).DOLocalMoveY (startY, offsetDuration);
	}
}
