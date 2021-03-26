using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.SceneManagement;

public sealed class GameManager : MonoBehaviour
{
    public delegate void FrameDelegate();
    public static event FrameDelegate OnFrame;

    public static GameManager Instance { get; private set; }
    public static Environment Environment { get; private set; }
    public static PlayerEntity Player { get; private set; }
    public static CameraBehaviour Camera { get; private set; }
    public static UserInterface UI { get; private set; }

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
        Camera = player.GetComponent<CameraBehaviour>();
        UI = gameObject.GetComponentInChildren<UserInterface>();
        PostProcessing.Initialize(GetComponent<Volume>());

        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    private void Update()
    {
        OnFrame?.Invoke();
    }

    public void SetPreviousState() 
    {
        StateMachine.SetState(StateMachine.State.Previous);
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
            StateMachine.SetState(new MenuState(StateMachine.State?.Previous));
            return;
        }
        else if (scene.name == "LoadingScene")
        {
            StateMachine.SetState(new LoadingState(StateMachine.State?.Previous));
            return;
        }

        Player = Instantiate(player, SceneLoader.StartPosition, Quaternion.identity)
            .GetComponent<PlayerEntity>();
        Environment.gameObject.SetActive(true);

        StateMachine.SetState(new IdleState(StateMachine.State?.Previous));
    }

    public static void SetTime(float time, float duration = 0)
    {
        if (duration <= 0)
        {
            Time.timeScale = time;
            return;
        }

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
