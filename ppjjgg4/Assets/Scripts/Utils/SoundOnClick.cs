using UnityEngine;
using UnityEngine.EventSystems;

public class SoundOnClick : MonoBehaviour, IPointerClickHandler
{
    [SerializeField] private AudioClip sound;

    [Range(0f, 1f)]
    [SerializeField] private float volume = 0.5f;

    private void Start()
    {
        // Check that a collider is attached to the GameObject
        if (!TryGetComponent<Collider2D>(out _))
        {
            Debug.LogWarning("SoundOnClick requires a Collider2D component to detect clicks.", this);
        }
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        // Play the sound when the GameObject is clicked
        if (sound != null)
        {
            AudioController.Instance.PlaySFXInParallel(sound, volume);
        }
        else
        {
            Debug.LogWarning("No sound assigned to SoundOnClick.", this);
        }

    }
}
