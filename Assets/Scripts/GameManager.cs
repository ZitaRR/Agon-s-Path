using System;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public enum GameState
    {
        Menu,
        Idle,
        Combat,
        Paused
    }

    public delegate void StateDelegate(GameState state);
    public static event StateDelegate OnStateChange;

    public static GameManager Instance { get; private set; }
    public static Environment Environment { get; private set; }
    public static PlayerEntity Player => Instance.player.GetComponent<PlayerEntity>();
    public static GameState State { get; private set; }

    [SerializeField]
    private GameObject player;

    private void Awake()
    {
        if (Instance != null)
            Destroy(gameObject);

        Instance = this;
        DontDestroyOnLoad(gameObject);

        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    public static void SetState(GameState state)
    {
        State = state;
        OnStateChange?.Invoke(state);
    }

    public void LoadScene(string name)
        => SceneLoader.LoadScene(name);

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        if (!scene.name.Contains("PL") && scene.name != "SampleScene")
            return;

        Environment = gameObject.AddComponent<Environment>();
        SetState(GameState.Idle);
        player = Instantiate(player, new Vector2(0, -3), Quaternion.identity);
    }
}
