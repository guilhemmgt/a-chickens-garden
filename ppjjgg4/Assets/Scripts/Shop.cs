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


    public SpriteRenderer tempChosen;

    private void Awake()
    {
        tempChosen = GetComponent<SpriteRenderer>();
        
    }

    [ProButton]
    public void Open()
    {
        commonSlot.Open();
        if (!rareSlot.isOpen && rareUnlockScore <= GameManager.Instance.score) rareSlot.Open();
        if (!uniqueSlot.isOpen && uniqueUnlockScore <= GameManager.Instance.score) uniqueSlot.Open();
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
        slot.tempPreview.color = Color.gold;
        tempChosen.sprite = plant.Sprite;
    }

}


