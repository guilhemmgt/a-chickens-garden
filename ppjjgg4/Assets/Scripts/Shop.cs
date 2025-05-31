using System.Collections.Generic;
using AYellowpaper.SerializedCollections;
using com.cyborgAssets.inspectorButtonPro;
using DG.Tweening;
using UnityEngine;

public class Shop : MonoBehaviour
{
    [SerializeField] private ShopSlot commonSlot;
    [SerializeField] private int rareUnlockScore;
    [SerializeField] private ShopSlot rareSlot;
    [SerializeField] private int uniqueUnlockScore;
    [SerializeField] private ShopSlot uniqueSlot;

    [SerializeField] public Sprite emptyCard;
	[SerializeField] public Sprite commonCard;
	[SerializeField] public Sprite rareCard;
	[SerializeField] public Sprite uniqueCard;
	[SerializeField] public Sprite lockedCommonCard;
	[SerializeField] public Sprite lockedRareCard;
	[SerializeField] public Sprite lockedUniqueCard;

    [SerializeField] private GameObject soldOutSign;

	private bool hasBeenRolledToday = false;

    private void OnEnable()
    {
        GameManager.OnDayEnded += () => hasBeenRolledToday = false;
    }

    [ProButton]
    public void Open()
    {
        if (!hasBeenRolledToday) {        
			soldOutSign.SetActive (false);

			commonSlot.Open();
            if (!rareSlot.isOpen && rareUnlockScore <= GameManager.Instance.score) rareSlot.Open();
            if (!uniqueSlot.isOpen && uniqueUnlockScore <= GameManager.Instance.score) uniqueSlot.Open();

            Roll();
            hasBeenRolledToday = true;
        }
    }

    [ProButton]
    public void Roll()
    {
        commonSlot.Roll();
        rareSlot.Roll();
        uniqueSlot.Roll();
    }

    public void OnPlantSelected(ShopSlot slot, Plant plant)
    {
        ChoiceHandler.Instance.SetCurrentPlant(plant);
        CloseShop();
        UI_Controller.Instance.ShowGameTween ().OnComplete (() => { soldOutSign.SetActive (true); });
    }

    public void CloseShop()
    {
        commonSlot.Close();
        rareSlot.Close();
        uniqueSlot.Close ();
	}

}


