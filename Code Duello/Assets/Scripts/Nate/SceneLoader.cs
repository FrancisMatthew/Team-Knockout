using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public class SceneLoader : MonoBehaviour
{
    public string sceneName;

    public int characterOneChoice;

    [SerializeField] private GameObject[] characters;

    [SerializeField] private GameObject player1PrefabTest;
    [SerializeField] private GameObject player2PrefabTest;
    [SerializeField] private int playerOneChoice, playerTwoChoice;


    public void OnChangeScene() 
    {

        //SpawnManager.player1Prefab = player1PrefabTest;
        //SpawnManager.player2Prefab = player2PrefabTest;
        SceneManager.LoadScene(sceneName, LoadSceneMode.Single);
    }

    public void OnStartGame() 
    {
        PlayerPrefs.SetInt("playerOneChoice", playerOneChoice);
        PlayerPrefs.SetInt("playerTwoChoice", playerTwoChoice);
        SceneManager.LoadScene(sceneName, LoadSceneMode.Single);
    }



}