using UnityEngine;
using UnityEngine.EventSystems;

public class Pickaxe : MonoBehaviour, IPointerDownHandler {
	public static Pickaxe Instance;

	[SerializeField] private Sprite lockedSprite;
	[SerializeField] private Sprite idleSprite;
	[SerializeField] private Sprite usingSprite;
	private SpriteRenderer sr;

	[SerializeField] private bool unlocked;

	private void Awake () {
		Instance = this;
		sr = GetComponent<SpriteRenderer> ();
	}

	private void Start () {
		sr.sprite = lockedSprite;
	}

	public void UsePickaxe () {
		if (GameManager.GameState == GameState.Planting && unlocked) {
			// Change the sprite to cancel
			Shovel.Instance.DisableShovel ();
			sr.sprite = usingSprite;
			GameManager.GameState = GameState.Mining;
		} else if (GameManager.GameState == GameState.Mining) {
			// Change the sprite back to shovel
			sr.sprite = idleSprite;
			GameManager.GameState = GameState.Planting;
		} else {
			Debug.LogWarning ("Shovel can only be used in Planting or Mining state.");
			return;
		}
	}

	public void DisablePickaxe () {
		if (IsMining ())
			UsePickaxe ();
	}

	public void UnlockPickaxe () {
		unlocked = true;
		sr.sprite = idleSprite;
	}

	public bool IsMining () {
		return GameManager.GameState == GameState.Mining;
	}

	public void OnPointerDown (PointerEventData eventData) {
		UsePickaxe ();
	}
}
