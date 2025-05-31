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

    [Header("Conditions")]
    public int nbClicksToReach = 10;
    public int scoreToReach = 1000;

    private int nbClicks = 0;

    public static Trophies Instance;

    void Awake()
    {
        POULETTO.SetActive(false);
        herbierTrophy.SetActive(false);
        scoreTrophy.SetActive(false);

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

        Chicken.Instance.OnChickenClick += OnChickenClicked;
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

    private void OnScoreUpdated(int score)
    {
        if (!scoreTrophy.activeSelf && score >= scoreToReach) scoreTrophy.SetActive(true);
    }

    private void OnChickenClicked()
    {
        nbClicks++;
        if (!POULETTO.activeSelf && nbClicks >= nbClicksToReach)
        {
            // Apparition avec dotween 
            POULETTO.SetActive(true);
            AudioController.Instance.PlayTrophySuccessSound();
            POULETTO.transform.localScale = Vector3.zero;
            POULETTO.transform.DOScale(Vector3.one, 0.5f).SetEase(Ease.OutBack);
        }
    }

    public void ShowHerbierTrophy()
    {
        herbierTrophy.SetActive(true);
    }

}
