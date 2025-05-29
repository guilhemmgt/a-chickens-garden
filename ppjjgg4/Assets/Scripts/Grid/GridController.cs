using com.cyborgAssets.inspectorButtonPro;
using UnityEngine;

public class GridController : MonoBehaviour
{
    [Header("Grid Settings")]
    [SerializeField] private int nbLines = 8;
    [SerializeField] private int nbColumns = 5;
    [SerializeField] private float widthPercentage = 0.8f;
    [SerializeField] private float heightPercentage = 0.8f;

    [SerializeField] private GameObject gridCellPrefab;
    private GameObject[,] gridCells;

    void Start()
    {
        InitializeGrid();
    }

    private void InitializeGrid()
    {
        Camera cam = Camera.main;

        // Dimensions visibles en unités monde
        float worldHeight = cam.orthographicSize * 2f;
        float worldWidth = worldHeight * cam.aspect;

        float gridWidth = worldWidth * widthPercentage;
        float gridHeight = worldHeight * heightPercentage;

        float cellWidth = gridWidth / nbColumns;
        float cellHeight = gridHeight / nbLines;
        float cellSize = Mathf.Min(cellWidth, cellHeight); // carré

        // Origine : coin en haut à gauche, centrée dans le monde
        Vector2 startPos = new Vector2(
            -cellSize * nbColumns / 2f + cellSize / 2f,
            cellSize * nbLines / 2f - cellSize / 2f
        );

        gridCells = new GameObject[nbLines, nbColumns];

        for (int i = 0; i < nbLines; i++)
        {
            for (int j = 0; j < nbColumns; j++)
            {
                GameObject cell = Instantiate(gridCellPrefab, transform);

                Vector2 pos = startPos + new Vector2(j * cellSize, -i * cellSize);
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
