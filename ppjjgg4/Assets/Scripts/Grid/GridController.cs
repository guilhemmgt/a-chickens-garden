using com.cyborgAssets.inspectorButtonPro;
using UnityEngine;

public class GridController : MonoBehaviour
{
    [Header("Grid Settings")]
    [SerializeField] private int nbLines = 8;
    [SerializeField] private int nbColumns = 5;
    [SerializeField] private float widthPercentage = 0.8f;
    [SerializeField] private float heightPercentage = 0.8f;
    [SerializeField] private float cellSpacing = 0.05f; // Espacement entre les cellules en unités monde

    [SerializeField] private Sprite defaultCellSprite; // Sprite par défaut pour les cellules

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

        float gridWidth = worldWidth * widthPercentage;
        float gridHeight = worldHeight * heightPercentage;

        float totalSpacingX = (nbColumns - 1) * cellSpacing;
        float totalSpacingY = (nbLines - 1) * cellSpacing;

        float cellWidth = (gridWidth - totalSpacingX) / nbColumns;
        float cellHeight = (gridHeight - totalSpacingY) / nbLines;
        float cellSize = Mathf.Min(cellWidth, cellHeight);

        // Origine centrée, première cellule en haut à gauche
        Vector2 startPos = new Vector2(
            -cellSize * nbColumns / 2f - cellSpacing * (nbColumns - 1) / 2f + cellSize / 2f,
            cellSize * nbLines / 2f + cellSpacing * (nbLines - 1) / 2f - cellSize / 2f
        );

        plots = new Plot[nbLines, nbColumns];

        for (int i = 0; i < nbLines; i++)
        {
            for (int j = 0; j < nbColumns; j++)
            {
                GameObject cell = Instantiate(gridCellPrefab, transform);

                Plot plot = cell.AddComponent<Plot>();
                plot.i = i;
                plot.j = j;

                Vector2 pos = startPos + new Vector2(j * (cellSize + cellSpacing), -i * (cellSize + cellSpacing));
                cell.transform.position = pos;

                // Ajuste la taille du sprite pour correspondre à la cellule si un sprite est présent
                if (cell.TryGetComponent<SpriteRenderer>(out var sr))
                {
                    sr.sprite = defaultCellSprite;
                    Vector2 originalSize = sr.sprite.bounds.size;
                    float scaleX = cellSize / originalSize.x;
                    float scaleY = cellSize / originalSize.y;
                    cell.transform.localScale = new Vector3(scaleX, scaleY, 1f);
                }

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
