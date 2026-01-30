using UnityEngine;

public class RevealEffects : MonoBehaviour
{
    [Header("Audio")]
    public AudioClip revealSound;
    private AudioSource audioSource;

    [Header("Shimmer Settings")]
    public float shimmerDuration = 0.5f;
    public float shimmerScaleMultiplier = 1.2f;

    void Awake()
    {
        audioSource = gameObject.AddComponent<AudioSource>();
        audioSource.playOnAwake = false;
    }

    public void PlayRevealEffects()
    {
        if (revealSound != null)
            audioSource.PlayOneShot(revealSound);

        StartCoroutine(Shimmer());

        SubtleParticleShimmer ps = GetComponent<SubtleParticleShimmer>();
        if (ps != null)
            ps.PlayShimmer();
    }

    private System.Collections.IEnumerator Shimmer()
    {
        float timer = 0f;

        Vector3 originalScale = transform.localScale;
        Vector3 peakScale = originalScale * shimmerScaleMultiplier;

        while (timer < shimmerDuration)
        {
            timer += Time.deltaTime;
            float t = timer / shimmerDuration;

            transform.localScale = Vector3.Lerp(originalScale, peakScale, t);

            yield return null;
        }

        transform.localScale = originalScale;
    }
}
