using System.Collections.Generic;
using AYellowpaper.SerializedCollections;
using com.cyborgAssets.inspectorButtonPro;
using UnityEngine;
using UnityEngine.UI;

public class ShopSlot : MonoBehaviour
{
    [SerializeField] private Shop shop;
    [SerializeField] private SerializedDictionary<float, List<Plant>> pool;
    public bool isOpen = false;
    private Plant currentPlant;

    //public SpriteRenderer tempPreview;

    [HideInInspector]
    public Image imagePreview;

    private void Awake()
    {
        imagePreview = GetComponentInChildren<Image>();
        isOpen = false;
        imagePreview.color = Color.black;
    }


    // Roll pour choisir la plante du jour
    [ProButton]
    public void Roll()
    {
        float r = Random.Range(0f, 1f);
        float current_p = 0;
        List<Plant> choice = new();
        foreach (float key in pool.Keys)
        {
            if (r >= current_p && r < (current_p + key))
            {
                choice = pool[key];
                break;
            }
            current_p += key;
        }
        Plant p = choice[Random.Range(0, choice.Count)];
        currentPlant = p;
        imagePreview.sprite = p.GetMatureSprite();
    }

    // Open slot when score is high enough
    public void Open()
    {
        isOpen = true;
        imagePreview.color = Color.white;
    }

    // Close slot after plant was chosen today
    public void Close()
    {
        if (isOpen)
        {
            imagePreview.color = Color.gray;
        }
        else
        {
            imagePreview.color = Color.black;
        }
        isOpen = false;
    }

    [ProButton]
    public void OnClick()
    {
        if (isOpen) shop.OnPlantSelected(this, currentPlant);
    }
    
    
}
