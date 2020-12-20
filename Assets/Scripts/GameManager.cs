using System;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }
    public Environment Environment { get; private set; }
    public PlayerEntity Player => player.GetComponent<PlayerEntity>();

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

    public void LoadScene(string name)
        => SceneLoader.LoadScene(name);

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        if (!scene.name.Contains("PL") && scene.name != "SampleScene")
            return;

        Environment = gameObject.AddComponent<Environment>();
        player = Instantiate(player, new Vector2(8, 8), Quaternion.identity);
    }
}
