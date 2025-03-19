using TMPro;
using UnityEngine;

public class PopupUIController : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI popupText;

    /// <summary>
    /// Initializes the popup with a text message.
    /// </summary>
    /// <param name="message">Text to display</param>
    public void Initialize(string message)
    {
        if (popupText != null)
        {
            popupText.text = message;
        }
        else
        {
            Debug.LogWarning("[PopupUIController] TextMeshProUGUI is missing!");
        }
    }
}
