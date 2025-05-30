using System.Collections.Generic;
using AYellowpaper.SerializedCollections;
using com.cyborgAssets.inspectorButtonPro;
using UnityEngine;
using UnityEngine.UIElements;

public class ShopSlot : MonoBehaviour
{
    [SerializeField] private Shop shop;
    [SerializeField] private SerializedDictionary<float, List<Plant>> pool;
    public bool isOpen = false;
    private Plant currentPlant;

    public SpriteRenderer tempPreview;


    private void Awake()
    {
        tempPreview = GetComponent<SpriteRenderer>();
        isOpen = false;
        tempPreview.color = Color.red;
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
        tempPreview.sprite = p.Sprite;
    }

    // Open slot when score is high enough
    public void Open()
    {
        isOpen = true;
        tempPreview.color = Color.white;
    }

    // Close slot after plant was chosen today
    public void Close()
    {
        tempPreview.color = Color.gray;
    }

    [ProButton]
    public void OnClick()
    {
        if (isOpen) shop.OnPlantSelected(this, currentPlant);
    }
    
    
}
