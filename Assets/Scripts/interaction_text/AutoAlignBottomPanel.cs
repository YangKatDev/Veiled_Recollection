using UnityEngine;
using UnityEngine.UI;

[ExecuteAlways]
public class AutoAlignBottomPanel : MonoBehaviour
{
    [Header("Panel Settings")]
    public float panelHeight = 300f;   // height of your dialogue box

    private RectTransform rect;

    void OnEnable()
    {
        AlignPanel();
    }

    void OnValidate()
    {
        AlignPanel();
    }

    void AlignPanel()
    {
        if (rect == null)
            rect = GetComponent<RectTransform>();

        // --- FORCE SIZE MODE ---
        rect.sizeDelta = new Vector2(0, panelHeight);

        // --- ANCHOR TO BOTTOM ---
        rect.anchorMin = new Vector2(0, 0); // bottom left
        rect.anchorMax = new Vector2(1, 0); // bottom right
        rect.pivot = new Vector2(0.5f, 0);  // center bottom

        // --- LOCK BOTTOM & HEIGHT ---
        rect.offsetMin = new Vector2(0, 0);
        rect.offsetMax = new Vector2(0, panelHeight);

        // --- DO NOT LET CHILDREN RESIZE PANEL ---
        LayoutRebuilder.ForceRebuildLayoutImmediate(rect);

        // Reset scale & rotation
        rect.localScale = Vector3.one;
        rect.localRotation = Quaternion.identity;
    }
}
