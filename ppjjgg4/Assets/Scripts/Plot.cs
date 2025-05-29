using System;
using System.Collections.Generic;
using com.cyborgAssets.inspectorButtonPro;
using Unity.VisualScripting;
using Unity.XR.GoogleVr;
using UnityEngine;
using UnityEngine.EventSystems;

public class Plot : MonoBehaviour, IPointerClickHandler
{
    public event Action<Plant> OnPlantEnter;
    public event Action<Plant> OnPlantExit;
    public enum Type { Rock, Soil }
    public static Plot Instantiate(int i, int j)
    {
        Plot plot = new GameObject().AddComponent<Plot>();
        plot.i = i;
        plot.j = j;
        return plot;
    }
    public int i { get; set; }
    public int j { get; set; }

    [field : SerializeField] public List<Effect> effects { get; private set; }
    [SerializeField] private Type type = Type.Soil;
    private SpriteRenderer sr => GetComponent<SpriteRenderer>();
    [field: SerializeField] public Plant plant { get; private set; }


    private void Awake()
    {
        effects = new();
    }

    [ProButton]
    public void AddPlant(Plant plant)
    {
        if (this.plant != null)
        {
            Debug.LogWarning("Attempting to add plant to not empty plot");
            return;
        }
        this.plant = Instantiate(plant);
        this.plant.OnPlanted(this);
        sr.sprite = this.plant.Sprite;
        OnPlantEnter?.Invoke(this.plant);
    }

    [ProButton]
    public void RemovePlant()
    {
        if (plant != null)
        {
            plant.OnRemoved();
            Destroy(plant);
            plant = null;
            OnPlantExit?.Invoke(plant);
        }
    }

    [ProButton]
    public void EndDay()
    {
        if (plant != null)
        {
            plant.EndDay();
            sr.sprite = plant.Sprite;
        }
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        if (plant != null)
        {
            Debug.Log("Plot " + i + "," + j + " clicked, plant: " + plant.name);
            RemovePlant();
        }
        else
        {
            Debug.Log("Plot " + i + "," + j + " clicked, no plant");
        }
    }
}
