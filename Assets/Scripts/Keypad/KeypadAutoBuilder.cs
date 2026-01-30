using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.Events;

[ExecuteAlways]
public class KeypadAutoBuilder : MonoBehaviour
{
    [Header("Setup")]
    public Transform keypadPanel;
    public KeypadManager keypadManager;
    public Font legacyFont;

    [Header("Layout Settings")]
    public Vector2 cellSize = new Vector2(100, 100);
    public Vector2 spacing = new Vector2(10, 10);
    public int columns = 3;
    public Vector2 startPosition = new Vector2(-120, 220);

    private readonly string[] labels =
    {
        "1", "2", "3",
        "4", "5", "6",
        "7", "8", "9",
        "Clear", "0", "Enter",
        "Exit"   // ← NEW EXIT BUTTON
    };

    [ContextMenu("Rebuild Keypad")]
    public void RebuildKeypad()
    {
        if (!keypadPanel)
        {
            Debug.LogError("Assign keypadPanel!");
            return;
        }

        // Destroy old buttons
        for (int i = keypadPanel.childCount - 1; i >= 0; i--)
        {
            DestroyImmediate(keypadPanel.GetChild(i).gameObject);
        }

        // Loop through labels, create buttons
        for (int i = 0; i < labels.Length; i++)
        {
            GameObject btnObj = new GameObject("Button_" + labels[i], typeof(RectTransform), typeof(Image), typeof(Button));
            btnObj.transform.SetParent(keypadPanel, false);

            RectTransform rect = btnObj.GetComponent<RectTransform>();
            int row = i / columns;
            int col = i % columns;

            float x = startPosition.x + (cellSize.x + spacing.x) * col;
            float y = startPosition.y - (cellSize.y + spacing.y) * row;
            rect.sizeDelta = cellSize;
            rect.anchoredPosition = new Vector2(x, y);

            Image img = btnObj.GetComponent<Image>();
            img.color = Color.gray;

            // Add label text
            GameObject textObj = new GameObject("Label", typeof(RectTransform));
            textObj.transform.SetParent(btnObj.transform, false);

            RectTransform tRect = textObj.GetComponent<RectTransform>();
            tRect.anchorMin = Vector2.zero;
            tRect.anchorMax = Vector2.one;
            tRect.offsetMin = Vector2.zero;
            tRect.offsetMax = Vector2.zero;

            TMP_Text tmp = textObj.AddComponent<TextMeshProUGUI>();
            tmp.alignment = TextAlignmentOptions.Center;
            tmp.fontSize = 36;
            tmp.text = labels[i];

            // Color special buttons
            if (labels[i] == "Clear") img.color = new Color(1f, 0.3f, 0.3f);
            if (labels[i] == "Enter") img.color = new Color(0.3f, 1f, 0.3f);
            if (labels[i] == "Exit") img.color = new Color(0.3f, 0.3f, 1f);   // Blue for exit

            // Hook up button behavior
            if (keypadManager != null)
            {
                Button b = btnObj.GetComponent<Button>();
                string label = labels[i];

                if (label == "Clear")
                    b.onClick.AddListener(() => keypadManager.Clear());
                else if (label == "Enter")
                    b.onClick.AddListener(() => keypadManager.Submit());
                else if (label == "Exit")
                    b.onClick.AddListener(() => keypadManager.HideKeypad());   // EXIT HERE
                else
                    b.onClick.AddListener(() => keypadManager.PressKey(label));
            }
        }

        Debug.Log("✅ Keypad rebuilt with EXIT button.");
    }
}
