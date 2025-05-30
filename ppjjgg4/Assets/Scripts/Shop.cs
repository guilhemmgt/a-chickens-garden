using System.Collections.Generic;
using AYellowpaper.SerializedCollections;
using com.cyborgAssets.inspectorButtonPro;
using UnityEngine;

public class Shop : MonoBehaviour
{
    [SerializeField] private ShopSlot commonSlot;
    [SerializeField] private int rareUnlockScore;
    [SerializeField] private ShopSlot rareSlot;
    [SerializeField] private int uniqueUnlockScore;
    [SerializeField] private ShopSlot uniqueSlot;

    private bool hasBeenRolledToday = false;

    private void OnEnable()
    {
        GameManager.OnDayEnded += () => hasBeenRolledToday = false;
    }

    [ProButton]
    public void Open()
    {
        commonSlot.Open();
        if (!rareSlot.isOpen && rareUnlockScore <= GameManager.Instance.score) rareSlot.Open();
        if (!uniqueSlot.isOpen && uniqueUnlockScore <= GameManager.Instance.score) uniqueSlot.Open();

        if (!hasBeenRolledToday)
        {
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
        slot.imagePreview.color = Color.gold;
        ChoiceHandler.Instance.SetCurrentPlant(plant);
    }

}


