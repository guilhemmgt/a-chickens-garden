using com.cyborgAssets.inspectorButtonPro;
using System.Collections.Generic;
using UnityEngine;

public class Herbarium : MonoBehaviour
{
    // Set des plantes déjà obtenues
    public ISet<Plant> obtainedSeeds = new HashSet<Plant>();
    public ISet<Plant> obtainedPlants = new HashSet<Plant>();

    public List<Plant> allPlants = new List<Plant>();

    private void Start()
    {
        Garden.OnPlantEnter += (Plot _, Plant p) =>
        {
            obtainedSeeds.Add(p);
        };
        Garden.OnPlantMatured += (Plot _, Plant p) =>
        {
            obtainedPlants.Add(p);
            if (HasObtainedAllPlants()) Trophies.Instance.ShowHerbierTrophy();
        };
    }

    public bool HasSeed(Plant plant)
    {
        foreach (Plant p in obtainedSeeds)
        {
            if (p.Species == plant.Species)
            {
                return true;
            }
        }

        return false;
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

    [ProButton]
    public bool HasObtainedAllPlants()
    {
        // Vérifie si toutes les plantes de allPlants sont dans obtainedPlants
        foreach (Plant plant in allPlants)
        {
            if (!HasPlant(plant))
            {
                return false; // Si une plante n'est pas obtenue, retourne false
            }
        }
        return true; // Toutes les plantes ont été obtenues
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
