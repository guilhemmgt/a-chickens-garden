using DG.Tweening;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class Chicken : MonoBehaviour, IPointerClickHandler {

	public static Chicken Instance;
	public event Action OnChickenClick = delegate { };

    [SerializeField] private Transform mask;

	[SerializeField] private float targetScale;

	[SerializeField] private Transform spotsParent;

	[SerializeField] private float inDuration1;
	[SerializeField] private float pauseDuration1;
	[SerializeField] private Ease inEase1;

	[SerializeField] private float inDuration2;
	[SerializeField] private float pauseDuration2;
	[SerializeField] private Ease inEase2;

	[SerializeField] private float outDuration;
	[SerializeField] private Ease outEase;

	private GameObject eventSystem;
	private int i = 0;


	private void Awake()
	{
		Instance = this;
	}

	private void Start () {
		eventSystem = GameObject.Find ("EventSystem");
		transform.position = spotsParent.GetChild (i).position;
	}

	public Tween FadeIn() {
		eventSystem.SetActive (false);
		mask.position = transform.position;
		Sequence seq = DOTween.Sequence ();
		seq.Append (mask.DOScale (targetScale, inDuration1).SetEase (inEase1));
		seq.Append (DOVirtual.DelayedCall (pauseDuration1, () => { }));

		seq.Append (mask.DOScale (0, inDuration2).SetEase (inEase2));
		seq.Append (DOVirtual.DelayedCall (pauseDuration2, () => {
			i = (i + 1) % spotsParent.childCount;
			transform.position = spotsParent.GetChild (i).position;
			mask.position = transform.position;
		}));
		return seq;
	}

	public void FadeOut() {
		mask.DOScale (700, outDuration).OnComplete (() => {
			mask.position = Vector3.zero;
			eventSystem.SetActive (true);
		});
	}

    public void OnPointerClick(PointerEventData eventData)
    {
		OnChickenClick?.Invoke();
    }
}
