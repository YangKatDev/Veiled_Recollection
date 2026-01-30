using UnityEngine;

[RequireComponent(typeof(Renderer))]
public class UVSpotlightController : MonoBehaviour
{
    [Header("Flashlight Reference")]
    public Light uvFlashlight;

    [Header("Glow Settings")]
    public Color glowColor = new Color(0.5f, 0f, 1f);
    [Range(0f, 20f)] public float baseIntensity = 10f;
    [Range(0.5f, 30f)] public float range = 10f;

    [Header("Wall Spread Settings")]
    public float minDistance = 0.5f;
    public float maxDistance = 5f;
    public float maxSpreadMultiplier = 2f;
    public float closeIntensityMultiplier = 0.6f;

    private Material mat;

    void Start()
    {
        mat = GetComponent<Renderer>().material;

        mat.SetColor("_GlowColor", glowColor);
        mat.SetFloat("_Range", range);
        mat.SetFloat("_Intensity", baseIntensity);

        // Make sure UV starts OFF
        mat.SetFloat("_UVActive", 0f);
    }

    void Update()
    {
        bool uvOn = (uvFlashlight != null && uvFlashlight.gameObject.activeInHierarchy);

        // 🔥 Tell shader UV on/off
        mat.SetFloat("_UVActive", uvOn ? 1f : 0f);

        if (!uvOn)
            return; // stop here (shader will auto output nothing)


        // -----------------------
        // 🔥 UV IS ON → Update all values
        // -----------------------

        float spreadMultiplier = 1f;
        float intensityMultiplier = 1f;

        if (Physics.Raycast(uvFlashlight.transform.position, uvFlashlight.transform.forward,
                            out RaycastHit hit, maxDistance))
        {
            float dist = Mathf.Clamp(hit.distance, minDistance, maxDistance);
            float t = 1f - Mathf.InverseLerp(minDistance, maxDistance, dist);

            spreadMultiplier = Mathf.Lerp(1f, maxSpreadMultiplier, t);
            intensityMultiplier = Mathf.Lerp(1f, closeIntensityMultiplier, t);
        }

        // Required shader inputs
        mat.SetVector("_LightPos", uvFlashlight.transform.position);
        mat.SetVector("_LightDir", uvFlashlight.transform.forward);

        // Cone angle update
        float coneAngle = uvFlashlight.spotAngle * 0.5f * Mathf.Deg2Rad * spreadMultiplier;
        mat.SetFloat("_ConeAngle", coneAngle);

        // Adjusted glow intensity
        mat.SetFloat("_Intensity", baseIntensity * intensityMultiplier);
    }
}

