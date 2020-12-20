using UnityEngine;
using static UnityEngine.ParticleSystem;

public class Environment : MonoBehaviour
{
    public const float DAY_LENGTH = 24;

    public float Time { get; set; }
    public bool IsDay => Time >= dayStart && Time < nightStart;

    private Light sun;
    private ParticleSystem weather;
    private MainModule main;
    private float sunIntensity;
    private float duration;
    private float cooldown;

    [Header("Time Settings")]
    [SerializeField]
    [Range(0, 24)]
    private float start;
    [SerializeField]
    private float timeMultiplier = 1;
    [SerializeField]
    private float dayStart = 6;
    [SerializeField]
    private float nightStart = 18;

    [Header("Weather Settings")]
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

    private void Start()
    {
        Time = start;
    }

    private void FixedUpdate()
    {
        main.simulationSpeed = timeMultiplier;

        var tick = TimeIncrement();
        Time += tick;
        
        if (Time >= DAY_LENGTH)
            Time = 0;

        UpdateSun(tick);
        UpdateWeather(tick);
    }

    private void UpdateSun(float time)
    {
        switch (Time)
        {
            case float t when (t >= dayStart + 1 && t < nightStart):
                sunIntensity = 1;
                break;
            case float t when (t >= nightStart + 1 || t < dayStart):
                sunIntensity = 0;
                break;
            default:
                time = IsDay ? time : -time;
                sunIntensity += time;
                break;
        }

        if (sunIntensity > 1)
            sunIntensity = 1;
        else if (sunIntensity < 0)
            sunIntensity = 0;

        sun.intensity = sunIntensity;
    }

    private void UpdateWeather(float time)
    {
        duration -= time > duration ? duration : time;
        if (duration > 0)
            return;
        cooldown -= time > cooldown ? cooldown : time;
        if (cooldown > 0)
            return;
        if (Random.value * 1000 > frequence)
            return;

        duration = Random.value * durationMulitplier;
        cooldown = weatherCooldown;
        main.duration = duration * DAY_LENGTH;
        weather.Play();
    }

    private float TimeIncrement()
        => UnityEngine.Time.deltaTime * timeMultiplier / DAY_LENGTH;
}
