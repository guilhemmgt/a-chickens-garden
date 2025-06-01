using UnityEngine;
using UnityEngine.UI;

/**
 * ChoiceHandler is a singleton class that manages the current plant selection in the game.
 * It allows setting and checking the current plant, ensuring only one instance exists.
 */
public class ChoiceHandler : MonoBehaviour
{
    [SerializeField] private Plant currentPlant;

    public static ChoiceHandler Instance { get; private set; }

    private void Awake()
    {
        if (Instance != null)
        {
            Debug.LogWarning("Multiple instances of ChoiceHandler detected. Destroying duplicate.");
            Destroy(gameObject);
            return;
        }
        Instance = this;

        if (currentPlant != null)
        {
			SeedPanelView.Instance.SetImage (currentPlant.Sprite);
		}
    }

    public bool HasCurrentPlant()
    {
        return currentPlant != null;
    }

    public Plant GetCurrentPlant()
    {
        if (currentPlant == null)
        {
            Debug.LogWarning("No current plant selected.");
            return null;
        }
        return currentPlant;
    }

    public void SetCurrentPlant(Plant plant)
    {
		currentPlant = plant;
        if (currentPlant != null) {
            Debug.LogWarning ("Replacing current plant: " + currentPlant.name + " with " + plant.name);
            SeedPanelView.Instance.SetImage (currentPlant.Sprite); // Update UI image
        } else {
            SeedPanelView.Instance.ClearImage ();
		}
    }

    public bool TryPlantCurrent(Plot plot)
    {
        if (!HasCurrentPlant())
        {
            Debug.LogWarning("No current plant selected.");
            return false;
        }
        if (plot.plant != null || plot.type == Plot.Type.Rock)
        {
            Debug.LogWarning("Plot is already occupied.");
            return false;
        }

        plot.AddPlant(currentPlant);
        AudioController.Instance.PlayPlantingSound(); // Play sound effect when setting a new plant

        currentPlant = null; // Clear current plant after planting
        SeedPanelView.Instance.ClearImage (); // Clear UI image
		return true;
    }
}
