using UnityEngine;

public class SubtleParticleShimmer : MonoBehaviour
{
    [Header("Particle Settings")]
    public float duration = 1.2f;
    public float particleSize = 0.06f;
    public float particleSpeed = 0.2f;
    public Color particleColor = new Color(1f, 0.8f, 1f, 0.8f);

    public void PlayShimmer()
    {
        StartCoroutine(SpawnParticles());
    }

    private System.Collections.IEnumerator SpawnParticles()
    {
        // Create object container
        GameObject fx = new GameObject("SubtleShimmerFX");
        fx.transform.position = transform.position;
        fx.transform.localScale = Vector3.one;

        ParticleSystem ps = fx.AddComponent<ParticleSystem>();
        var main = ps.main;
        main.startLifetime = duration;
        main.startSpeed = particleSpeed;
        main.startSize = particleSize;
        main.startColor = particleColor;
        main.loop = false;
        main.maxParticles = 40;
        main.playOnAwake = false;

        var emission = ps.emission;
        emission.rateOverTime = 0f;
        emission.SetBursts(new ParticleSystem.Burst[]
        {
            new ParticleSystem.Burst(0f, 18) // nice subtle sprinkle
        });

        var shape = ps.shape;
        shape.shapeType = ParticleSystemShapeType.Sphere;
        shape.radius = 0.1f;  // small radius

        var colorOverLifetime = ps.colorOverLifetime;
        colorOverLifetime.enabled = true;

        Gradient grad = new Gradient();
        grad.SetKeys(
            new GradientColorKey[] {
                new GradientColorKey(particleColor, 0f),
                new GradientColorKey(particleColor * 0.8f, 1f)
            },
            new GradientAlphaKey[] {
                new GradientAlphaKey(1f, 0f),
                new GradientAlphaKey(0f, 1f)
            }
        );
        colorOverLifetime.color = new ParticleSystem.MinMaxGradient(grad);

        var renderer = ps.GetComponent<ParticleSystemRenderer>();
        renderer.material = new Material(Shader.Find("Sprites/Default"));
        renderer.renderMode = ParticleSystemRenderMode.Billboard;

        ps.Play();

        yield return new WaitForSeconds(duration);
        GameObject.Destroy(fx);
    }
}
