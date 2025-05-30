using com.cyborgAssets.inspectorButtonPro;
using UnityEngine;

public class GridController : MonoBehaviour
{
    [Header("Grid Settings")]
    [SerializeField] private int nbLines = 8;
    [SerializeField] private int nbColumns = 5;
    [SerializeField] private Vector2 startPos = Vector2.zero;
    [SerializeField] private float cellSpacing = 0.05f; // Espacement entre les cellules en unités monde

    [SerializeField] private GameObject gridCellPrefab;
    private Plot[,] plots;

    void Start()
    {
        InitializeGrid();
        LoadGarden();
    }

    private void LoadGarden()
    {
        Garden garden = Garden.Instance;
        if (garden == null)
        {
            Debug.LogError("Garden instance not found. Please ensure Garden is initialized before GridController.");
            return;
        }
        garden.SetGarden(plots);
    }

    private void InitializeGrid()
    {
        Camera cam = Camera.main;

        float worldHeight = cam.orthographicSize * 2f;
        float worldWidth = worldHeight * cam.aspect;

        float totalSpacingX = (nbColumns - 1) * cellSpacing;
        float totalSpacingY = (nbLines - 1) * cellSpacing;

        plots = new Plot[nbLines, nbColumns];

        for (int i = 0; i < nbLines; i++)
        {
            for (int j = 0; j < nbColumns; j++)
            {
                GameObject cell = Instantiate(gridCellPrefab, transform);

                Plot plot = cell.AddComponent<Plot>();
                plot.i = i;
                plot.j = j;

                Vector2 pos = startPos + new Vector2(j * cellSpacing, -i * cellSpacing);
                cell.transform.position = pos;

                cell.name = $"Cell_{i}_{j}";

                plots[i, j] = plot;
            }
        }
    }

    [ProButton]
    public void StoreChildrenToCellsArray()
    {
        plots = new Plot[nbLines, nbColumns];

        int totalCells = nbLines * nbColumns;
        int expectedChildren = Mathf.Min(transform.childCount, totalCells);

        for (int index = 0; index < expectedChildren; index++)
        {
            int i = index / nbColumns; // ligne
            int j = index % nbColumns; // colonne

            Transform child = transform.GetChild(index);
            plots[i, j] = child.gameObject.GetComponent<Plot>();
        }
    }




    [ProButton]
    public void ResetGrid()
    {
        CleanGrid();
        InitializeGrid();
    }

    public void CleanGrid()
    {
        if (plots != null)
        {
            foreach (Plot cell in plots)
            {
                Destroy(cell.gameObject);
            }
        }
        plots = null;
    }
}
