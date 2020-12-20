using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public sealed class SceneLoader : MonoBehaviour
{
    public static string SceneName { get; private set; }

    private AsyncOperation loading;
    private CanvasGroup group;

    [SerializeField]
    private Text loadingText;
    [SerializeField]
    private Slider loadingBar;

    private void Awake()
    {
        group = GetComponent<CanvasGroup>();
    }

    private void Start()
    {
        StartCoroutine(Load());
    }

    private IEnumerator Load()
    {
        yield return StartCoroutine(SetAlpha(1, 1));
        loading = SceneManager.LoadSceneAsync(SceneName);
        while (!loading.isDone)
        {
            loadingText.text = $"Loading {loading.progress * 100:00}%";
            loadingBar.value = loading.progress * 100;
            yield return null;
        }
        yield return StartCoroutine(SetAlpha(0, 1));
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
