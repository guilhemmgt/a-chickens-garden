using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using com.cyborgAssets.inspectorButtonPro;
using Unity.VisualScripting;
using Unity.XR.GoogleVr;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SocialPlatforms.Impl;

public class Plot : MonoBehaviour, IPointerClickHandler, IPointerEnterHandler, IPointerExitHandler
{
    public Action<Plant> OnPlantEnter;
    public Action<Plant> OnPlantMatured;
    public Action<Plant> OnPlantExit;
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

    [SerializeField] private Sprite defaultSprite = null;

    [field: SerializeField] public List<Effect> effects { get; private set; }
    [SerializeField] private Type type = Type.Soil;
    private SpriteRenderer sr => transform.GetChild(0).GetComponent<SpriteRenderer>();
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
        Garden.OnPlantEnter?.Invoke(this, this.plant);
        PreviewController.Instance.UpdateInfo(GetInfoPlot());
    }

    [ProButton]
    public void RemovePlant()
    {
        if (plant != null)
        {
            plant.OnRemoved();
            OnPlantExit?.Invoke(plant);
            OnPlantExit = null;
            Garden.OnPlantExit?.Invoke(this, plant);
            Destroy(plant);
            plant = null;
            GameManager.Instance.UpdateScore();
            sr.sprite = defaultSprite; // Reset sprite to default
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

    public int GetPlotScore()
    {
        int score = 0;

        if (plant != null && plant.hasMatured)
        {
            // Score de la plante
            score += plant.GetScore();

            if (effects.Where(e => e.flag == Effect.Flag.EffectCancelled).Count() == 0)
            {
                // Score des buffs
                foreach (Effect effect in effects)
                {
                    score += Effect.GetFlagScore(effect.flag);
                }
            }
        }

        return score;
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        if (GameManager.GameState == GameState.Digging)
        {
            if (plant != null)
            {
                Debug.Log("Plot " + i + "," + j + " clicked, removing plant");
                RemovePlant();
                PreviewController.Instance.UpdateInfo(GetInfoPlot());
                AudioController.Instance.PlayShovelSound();
            }
        }

        if (GameManager.GameState != GameState.Planting)
        {
            return; // Can only plant in Planting state
        }

        Plant currentPlant = ChoiceHandler.Instance.GetCurrentPlant();

        if (plant == null)
        {
            // Want to plant a new plant
            if (ChoiceHandler.Instance.HasCurrentPlant())
            {
                if (ChoiceHandler.Instance.TryPlantCurrent(this))
                {
                    //Debug.Log("Plot " + i + "," + j + " clicked, planted " + currentPlant.name);
                }
                else
                {
                    //Debug.Log("Plot " + i + "," + j + " clicked, failed to plant current plant");
                }
            }
            else
            {
                //Debug.Log("Plot " + i + "," + j + " clicked, no current plant selected");
            }
        }
        else
        {
            //Debug.Log("Plot " + i + "," + j + " clicked, no plant");
        }
    }

    #region Inspector Methods
    public float timeBeforeShowBubble = 1.5f; // Time in seconds before showing the bubble

    private IEnumerator currentCoroutine = null;

    public void OnPointerEnter(PointerEventData eventData)
    {
        //Debug.Log("Plot " + i + "," + j + " hovered, showing bubble");
        if (GameManager.GameState == GameState.Menu)
        {
            return; // Do not show bubble in menu state
        }
        currentCoroutine = ShowBubbleAfterDelay();
        StartCoroutine(currentCoroutine); // Start the coroutine to show the bubble after a delay
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        PreviewController.Instance.HideBubble();
        if (currentCoroutine != null)
            StopCoroutine(currentCoroutine); // Stop the coroutine if the pointer exits before the delay
        currentCoroutine = null;
    }

    public IEnumerator ShowBubbleAfterDelay()
    {
        yield return new WaitForSeconds(timeBeforeShowBubble);
        PreviewController.Instance.ShowBubble(transform.position, GetInfoPlot());
    }

    public string GetInfoPlot()
    {
        string info = $"Plot ({i}, {j})\n";
        //info += $"Type: {type}\n";
        if (plant != null)
        {
            info += $"Plant: {plant.Species}\n";
            info += $"Day passed: {plant.day}\n";
            //info += $"Has matured: {plant.hasMatured}\n";
            info += $"Plant Score: {plant.GetScore()}\n";
            info += $"Total score: {GetPlotScore()} \n";
        }
        else
        {
            info += "No plant\n";
        }
        return info;
    }

    #endregion
}
