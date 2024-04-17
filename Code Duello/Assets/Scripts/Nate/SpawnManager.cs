using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Users;
using UnityEngine.UI;

public class SpawnManager : MonoBehaviour
{

    [Header("Player Refs")][Space]

    [SerializeField] private PlayerInput p1;
    [SerializeField] private GameObject player1Prefab;
    [SerializeField] private PlayerInput p2;
    [SerializeField] private GameObject player2Prefab;
    [SerializeField] private int playerCount = 0;



    [SerializeField] private GameObject[] characters;
    [SerializeField] private int playerOneChoice, playerTwoChoice;

    [Header("Debug Keys")][Space]
    [SerializeField] private KeyCode spawnKey;

    [Header("Player Health Sliders")][Space]

    [SerializeField] public Slider player1HSlider, player2HSlider;

    [Header("Player Spawn Locations")][Space]

    [SerializeField] private Transform player1SpawnLoc;
    [SerializeField] private Transform player2SpawnLoc;

    // Start is called before the first frame update
    void Start()
    {

        playerOneChoice = PlayerPrefs.GetInt("playerOneChoice", playerOneChoice);
        playerTwoChoice = PlayerPrefs.GetInt("playerTwoChoice", playerTwoChoice);
        player1Prefab = characters[playerOneChoice];
        player2Prefab = characters[playerTwoChoice];

        SpawnPlayer();
        SpawnPlayer();

    }
    public void SpawnPlayer() 
    {
        if (playerCount == 0)
        {
            Debug.Log(Gamepad.all[playerCount]);
            p1 = PlayerInput.Instantiate(player1Prefab, controlScheme: "Gamepad", pairWithDevice: Gamepad.all[playerCount]);
            p1.name = "player 1";
            playerCount++;
            SetPlayDefaults(p1.gameObject);
        }
        else if (playerCount == 1)
        {
            Debug.Log(Gamepad.all[playerCount]);
            p2 = PlayerInput.Instantiate(player2Prefab, controlScheme: "Gamepad", pairWithDevice: Gamepad.all[playerCount]); 
            p2.name = "player 2";
            playerCount++;
            SetPlayDefaults(p2.gameObject);
        }
    }


    public void SetPlayDefaults(GameObject player) 
    {
        if (playerCount == 1)
        {
            player.transform.rotation = player1SpawnLoc.rotation;
            player.transform.position = player1SpawnLoc.position;
            PlayerHealthClass player1HC = player.GetComponent<PlayerHealthClass>();
            player1HC.healthSlider = player1HSlider;
            MovementController player1MC = player.GetComponent<MovementController>();
            player1MC.playerVal = 0;
        }
        else if (playerCount == 2)
        {
            Debug.Log("Spawn 2");
            player.transform.rotation = player2SpawnLoc.rotation;
            player.transform.position = player2SpawnLoc.position;
            PlayerHealthClass player2HC = player.GetComponent<PlayerHealthClass>();
            player2HC.healthSlider = player2HSlider;
            MovementController player2MC = player.GetComponent<MovementController>();
            player2MC.invertContorls = true;
            player2MC.playerVal = 1;
        }
    }
}
