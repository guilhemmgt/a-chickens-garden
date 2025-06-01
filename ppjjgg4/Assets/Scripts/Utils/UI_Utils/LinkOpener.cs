using UnityEngine;
using TMPro;
using UnityEngine.EventSystems;

[RequireComponent (typeof (TextMeshProUGUI))]
public class LinkOpener : MonoBehaviour, IPointerClickHandler {
	private TextMeshProUGUI textMeshPro;

	void Awake () {
		textMeshPro = GetComponent<TextMeshProUGUI> ();
	}

	public void OnPointerClick (PointerEventData eventData) {
		int linkIndex = TMP_TextUtilities.FindIntersectingLink (textMeshPro, eventData.position, eventData.enterEventCamera);

		if (linkIndex != -1) {
			TMP_LinkInfo linkInfo = textMeshPro.textInfo.linkInfo[linkIndex];
			string url = linkInfo.GetLinkID ();
			Debug.Log ("Clicked URL: " + url);
			Application.OpenURL (url);
		}
	}
}