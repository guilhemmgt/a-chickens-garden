using UnityEngine;
using UnityEngine.UI;

/**
 * ButtonSound is a MonoBehaviour that adds a click sound effect to a UI Button.
 * It listens for button clicks and plays a sound effect when the button is clicked.
 * This script should be attached to a GameObject that has a Button component.
 */
public class ButtonSound : MonoBehaviour
{
    void Start()
    {
        if (TryGetComponent<Button>(out var button))
        {
            button.onClick.AddListener(() => AudioController.Instance.PlayButtonClickSound());
        }
        else
        {
            Debug.LogWarning("ButtonSound script must be attached to a GameObject with a Button component.");
        }

    }
}
