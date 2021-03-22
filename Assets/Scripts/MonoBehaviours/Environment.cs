using UnityEngine;
using static UnityEngine.ParticleSystem;

public sealed class Environment : MonoBehaviour
{
    public const float DAY_LENGTH = 24;

    public float Time { get; set; }
    public float LightIntensity { get; private set; } = 1;
    public bool IsDay => Time >= dayStart && Time < nightStart;

    private Light sun;
    private Light player;
    private ParticleSystem weather;
    private MainModule main;
    private float duration;
    private float cooldown;

    [Header("Time Settings")]
    [SerializeField]
    [Range(0, 24)]
    private float start = 12;
    [SerializeField]
    private float timeMultiplier = 1;
    [SerializeField]
    private float dayStart = 6;
    [SerializeField]
    private float nightStart = 18;

    [Header("Weather Settings")]
    [SerializeField]
    private float durationMulitplier = 4;
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

    private void OnEnable()
    {
        if (GameManager.IsPaused)
            return;

        player = GameManager.Player.GetComponentInChildren<Light>();
    }

    private void FixedUpdate()
    {
        main.simulationSpeed = timeMultiplier;

        var tick = TimeIncrement();
        Time += tick;
        
        if (Time >= DAY_LENGTH)
            Time = 0;

        UpdateLight();
        UpdateWeather(tick);
    }

    private void UpdateLight()
    {
        var light = IsDay ? Time - dayStart : 
            Time < dayStart ? Time - nightStart : -Time + (nightStart + 1);
        LightIntensity = Mathf.Clamp(light, 0, 1);

        sun.intensity = LightIntensity;
        player.intensity = -LightIntensity + .5f;
    }

    private void UpdateWeather(float tick)
    {
        duration -= tick > duration ? duration : tick;
        if (duration > 0)
            return;
        cooldown -= tick > cooldown ? cooldown : tick;
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
