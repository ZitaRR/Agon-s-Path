using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public sealed class SceneLoader : MonoBehaviour
{
    public static string SceneName { get; private set; }

    private AsyncOperation loading;
    private CanvasGroup group;
    private readonly string[] messages =
    {
        "You can move using W, A, S, D - if you did not know this, perhaps you should uninstall",
        "Are you having trouble staying alive? Just stop dying",
        "Health potions is an excellent choice when you're in a rough spot, if you're shit that is",
        "Enemies too tough? Come back when you got better gear or ride it out like a real man",
        "You better not be here because you died"
    };

    [SerializeField]
    private Text loadingText;
    [SerializeField]
    private Text loadingMessage;
    [SerializeField]
    private Slider loadingBar;

    private void Awake()
    {
        group = GetComponent<CanvasGroup>();
        group.alpha = 0;
    }

    private void Start()
    {
        int index = Random.Range(0, messages.Length);
        loadingMessage.text = messages[index];
        StartCoroutine(Load());
    }

    private IEnumerator Load()
    {
        yield return StartCoroutine(SetAlpha(1, .5f));
        loading = SceneManager.LoadSceneAsync(SceneName);
        loading.allowSceneActivation = false;
        while (!loading.isDone)
        {
            var percentage = (loading.progress + .1f) * 100;
            loadingText.text = $"Loading {percentage}%";
            loadingBar.value = percentage;
            if (percentage >= 100)
                break;
            yield return null;
        }
        yield return StartCoroutine(Unload());
    }

    private IEnumerator Unload()
    {
        yield return StartCoroutine(SetAlpha(0, .5f));
        loading.allowSceneActivation = true;
    }

    private IEnumerator SetAlpha(float alpha, float duration)
    {
        float start = group.alpha;
        float time = 0;

        while (time < duration)
        {
            group.alpha = Mathf.Lerp(start, alpha, time / duration);
            time += Time.deltaTime;
            yield return null;
        }
        group.alpha = alpha;
    }

    public static void LoadScene(string name)
    {
        SceneName = name;
        SceneManager.LoadScene("LoadingScene");
    }
}
