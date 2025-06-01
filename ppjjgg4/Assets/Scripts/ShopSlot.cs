using System;
using System.Collections.Generic;
using System.Linq;
using AYellowpaper.SerializedCollections;
using com.cyborgAssets.inspectorButtonPro;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
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
	[SerializeField] private List<Pool> pools;
    public bool isOpen = false;
    private Plant currentPlant;

    private bool mouseOver;
    private InputSystem_Actions actions;

    private Vector3 previewOffset = Vector3.zero;

    [SerializeField] private GameObject ifLocked;

	//public SpriteRenderer tempPreview;

	[HideInInspector]
    public Image imagePreview;

    private void Awake()
    {
        imagePreview = GetComponentInChildren<Image>();
        isOpen = false;
        actions = new();
    }

    private void Update()
    {
        if (mouseOver)
        {
            Vector3 mousePos = Camera.main.ScreenToWorldPoint(actions.UI.Point.ReadValue<Vector2>());
            mousePos.z = 0;
            PreviewController.Instance.PlaceBubble(mousePos + previewOffset);
        }   
    }


    // Roll pour choisir la plante du jour
    [ProButton]
    public void Roll()
    {
        int maxIter = 100; // Mesure de s�curit�
        int iter = 0;
        Pool chosenPool = SelectPool();
        while (!IsRollable(chosenPool) && iter < maxIter)
        {
            chosenPool = SelectPool();
            iter++;
        }
        if (iter >= maxIter) throw new Exception("Could not roll a plant");

        List<Plant> choice = chosenPool.plantPool;
        Plant p = choice[UnityEngine.Random.Range(0, choice.Count)];
        iter = 0;
        while (chosenPool.unique && !IsRollable(p) && iter < maxIter)
        {
            p = choice[UnityEngine.Random.Range(0, choice.Count)];
            iter++;
        }
        if (iter >= maxIter) throw new Exception("Could not roll a plant");

        currentPlant = p;
        switch (chosenPool.rarity)
        {
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

    private Pool SelectPool()
    {
        float r = UnityEngine.Random.Range(0f, 1f);
        float current_p = 0;
        Pool choice = null;
        foreach (Pool pool in pools)
        {
            choice = pool;
            if (r >= current_p && r < (current_p + pool.probability))
            {
                break;
            }
            current_p += pool.probability;
        }
        return choice;
    }

    private bool IsRollable(Plant plant) => Garden.Instance.GetSpecies(plant.Species).Count() == 0;
    private bool IsRollable(Pool pool) => !pool.unique || pool.plantPool.Where(p => IsRollable(p)).Count() >= 1;



    // Open slot when score is high enough
    public void Open()
    {
        isOpen = true;
        if (ifLocked != null)
            ifLocked.SetActive (false);
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
        mouseOver = true;
        actions.Enable();
        Vector3 mousePos = Camera.main.ScreenToWorldPoint(actions.UI.Point.ReadValue<Vector2>());
        mousePos.z = 0;

        PreviewController.Instance.ShowBubble(mousePos + previewOffset, currentPlant.GetShopDescription());
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        mouseOver = false;
        actions.Disable();
        PreviewController.Instance.HideBubble();
    }
}
