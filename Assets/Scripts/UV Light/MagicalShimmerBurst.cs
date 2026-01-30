using UnityEngine;

public class MagicalShimmerBurst : MonoBehaviour
{
    [Header("Burst Settings")]
    public float burstDuration = 0.5f;
    public float burstScale = 1.5f;
    public float fadeSpeed = 6f;
    public Color burstColor = new Color(1f, 0.7f, 1f, 1f);

    private GameObject burstObj;
    private Material burstMat;

    void Start()
    {
        // Create sphere for shimmer burst
        burstObj = GameObject.CreatePrimitive(PrimitiveType.Sphere);
        burstObj.transform.parent = transform;
        burstObj.transform.localPosition = Vector3.zero;
        burstObj.transform.localScale = Vector3.zero;  // start small

        Destroy(burstObj.GetComponent<Collider>());

        burstMat = new Material(Shader.Find("Unlit/Color"));
        burstMat.color = new Color(burstColor.r, burstColor.g, burstColor.b, 0f); // invisible start
        burstObj.GetComponent<Renderer>().material = burstMat;

        burstObj.SetActive(false);
    }

    public void PlayBurst()
    {
        StartCoroutine(BurstRoutine());
    }

    private System.Collections.IEnumerator BurstRoutine()
    {
        burstObj.SetActive(true);

        float timer = 0f;

        // PHASE 1 — grow & fade in
        while (timer < burstDuration)
        {
            timer += Time.deltaTime;
            float t = timer / burstDuration;

            burstObj.transform.localScale = Vector3.Lerp(Vector3.zero, Vector3.one * burstScale, t);

            Color c = burstMat.color;
            c.a = Mathf.Lerp(0f, 1f, t);
            burstMat.color = c;

            yield return null;
        }

        // PHASE 2 — fade out
        while (burstMat.color.a > 0f)
        {
            Color c = burstMat.color;
            c.a -= Time.deltaTime * fadeSpeed;
            burstMat.color = c;

            yield return null;
        }

        burstObj.SetActive(false);
    }
}
