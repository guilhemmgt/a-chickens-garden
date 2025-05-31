using System;
using System.Collections.Generic;
using System.Linq;
using AYellowpaper.SerializedCollections;
using com.cyborgAssets.inspectorButtonPro;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class ShopSlot : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    public enum Rarity {
        COMMON, RARE, UNIQUE
    }

    [Serializable]
    public class Pool
    {
        public Rarity rarity;
		public bool unique;
        public float probability;
        public List<Plant> plantPool;
    }
    [SerializeField] private Shop shop;
	[SerializeField] private TextMeshProUGUI text;
	[SerializeField] private GameObject unlockText;
	[SerializeField] private List<Pool> pools;
    public bool isOpen = false;
    private Plant currentPlant;

    private bool mouseOver;
    private InputSystem_Actions actions;

    //public SpriteRenderer tempPreview;

    [HideInInspector]
    public Image imagePreview;

    private void Awake()
    {
        imagePreview = GetComponentInChildren<Image>();
        isOpen = false;
        actions = new();
    }

    void Update()
    {
        if (mouseOver)
        {
            Vector3 mousePos = actions.UI.Point.ReadValue<Vector2>();
            Vector3 worldMousePos = Camera.main.ScreenToWorldPoint(mousePos);
            worldMousePos.z = 0;
            PreviewController.Instance.PlaceBubble(worldMousePos + new Vector3(3,3,0));
        }
    }


    // Roll pour choisir la plante du jour
    [ProButton]
    public void Roll()
    {
        float r = UnityEngine.Random.Range(0f, 1f);
        float current_p = 0;
        Pool chosenPool = null;
        List<Plant> choice = new();
        foreach (Pool pool in pools)
        {
            chosenPool = pool;
            if (r >= current_p && r < (current_p + pool.probability))
            {
                choice = pool.plantPool;
                break;
            }
            current_p += pool.probability;
        }
        Plant p = choice[UnityEngine.Random.Range(0, choice.Count)];
        if (chosenPool.unique && Garden.Instance.GetSpecies(p.Species).Count() > 0)
        {
            Roll();
        }
        else
        {
            currentPlant = p;
            switch (chosenPool.rarity) {
                case Rarity.COMMON:
                    imagePreview.sprite = isOpen ? shop.commonCard : shop.lockedCommonCard;
                    break;
                case Rarity.RARE:
					imagePreview.sprite = isOpen ? shop.rareCard : shop.lockedRareCard;
					break;
                case Rarity.UNIQUE:
					imagePreview.sprite = isOpen ? shop.uniqueCard : shop.lockedUniqueCard;
					break;
			}
            text.text = currentPlant.name;
        }
    }

    // Open slot when score is high enough
    public void Open()
    {
        isOpen = true;
        if (unlockText != null)
            unlockText.SetActive (false);
	}

    // Close slot after plant was chosen today
    public void Close()
    {
        //imagePreview.sprite = shop.emptyCard;
		isOpen = false;
    }

    [ProButton]
    public void OnClick()
    {
        if (isOpen) shop.OnPlantSelected(this, currentPlant);
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        actions.Enable();
        mouseOver = true;
        Vector3 worldMousePos = actions.UI.Point.ReadValue<Vector2>();
        worldMousePos.z = 0;
        PreviewController.Instance.ShowBubble(worldMousePos + new Vector3(3,3,0), currentPlant.skill.Name + " :\n" + currentPlant.skill.Description);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        actions.Disable();
        mouseOver = false;
        PreviewController.Instance.HideBubble();
    }
}
