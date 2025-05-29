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

    [SerializeField] private GameObject gridCellPrefab;
    private GameObject[,] gridCells;

    void Start()
    {
        InitializeGrid();
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

        gridCells = new GameObject[nbLines, nbColumns];

        for (int i = 0; i < nbLines; i++)
        {
            for (int j = 0; j < nbColumns; j++)
            {
                GameObject cell = Instantiate(gridCellPrefab, transform);

                Vector2 pos = startPos + new Vector2(j * (cellSize + cellSpacing), -i * (cellSize + cellSpacing));
                cell.transform.position = pos;

                // Ajuste la taille du sprite pour correspondre à la cellule si un sprite est présent
                SpriteRenderer sr = cell.GetComponent<SpriteRenderer>();
                if (sr != null)
                {
                    Vector2 originalSize = sr.sprite.bounds.size;
                    float scaleX = cellSize / originalSize.x;
                    float scaleY = cellSize / originalSize.y;
                    cell.transform.localScale = new Vector3(scaleX, scaleY, 1f);
                }

                cell.name = $"Cell_{i}_{j}";
                gridCells[i, j] = cell;
            }
        }
    }

    [ProButton]
    public void StoreChildrenToCellsArray()
    {
        gridCells = new GameObject[nbLines, nbColumns];

        int totalCells = nbLines * nbColumns;
        int expectedChildren = Mathf.Min(transform.childCount, totalCells);

        for (int index = 0; index < expectedChildren; index++)
        {
            int i = index / nbColumns; // ligne
            int j = index % nbColumns; // colonne

            Transform child = transform.GetChild(index);
            gridCells[i, j] = child.gameObject;
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
        if (gridCells != null)
        {
            foreach (GameObject cell in gridCells)
            {
                Destroy(cell);
            }
        }
        gridCells = null;
    }
}
