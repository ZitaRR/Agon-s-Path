using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

public static class PostProcessing 
{
    private static Volume volume;
    private static VolumeProfile profile;
    private static ColorAdjustments colours;
    private static ChromaticAberration chromatic;

    public static void Initialize(Volume ppVolume)
    {
        volume = ppVolume;
        profile = volume.profile;

        profile.TryGet(out colours);
        profile.TryGet(out chromatic);
    }

    public static void SetColourEffect(float saturation, float time = 0)
    {
        GameManager.Instance.StartCoroutine(AnimateEffect((float value) =>
        {
            colours.saturation.value = value;
        }, colours.saturation.value, saturation, time));
    }

    public static void SetChromaticAberration(float intensity, float time = 0)
    {
        GameManager.Instance.StartCoroutine(AnimateEffect((float value) =>
        {
            chromatic.intensity.value = value;
        }, chromatic.intensity.value, intensity, time));
    }

    private static IEnumerator AnimateEffect(Action<float> action, float start, float end, float duration)
    {
        float time = 0f;
        while (time <= duration)
        {
            time += Time.unscaledDeltaTime;
            action(Mathf.Lerp(start, end, time / duration));
            yield return null;
        }
    }
}
