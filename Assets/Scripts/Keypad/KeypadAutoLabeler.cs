using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class KeypadAutoLabeler : MonoBehaviour
{
    [Header("Button Panel (parent containing all keypad buttons)")]
    public Transform keypadPanel;

    // Ordered labels for keypad buttons
    private readonly string[] buttonLabels =
    {
        "1", "2", "3",
        "4", "5", "6",
        "7", "8", "9",
        "Clear", "0", "Enter",
        "Exit" // <-- NEW EXIT BUTTON LABEL
    };

    [ContextMenu("Auto Label Buttons")]
    public void AutoLabelButtons()
    {
        if (keypadPanel == null)
        {
            Debug.LogWarning("Please assign the keypad panel in the inspector!");
            return;
        }

        Button[] buttons = keypadPanel.GetComponentsInChildren<Button>(true);

        if (buttons.Length == 0)
        {
            Debug.LogWarning("No buttons found under the assigned panel!");
            return;
        }

        int count = Mathf.Min(buttons.Length, buttonLabels.Length);

        for (int i = 0; i < count; i++)
        {
            var btn = buttons[i];
            string label = buttonLabels[i];

            // Find TMP or legacy UI text component
            TMP_Text tmp = btn.GetComponentInChildren<TMP_Text>(true);
            if (tmp)
            {
                tmp.text = label;
            }
            else
            {
                Text legacy = btn.GetComponentInChildren<Text>(true);
                if (legacy)
                    legacy.text = label;
            }

            // Rename the button GameObject
            btn.name = "Button_" + label;

            Debug.Log($"Labeled {btn.name} as \"{label}\"");
        }
    }
}
