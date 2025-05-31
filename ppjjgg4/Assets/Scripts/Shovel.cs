using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class Shovel : MonoBehaviour, IPointerDownHandler {

    public static Shovel Instance;

    [SerializeField] private Sprite idleSprite;
    [SerializeField] private Sprite usingSprite;
    private SpriteRenderer sr;

	private void Awake () {
        Instance = this;
		sr = GetComponent<SpriteRenderer>();
	}

	void Start () {
		sr.sprite = idleSprite;
    }

    public void UseShovel () {
        if (GameManager.GameState == GameState.Planting) {
			// Change the sprite to cancel
			sr.sprite = usingSprite;
            GameManager.GameState = GameState.Digging;
        } else if (GameManager.GameState == GameState.Digging) {
			// Change the sprite back to shovel
			sr.sprite = idleSprite;
            GameManager.GameState = GameState.Planting;
        } else {
            Debug.LogWarning ("Shovel can only be used in Planting or Digging state.");
            return;
        }
    }

    public void DisableShovel () {
        if (IsDigging ())
            UseShovel ();
    }

    public bool IsDigging () {
        return GameManager.GameState == GameState.Digging;
    }

    public void OnPointerDown (PointerEventData eventData) {
        UseShovel ();
    }
}
