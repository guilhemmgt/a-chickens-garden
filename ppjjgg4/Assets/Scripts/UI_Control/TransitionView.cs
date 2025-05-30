using DG.Tweening;
using UnityEngine;

public class TransitionView : MonoBehaviour {

	public static TransitionView Instance;

	[SerializeField] private Transform mask;
	[SerializeField] private Transform targetTransform;

	[SerializeField] private float targetScale;

	[SerializeField] private float inDuration1;
	[SerializeField] private float pauseDuration1;
	[SerializeField] private Ease inEase1;

	[SerializeField] private float inDuration2;
	[SerializeField] private float pauseDuration2;
	[SerializeField] private Ease inEase2;

	[SerializeField] private float outDuration;
	[SerializeField] private Ease outEase;

	private void Awake () {
		Instance = this;
	}

	public Tween FadeIn() {
		mask.position = targetTransform.position;
		Sequence seq = DOTween.Sequence ();
		seq.Append (mask.DOScale (targetScale, inDuration1).SetEase (inEase1));
		seq.Append (DOVirtual.DelayedCall (pauseDuration1, () => { }));

		seq.Append (mask.DOScale (0, inDuration2).SetEase (inEase2));
		seq.Append (DOVirtual.DelayedCall (pauseDuration2, () => { }));
		return seq;
	}

	public void FadeOut() {
		mask.DOScale (700, outDuration).OnComplete (() => { mask.position = Vector3.zero; });
	}
}
