using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }
    public Environment Environment { get; private set; }
    public PlayerEntity Player { get; private set; }

    private void Awake()
    {
        if (Instance != null)
            Destroy(gameObject);

        Instance = this;
        DontDestroyOnLoad(gameObject);

        Environment = GetComponent<Environment>();
        Player = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerEntity>();
    }
}
