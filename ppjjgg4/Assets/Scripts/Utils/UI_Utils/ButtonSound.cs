using UnityEngine;
using UnityEngine.UI;

public class ButtonSound : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        // Get the Button component attached to this GameObject
        Button button = GetComponent<Button>();
        button.onClick.AddListener(() => AudioController.Instance.PlayButtonClickSound());
    }
}
