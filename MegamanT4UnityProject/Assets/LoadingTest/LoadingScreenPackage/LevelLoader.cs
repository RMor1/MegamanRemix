using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelLoader : MonoBehaviour
{
    private Animator loadingScreenAnimator;
    [System.NonSerialized] public bool finishedStart;
    private void Start()
    {
        loadingScreenAnimator = GameObject.Find("LoadingScreen").GetComponent<Animator>();
        loadingScreenAnimator.gameObject.GetComponent<CanvasGroup>().alpha = 1;
        loadingScreenAnimator.SetTrigger("loadingend");
    }
    public void LoadLevel(int sceneIndex)
    {
        StartCoroutine(LoadAsync(sceneIndex));
    }
    IEnumerator LoadAsync(int sceneIndex)
    {
        loadingScreenAnimator.SetTrigger("loadingstart");
        while(!finishedStart)
        {

            yield return null;
        }
        AsyncOperation operation = SceneManager.LoadSceneAsync(sceneIndex);
        while (!operation.isDone)
        {
            float progress = Mathf.Clamp01(operation.progress / .9f);
            Debug.Log(progress);

            yield return null;
        }
    }
}
