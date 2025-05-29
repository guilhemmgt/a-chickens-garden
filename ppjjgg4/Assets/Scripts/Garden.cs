using System.Collections.Generic;
using System.Linq;
using com.cyborgAssets.inspectorButtonPro;
using Unity.VisualScripting;
using UnityEngine;

public class Garden : MonoBehaviour
{
    public static Garden Instance { get; private set; }
    [SerializeField] private int width = 6;
    [SerializeField] private int height = 4;
    private Plot GetPlot(int i, int j) => transform.GetChild(width * i + j).GetComponent<Plot>();

    private void Awake()
    {
        if (Instance != null)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
        for (int i = 0; i < height; i++)
        {
            for (int j = 0; j < width; j++)
            {
                Plot plot = Plot.Instantiate(i, j);
                plot.name = "Plot " + i.ToString() + "," + j.ToString();
                plot.transform.SetParent(transform);
            }
        }
    }


    [ProButton]
    public void EndDay()
    {
        for (int i = 0; i < height; i++)
        {
            for (int j = 0; j < width; j++)
            {
                GetPlot(i, j).EndDay();
            }
        }
    }


    public List<Plot> GetNeighbours(int i, int j)
    {
        List<Plot> n = new();
        int[] i_id = { -1, -1, -1, 0, 0, 1, 1, 1 };
        int[] j_id = { -1, 0, 1, -1, 1, -1, 0, 1 };
        for (int id = 0; id < i_id.Count(); id++)
        {
            int ni = i + i_id[id];
            int nj = j + j_id[id];
            if (ni >= 0 && ni <= height - 1 && nj >= 0 && nj <= width - 1)
            {
                n.Add(GetPlot(ni, nj));
            }
        }
        return n;
    }

    public void OnEnable()
    {
        GameManager.OnDayEnded += EndDay;
    }

    public void OnDisable()
    {
        GameManager.OnDayEnded -= EndDay;
    }
}
