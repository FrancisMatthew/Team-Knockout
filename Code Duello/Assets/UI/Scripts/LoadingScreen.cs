using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class LoadingScreen : MonoBehaviour
{
    public GameObject Loading;
    public Image LoadingBarFill;

    public void LoadScene(int sceneId){
        StartCoroutine(LoadSceneAsync(sceneId));
    }

    IEnumerator LoadSceneAsync(int sceneId){

        AsyncOperation operation = SceneManager.LoadSceneAsync(sceneId);

        Loading.SetActive(true);

        // Set the fill amount of the LoadingBarFill image to 0 at the start
        LoadingBarFill.fillAmount = 0f;

        while (!operation.isDone)
        {
            float progressValue = Mathf.Clamp01(operation.progress / 0.9f);

            LoadingBarFill.fillAmount = progressValue;

            yield return null;
        }

        LoadingBarFill.fillAmount = 1f;

        yield return new WaitForSeconds(3f);

        Loading.SetActive(false);
    }
}
