using DG.Tweening;
using UnityEngine;
using UnityEngine.EventSystems;
public class SignPostView : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler {
	[SerializeField] private float offsetDuration;
	[SerializeField] private float offsetOnHover;

	private float startY;

	private void Awake () {
		startY = transform.GetChild(1).localPosition.y;
	}

	public void OnPointerEnter (PointerEventData pointerEventData) {
		transform.GetChild (1).DOLocalMoveY(startY + offsetOnHover, offsetDuration);
	}

	public void OnPointerExit (PointerEventData pointerEventData) {
		transform.GetChild (1).DOLocalMoveY (startY, offsetDuration);
	}
}
