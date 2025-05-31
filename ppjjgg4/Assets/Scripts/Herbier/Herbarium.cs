using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Herbarium : MonoBehaviour
{
    // Set des plantes déjà obtenues
    public List<Plant> obtainedPlants = new List<Plant>();

    public List<Plant> allPlants = new List<Plant>();

    private void Start()
    {
        Garden.OnPlantMatured += TryAddPlant;
    }

    public void TryAddPlant(Plot plot, Plant plant)
    {
        bool isAlreadyObtained = false;
        foreach (Plant p in allPlants)
        {
            if (p.Species == plant.Species)
            {
                isAlreadyObtained = true;
                return;
            }
        }

        if (!isAlreadyObtained)
        {
            allPlants.Add(plant);
            Debug.Log($"Plante {plant.Species} ajoutée à l'herbier.");
        }
        else
        {
            Debug.Log($"Plante {plant.Species} déjà dans l'herbier, pas besoin de l'ajouter à nouveau.");
        }
    }

    public bool HasPlant(Plant plant)
    {
        foreach (Plant p in obtainedPlants)
        {
            if (p.Species == plant.Species)
            {
                return true;
            }
        }

        return false;
    }

    public List<Plant> GetObtainedPlants()
    {
        // Retourne une liste des plantes obtenues
        return new List<Plant>(obtainedPlants);
    }

    public void ClearObtainedPlants()
    {
        // Vide le set des plantes obtenues
        obtainedPlants.Clear();
        Debug.Log("Herbarium vidé.");
    }
 }
