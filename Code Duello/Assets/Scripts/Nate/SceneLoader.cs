using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public class SceneLoader : MonoBehaviour
{
    public string sceneName;

    public bool isPlayerOneReady = false, isPlayerTwoReady = false;


    private void Update()
    {
        if(isPlayerOneReady && isPlayerTwoReady) 
        {
            isPlayerOneReady = false;
            isPlayerTwoReady = false;

            StartGame();

        }
    }

    public void OnChangeScene() 
    {
        SceneManager.LoadScene(sceneName, LoadSceneMode.Single);
    }

    public void StartGame() 
    {
        SceneManager.LoadScene(sceneName, LoadSceneMode.Single);
    }



}