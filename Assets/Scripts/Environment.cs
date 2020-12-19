using UnityEngine;
using static UnityEngine.ParticleSystem;

public class Environment : MonoBehaviour
{
    public const float DAY_LENGTH = 24;

    public bool IsDay => time >= dayStart && time < nightStart;

    private Light sun;
    private ParticleSystem weather;
    private MainModule main;
    private float time;
    private float sunIntensity;
    private float duration;
    private float cooldown;

    [Header("Time Settings")]
    [SerializeField]
    private float timeMultiplier = 1;
    [SerializeField]
    private float dayStart = 6;
    [SerializeField]
    private float nightStart = 18;

    [Header("Weather Settings")]
    [SerializeField]
    private float intensity;
    [SerializeField]
    private float durationMulitplier;
    [SerializeField]
    private float frequence = .1f;
    [SerializeField]
    private float weatherCooldown = 4;

    private void Awake()
    {
        sun = GetComponentInChildren<Light>();
        weather = GetComponentInChildren<ParticleSystem>();
        main = weather.main;
    }

    private void FixedUpdate()
    {
        main.simulationSpeed = timeMultiplier;

        var tick = TimeIncrement();
        time += tick;
        
        if (time >= DAY_LENGTH)
            time = 0;
        
        UpdateSun();
        UpdateWeather(tick);
    }

    private void UpdateSun()
    {
        if (IsDay)
        {
            sunIntensity += TimeIncrement();
            if (sunIntensity > 1)
                sunIntensity = 1;
            sun.intensity = sunIntensity;
        }
        else
        {
            sunIntensity -= TimeIncrement();
            if (sunIntensity < 0)
                sunIntensity = 0;
            sun.intensity = sunIntensity;
        }
    }

    private void UpdateWeather(float time)
    {
        duration -= time > duration ? duration : time;
        if (duration > 0)
            return;
        cooldown -= time > cooldown ? cooldown : time;
        if (cooldown > 0)
            return;
        if (Random.value * 100 > frequence)
            return;

        duration = Random.value * durationMulitplier;
        cooldown = weatherCooldown;
        main.duration = duration * DAY_LENGTH;
        weather.Play();
    }

    private float TimeIncrement()
        => Time.deltaTime * timeMultiplier / DAY_LENGTH;
}
