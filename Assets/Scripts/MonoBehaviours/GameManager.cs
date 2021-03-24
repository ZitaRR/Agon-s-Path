using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.SceneManagement;

public sealed class GameManager : MonoBehaviour
{
    public enum GameState
    {
        Menu,
        Idle,
        Combat,
        Paused,
        Loading
    }

    public delegate void FrameDelegate();
    public delegate void StateDelegate(GameState state);
    public static event FrameDelegate OnFrame;
    public static event StateDelegate OnStateChange;

    public static GameManager Instance { get; private set; }
    public static Environment Environment { get; private set; }
    public static PlayerEntity Player { get; private set; }
    public static UserInterface UI { get; private set; }
    public static GameState State { get; private set; }
    public static bool IsPlaying { get => State is GameState.Idle || State is GameState.Combat; }
    public static bool IsPaused { get => State is GameState.Paused || State is GameState.Menu; }
    public static bool InCombat { get => State is GameState.Combat; }

    [SerializeField]
    private GameObject player;

    private void Awake()
    {
        if (Instance != null)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);

        Environment = gameObject.GetComponentInChildren<Environment>();
        UI = gameObject.GetComponentInChildren<UserInterface>();
        PostProcessing.Initialize(GetComponent<Volume>());

        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    private void Update()
    {
        OnFrame?.Invoke();
    }

    public static void SetState(GameState state)
    {
        if (State == state)
            return;

        Debug.Log($"Game state changed from [{State}] to [{state}].");
        State = state;
        OnStateChange?.Invoke(state);
    }

    public void LoadScene(string name, Vector2 pos)
        => SceneLoader.LoadScene(name, pos);

    public void LoadScene(string name)
        => SceneLoader.LoadScene(name, default);

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        Environment.gameObject.SetActive(false);

        if (scene.name == "Menu")
        {
            SetState(GameState.Menu);
            return;
        }
        else if (scene.name == "LoadingScene")
        {
            SetState(GameState.Loading);
            return;
        }
        
        SetState(GameState.Idle);
        Player = Instantiate(player, SceneLoader.StartPosition, Quaternion.identity)
            .GetComponent<PlayerEntity>();
        Environment.gameObject.SetActive(true);
    }

    public static void SetTime(float time, float duration = 0)
    {
        Instance.StartCoroutine(PostProcessing.AnimateEffect((float value) =>
        {
            Time.timeScale = value;
        }, Time.timeScale, time, duration));
    }

    private void OnDrawGizmos()
    {
        if (ASTAR.Nodes is null)
            return;

        foreach (Node node in ASTAR.Nodes)
        {
            if (node.Walkable)
                Gizmos.color = Color.cyan;
            else Gizmos.color = Color.red;

            Gizmos.DrawCube(new Vector3(node.X, node.Y, 0), new Vector3(.2f, .2f, 0));
        }
    }
}

static class Extensions
{
    public static Vector2 GetCentroid(this List<Vector2> positions)
    {
        var centroid = new Vector2(0, 0);
        foreach (var pos in positions)
            centroid += pos;
        return centroid / positions.Count;
    }
}
