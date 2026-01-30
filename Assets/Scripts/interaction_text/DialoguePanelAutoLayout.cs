using UnityEngine;
using TMPro;

[ExecuteAlways]   // Runs even in Editor
public class DialoguePanelAutoLayout : MonoBehaviour
{
    [Header("Layout Settings")]
    public float panelHeight = 300f;
    public float sidePadding = 40f;
    public float topPadding = 20f;
    public float spacing = 20f;

    [Header("Assigned Automatically")]
    public RectTransform panelRect;
    public TMP_Text titleText;
    public TMP_Text bodyText;

    void OnEnable()
    {
        SetupLayout();
    }

    void OnValidate()
    {
        SetupLayout();
    }

    void SetupLayout()
    {
        if (panelRect == null)
            panelRect = GetComponent<RectTransform>();

        // --- PANEL POSITION ---
        panelRect.anchorMin = new Vector2(0, 0);
        panelRect.anchorMax = new Vector2(1, 0);
        panelRect.pivot = new Vector2(0.5f, 0);
        panelRect.offsetMin = new Vector2(0, 0);                  // bottom
        panelRect.offsetMax = new Vector2(0, panelHeight);        // height

        // --- TITLE TEXT ---
        if (titleText != null)
        {
            RectTransform t = titleText.rectTransform;
            t.anchorMin = new Vector2(0, 1);
            t.anchorMax = new Vector2(1, 1);
            t.pivot = new Vector2(0.5f, 1);
            t.offsetMin = new Vector2(sidePadding, -topPadding - 50f);
            t.offsetMax = new Vector2(-sidePadding, -topPadding);
            titleText.alignment = TextAlignmentOptions.Center;
        }

        // --- BODY TEXT ---
        if (bodyText != null)
        {
            RectTransform b = bodyText.rectTransform;
            b.anchorMin = new Vector2(0, 0);
            b.anchorMax = new Vector2(1, 1);
            b.pivot = new Vector2(0.5f, 1);
            b.offsetMin = new Vector2(sidePadding, spacing);
            b.offsetMax = new Vector2(-sidePadding, -topPadding - 80f);
            bodyText.alignment = TextAlignmentOptions.TopLeft;
        }
    }
}
