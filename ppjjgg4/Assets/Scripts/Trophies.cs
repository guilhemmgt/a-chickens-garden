using DG.Tweening;
using UnityEngine;

public class Trophies : MonoBehaviour
{
    public Vector3 previewOffset = Vector3.up;
    [Header("Trophies")]
    public GameObject POULETTO;
    [TextArea(3, 5)] public string POULETTO_desc;
    public GameObject herbierTrophy;
    [TextArea(3, 5)] public string herbierDesc;
    public GameObject scoreTrophy;
    [TextArea(3, 5)] public string scoreDesc;
    public GameObject shovelTrophy;
    [TextArea(3, 5)] public string shovelDesc;
    public GameObject pickaxeTrophy;
    [TextArea(3, 5)] public string pickaxeDesc;


    [Header("Conditions")]
    public int nbClicksToReach = 10;
    public int scoreToReach = 1000;
    public int nbOfPlantsToHave = 24;
    public int nbOfRocksToRemove = 1;

    private int nbClicks = 0;

    public static Trophies Instance;


    public int NbTrophiesUnlocked()
    {
        int n = 0;
        if (POULETTO.activeSelf) n++;
        if (herbierTrophy.activeSelf) n++;
        if (scoreTrophy.activeSelf) n++;
        return n;
    }

    void Awake()
    {
        POULETTO.SetActive(false);
        herbierTrophy.SetActive(false);
        scoreTrophy.SetActive(false);
        shovelTrophy.SetActive(false);
        pickaxeTrophy.SetActive(false);

        if (Instance != null)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
    }

    private void Start()
    {
        POULETTO.GetComponent<PointerHandlerDispatcher>().OnPointerEnter +=
            () => PreviewController.Instance.ShowBubble(POULETTO.transform.position + previewOffset, POULETTO_desc);
        POULETTO.GetComponent<PointerHandlerDispatcher>().OnPointerExit +=
            () => PreviewController.Instance.HideBubble();

        herbierTrophy.GetComponent<PointerHandlerDispatcher>().OnPointerEnter +=
            () => PreviewController.Instance.ShowBubble(herbierTrophy.transform.position + previewOffset, herbierDesc);
        herbierTrophy.GetComponent<PointerHandlerDispatcher>().OnPointerExit +=
            () => PreviewController.Instance.HideBubble();

        scoreTrophy.GetComponent<PointerHandlerDispatcher>().OnPointerEnter +=
            () => PreviewController.Instance.ShowBubble(scoreTrophy.transform.position + previewOffset, scoreDesc);
        scoreTrophy.GetComponent<PointerHandlerDispatcher>().OnPointerExit +=
            () => PreviewController.Instance.HideBubble();

        shovelTrophy.GetComponent<PointerHandlerDispatcher>().OnPointerEnter +=
            () => PreviewController.Instance.ShowBubble(shovelTrophy.transform.position + previewOffset, shovelDesc);
        shovelTrophy.GetComponent<PointerHandlerDispatcher>().OnPointerExit +=
            () => PreviewController.Instance.HideBubble();

        pickaxeTrophy.GetComponent<PointerHandlerDispatcher>().OnPointerEnter +=
            () => PreviewController.Instance.ShowBubble(pickaxeTrophy.transform.position + previewOffset, pickaxeDesc);
        pickaxeTrophy.GetComponent<PointerHandlerDispatcher>().OnPointerExit +=
            () => PreviewController.Instance.HideBubble();

        Chicken.Instance.OnChickenClick += OnChickenClicked;
        Garden.OnPlantMatured += (_, _) =>
        {
            if (CheckShovel()) UnlockTrophy(shovelTrophy);
        };
        // OnRockRemoved += () => pickaxeTrophy.SetActive(true);
    }

    private void OnEnable()
    {
        GameManager.OnScoreUpdate += OnScoreUpdated;
    }

    private void OnDisable()
    {
        GameManager.OnScoreUpdate -= OnScoreUpdated;
        Chicken.Instance.OnChickenClick -= OnChickenClicked;
    }

    private void UnlockTrophy(GameObject trophy)
    {
        trophy.SetActive(true);
        AudioController.Instance.PlayTrophySuccessSound();
    }

    private void OnScoreUpdated(int score)
    {
        if (!scoreTrophy.activeSelf && score >= scoreToReach)
        {
            scoreTrophy.SetActive(true);
            AudioController.Instance.PlayTrophySuccessSound();
        }
    }

    private void OnChickenClicked()
    {
        nbClicks++;
        if (!POULETTO.activeSelf && nbClicks >= nbClicksToReach)
        {
            // Apparition avec dotween 
            UnlockTrophy(POULETTO);
            POULETTO.transform.localScale = Vector3.zero;
            POULETTO.transform.DOScale(Vector3.one, 0.5f).SetEase(Ease.OutBack);
        }
    }

    public void ShowHerbierTrophy()
    {
        UnlockTrophy(herbierTrophy);
    }

    public bool CheckShovel()
    {
        Garden garden = Garden.Instance;
        for (int i=0; i<garden.height; i++)
        {
            for (int j=0; j<garden.width; j++)
            {
                Plot plot = garden.GetPlot(i, j);
                if (plot.plant == null || !plot.plant.hasMatured) return false;
            }
        }
        return true;
    }

}
