using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public class SceneLoader : MonoBehaviour
{
    public string sceneName;

    [SerializeField] private GameObject player1PrefabTest;
    [SerializeField] private GameObject player2PrefabTest;

    public void OnChangeScene() 
    {
        SpawnManager.player1Prefab = player1PrefabTest;
        SpawnManager.player2Prefab = player2PrefabTest;
        SceneManager.LoadScene(sceneName, LoadSceneMode.Single);
    }
}