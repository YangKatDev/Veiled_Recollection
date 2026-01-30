using UnityEngine;

[RequireComponent(typeof(Renderer))]
public class AlwaysOutline : MonoBehaviour
{
    public Color color = Color.white;
    [Range(0.01f, 0.1f)] public float width = 0.03f;

    private Material outlineMat;
    private Renderer rend;
    private Material[] originalMats;
    private bool outlined = false;

    void Start()
    {
        rend = GetComponent<Renderer>();
        originalMats = rend.materials;

        Shader s = Shader.Find("Custom/SimpleAlwaysOutline");
        if (!s)
        {
            Debug.LogError("Missing SimpleAlwaysOutline shader!");
            return;
        }

        outlineMat = new Material(s);
        outlineMat.SetColor("_OutlineColor", color);
        outlineMat.SetFloat("_OutlineWidth", width);
    }

    // ✅ Call this when object becomes interactable
    public void EnableOutline()
    {
        if (outlined) return;

        var mats = new Material[originalMats.Length + 1];
        for (int i = 0; i < originalMats.Length; i++)
            mats[i] = originalMats[i];
        mats[mats.Length - 1] = outlineMat;
        rend.materials = mats;

        outlined = true;
    }

    // ✅ Call this when it's *not* interactable anymore
    public void DisableOutline()
    {
        if (!outlined) return;

        rend.materials = originalMats;
        outlined = false;
    }
}
