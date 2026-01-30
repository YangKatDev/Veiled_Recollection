using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class SceneFaderBehaviour : MonoBehaviour
{
    public float fadeDuration = 1f;
    public FadeType currentFadeType;

    private int _fadeAmount = Shader.PropertyToID("_FadeAmount");

    private int _useShutters = Shader.PropertyToID("_UseShutters");
    private int _useRadialWipe = Shader.PropertyToID("_UseRadialWipe");
    private int _usePlainBlack = Shader.PropertyToID("_UsePlainBlack");
    private int _useGoop = Shader.PropertyToID("_UseGoop");

    private int? lastEffect;

    private Image image;
    private Material material;

    public enum FadeType
    {
        Shutters,
        RadialWipe,
        PlainBlack,
        Goop
    }

    private void Awake()
    {
        image = GetComponent<Image>();

        Material mat = image.material;
        image.material = new Material(mat);
        material = image.material;

        lastEffect = _useShutters;
    }

    private void Start()
    {
        FadeIn(currentFadeType);
    }

    public void FadeOut(FadeType fadeType)
    {
        ChangeFadeEffect(fadeType);
        StartFadeOut();
    }

    public void FadeIn(FadeType fadeType)
    {
        ChangeFadeEffect(fadeType);
        StartFadeIn();
    }

    private void ChangeFadeEffect(FadeType fadeType)
    {
        if (lastEffect.HasValue)
        {
            material.SetFloat(lastEffect.Value, 0f);
        }

        switch (fadeType)
        {
            case FadeType.Shutters:

                SwitchEffect(_useShutters);
                break;

            case FadeType.RadialWipe:

                SwitchEffect(_useRadialWipe);
                break;

            case FadeType.PlainBlack:

                SwitchEffect(_usePlainBlack);
                break;

            case FadeType.Goop:

                SwitchEffect(_useGoop);
                break;
        }
    }

    private void SwitchEffect(int effectToTurnOn)
    {
        material.SetFloat(effectToTurnOn, 1f);

        lastEffect = effectToTurnOn;
    }

    private void StartFadeOut()
    {
        material.SetFloat(_fadeAmount, 0f);

        StartCoroutine(HandleFade(1f, 0f));
    }

    private void StartFadeIn()
    {
        material.SetFloat(_fadeAmount, 1f);

        StartCoroutine(HandleFade(0f, 1f));
    }

    private IEnumerator HandleFade(float targetAmount, float startAmount)
    {
        float elapsedTime = 0f;
        while (elapsedTime < fadeDuration)
        {
            elapsedTime += Time.deltaTime;

            float lerpedAmount = Mathf.Lerp(startAmount, targetAmount, (elapsedTime / fadeDuration));
            material.SetFloat(_fadeAmount, lerpedAmount);

            yield return null;
        }

        material.SetFloat(_fadeAmount, targetAmount);
    }

    private IEnumerator FadeOutAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay);
        FadeOut(currentFadeType);
    }
}
