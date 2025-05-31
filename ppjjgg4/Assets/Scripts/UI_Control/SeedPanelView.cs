using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

public class SeedPanelView : MonoBehaviour {
	public static SeedPanelView Instance;

	[SerializeField] private Transform parent;
	[SerializeField] private Image image;

	private void Awake () {
		Instance = this;
	}
	private void Start () {
		ClearImage ();
	}

	public void SetImage (Sprite sprite) {
		image.gameObject.SetActive (true);
		image.sprite = sprite;
		if (GameManager.GameState != GameState.Planting && GameManager.GameState != GameState.Digging) {
			parent.localPosition = new Vector3 (0, 0, 0);
		} else {
			parent.DOLocalMoveY (0, UI_Controller.Instance.moveDuration).SetEase (UI_Controller.Instance.moveEase, UI_Controller.Instance.overshoot);
		}
	}

	public void ClearImage () {
		if (GameManager.GameState != GameState.Planting && GameManager.GameState != GameState.Digging) {
			parent.localPosition = new Vector3 (0, -1080, 0);
			image.gameObject.SetActive (false);
		} else {
			parent.DOLocalMoveY (-1080, UI_Controller.Instance.moveDuration).SetEase (UI_Controller.Instance.moveEase, UI_Controller.Instance.overshoot).OnComplete (() => {
				image.gameObject.SetActive (false);
			});
		}
	}
}
