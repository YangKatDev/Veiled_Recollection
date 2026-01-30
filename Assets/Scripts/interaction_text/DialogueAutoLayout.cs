using UnityEngine;
using TMPro;

[ExecuteAlways]
public class DialogueAutoLayout : MonoBehaviour
{
    [Header("Panel Layout")]
    public float panelHeight = 280f;
    public float sidePadding = 40f;
    public float titleHeight = 70f;
    public float spacing = 20f;

    [Header("Text Settings")]
    public float titleFontSize = 42f;
    public float bodyFontSize = 30f;

    [Header("UI References")]
    public TMP_Text titleText;
    public TMP_Text bodyText;

    private RectTransform panel;

    void OnEnable() => ApplyLayout();
    void OnValidate() => ApplyLayout();

    void ApplyLayout()
    {
        if (!panel) panel = GetComponent<RectTransform>();

        // ============================
        // PANEL (BOTTOM FIXED)
        // ============================
        panel.anchorMin = new Vector2(0, 0);
        panel.anchorMax = new Vector2(1, 0);
        panel.pivot = new Vector2(0.5f, 0);

        panel.offsetMin = new Vector2(0, 0);
        panel.offsetMax = new Vector2(0, panelHeight);

        panel.localScale = Vector3.one;
        panel.localRotation = Quaternion.identity;


        // ============================
        // TITLE TEXT (TOP OF PANEL)
        // ============================
        if (titleText)
        {
            RectTransform t = titleText.rectTransform;

            t.anchorMin = new Vector2(0, 1);
            t.anchorMax = new Vector2(1, 1);
            t.pivot = new Vector2(0.5f, 1);

            t.offsetMin = new Vector2(sidePadding, -titleHeight);
            t.offsetMax = new Vector2(-sidePadding, 0);

            titleText.fontSize = titleFontSize;
            titleText.alignment = TextAlignmentOptions.Center;
            titleText.enableAutoSizing = false;
            titleText.textWrappingMode = TextWrappingModes.Normal;
        }


        // ============================
        // BODY TEXT (FILL BELOW TITLE)
        // ============================
        if (bodyText)
        {
            RectTransform b = bodyText.rectTransform;

            b.anchorMin = new Vector2(0, 0);
            b.anchorMax = new Vector2(1, 1);
            b.pivot = new Vector2(0.5f, 1);

            b.offsetMin = new Vector2(sidePadding, spacing);
            b.offsetMax = new Vector2(-sidePadding, -titleHeight);

            bodyText.fontSize = bodyFontSize;
            bodyText.alignment = TextAlignmentOptions.TopLeft;
            bodyText.enableAutoSizing = false;
            bodyText.textWrappingMode = TextWrappingModes.Normal;
        }
    }
}
