using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Video;

[Serializable]
public class InfoTuto
{
    [TextArea(1, 3)]
    public string infotext;
    public VideoClip video;
}

public class TutoController : MonoBehaviour
{
    [Header("Tuto Content")]
    [SerializeField] private List<InfoTuto> infos = new List<InfoTuto>();

    private int currentIndex = 0;
    private int maxIndex = 0;

    [Header("UI References")]
    [SerializeField] private Button previousButton;
    [SerializeField] private Button nextButton;

    [Header("Video Display")]
    [SerializeField] private RawImage videoDisplay;       // RawImage for video
    [SerializeField] private VideoPlayer videoPlayer;     // VideoPlayer component

    [SerializeField] private TextMeshProUGUI text;

    void Start()
    {
        // Disable auto play
        videoPlayer.playOnAwake = false;
        videoPlayer.isLooping = true;

        maxIndex = infos.Count - 1;
        UpdateInfo();

        // Set initial button states
        previousButton.interactable = currentIndex > 0;
        nextButton.interactable = currentIndex < maxIndex;

        previousButton.onClick.AddListener(PreviousInfo);
        nextButton.onClick.AddListener(NextInfo);
    }

    public void NextInfo()
    {
        if (currentIndex < maxIndex)
        {
            currentIndex++;
            UpdateInfo();
        }
    }

    public void PreviousInfo()
    {
        if (currentIndex > 0)
        {
            currentIndex--;
            UpdateInfo();
        }
    }

    public void Replay()
    {
        if (videoPlayer.clip != null)
        {
            videoPlayer.Stop();
            videoPlayer.Play();
        }
    }

    private void UpdateInfo()
    {
        InfoTuto currentInfo = infos[currentIndex];
        text.text = currentInfo.infotext;

        if (currentInfo.video != null)
        {
            videoPlayer.Stop();
            videoPlayer.clip = currentInfo.video;
            videoPlayer.Play();
        }

        // Optional: Disable buttons at edges
        previousButton.interactable = currentIndex > 0;
        nextButton.interactable = currentIndex < maxIndex;
    }
}
