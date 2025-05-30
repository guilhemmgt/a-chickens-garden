using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class TopBarView : MonoBehaviour {
	public static TopBarView Instance { get; private set; }

	[SerializeField] private TextMeshProUGUI scoreText;
	[SerializeField] private TextMeshProUGUI dayText;
	[SerializeField] private Image eventImage;

	private void Awake () {
		Instance = this;
	}

	public void SetScore(int score) {
		scoreText.text = "Score: " + score;
	}

	public void SetDay(int day) {
		dayText.text = "Day: " + day;
	}

	public void SetEventSprite(Sprite sprite) {
		eventImage.sprite = sprite;
	}
}
