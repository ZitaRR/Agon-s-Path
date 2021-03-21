using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public sealed class GameManager : MonoBehaviour
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
    public static PlayerEntity Player { get; private set; }
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

    private void Start()
    {
        Player = player.GetComponent<PlayerEntity>();
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
