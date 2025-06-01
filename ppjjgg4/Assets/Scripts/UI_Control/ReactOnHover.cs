using DG.Tweening;
using UnityEngine;
using UnityEngine.EventSystems;
public class ReactOnHover : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler {
	[SerializeField] private float offsetDuration;
	[SerializeField] private float offsetOnHover;
	[SerializeField] private bool onFirstChildrenOnly = false; // pose pas de questions

	private float startY;

	private void Awake () {
		startY = onFirstChildrenOnly ? transform.GetChild(1).localPosition.y : transform.localPosition.y;
	}

	public void OnPointerEnter (PointerEventData pointerEventData) {
		(onFirstChildrenOnly ? transform.GetChild (1) : transform).DOLocalMoveY(startY + offsetOnHover, offsetDuration);
	}

	public void OnPointerExit (PointerEventData pointerEventData) {
		(onFirstChildrenOnly ? transform.GetChild (1) : transform).DOLocalMoveY (startY, offsetDuration);
	}
}
