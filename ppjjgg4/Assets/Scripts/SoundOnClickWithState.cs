using UnityEngine;
using UnityEngine.EventSystems;

public class SoundOnClickWithState : MonoBehaviour, IPointerClickHandler
{
    [SerializeField] private AudioClip normalSound;
    [SerializeField] private AudioClip soundWhenShovel;

    [Range(0f, 1f)]
    [SerializeField] private float volume = 0.5f; // Default volume

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        if (soundWhenShovel == null)
        {
            soundWhenShovel = normalSound;
        }
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        if (GameManager.GameState == GameState.Digging)
        {
            AudioController.Instance.PlaySFXInParallel(soundWhenShovel, volume);
        }
        else
        {
            AudioController.Instance.PlaySFXInParallel(normalSound, volume);
        }
    }


}
