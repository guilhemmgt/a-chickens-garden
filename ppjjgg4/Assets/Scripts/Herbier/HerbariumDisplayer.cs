using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class HerbariumDisplayer : MonoBehaviour
{
    private Herbarium herbarium;

    [SerializeField] private GameObject cellPrefab; // contains in child one image and one text

    private void Start()
    {
        herbarium = GetComponent<Herbarium>();
        if (herbarium == null)
        {
            Debug.LogError("Herbarium component not found on HerbariumController.");
        }

        DisplayObtainedPlants();

        UI_Controller.Instance.OnHerbierShow += DisplayObtainedPlants;
    }

    public void DisplayObtainedPlants()
    {
        // Destroy all existing cells in the display
        foreach (Transform child in transform)
        {
            Destroy(child.gameObject);
        }

        foreach (Plant plant in herbarium.allPlants)
        {
            // Crée une cellule pour la plante obtenue
            GameObject cell = Instantiate(cellPrefab, transform);
            cell.name = plant.Species;

            // Assigne l'image de la plante
            Image imagePrefab = cell.GetComponentInChildren<Image>();
            imagePrefab.sprite = plant.GetMatureSprite();

            Debug.Log("Displaying plant: " + plant.Species);
			TextMeshProUGUI desc = cell.transform.GetChild (1).GetComponent<TextMeshProUGUI> ();
			TextMeshProUGUI name = cell.transform.GetChild (2).GetComponent<TextMeshProUGUI> ();
			if (herbarium.HasPlant(plant))
            {
                imagePrefab.color = Color.white;
                name.text = plant.name;
                // Assigne le texte
                if (plant.skill != null)
                {
					// Si la plante a une description, l'affiche
					desc.text = "Score: " + plant.score + "\n"
                        + plant.skill.Name + ":\n" + plant.skill.Description;
                }
                else
                {
					// Si la plante n'a pas de description, affiche juste le score
					desc.text = "A perfectly normal flower.\n Score: " + plant.score;
                }
            }
            else
            {
                imagePrefab.color = Color.black;

                // Assigne le 
                name.text = "???";
                desc.text = "Yet to be discovered...";

			}
        }
    }




}
