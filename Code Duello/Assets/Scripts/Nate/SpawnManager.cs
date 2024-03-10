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

    [Header("Player Health Sliders")][Space]

    [SerializeField] public Slider player1HSlider, player2HSlider;

    [Header("Player Spawn Locations")][Space]

    [SerializeField] private Transform player1SpawnLoc;
    [SerializeField] private Transform player2SpawnLoc;

    // Start is called before the first frame update
    void Start()
    {
        p1 = PlayerInput.Instantiate(player1Prefab, controlScheme: "Gamepad", pairWithDevice: Gamepad.all[0]);
        p2 = PlayerInput.Instantiate(player2Prefab, controlScheme: "Gamepad", pairWithDevice: Gamepad.all[1]);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnPlayerJoined(PlayerInput playerInput) 
    {
        playerCount++;
        SetPlayDefaults(playerInput.gameObject);
    }

    public void SetPlayDefaults(GameObject player) 
    {
        if (playerCount == 1)
        {
            Debug.Log("Spawn 1");
            player.transform.rotation = player1SpawnLoc.rotation;
            PlayerHealthClass player1HC = player.GetComponent<PlayerHealthClass>();
            player1HC.healthSlider = player1HSlider;
            player.transform.position = new Vector3(player1SpawnLoc.position.x, player1SpawnLoc.position.y, player1SpawnLoc.position.z);

            MovementController player1MC = player.GetComponent<MovementController>();
            player1MC.playerVal = 0;
        }
        else if (playerCount == 2)
        {
            Debug.Log("Spawn 2");
            player.transform.rotation = player2SpawnLoc.rotation;
            PlayerHealthClass player2HC = player.GetComponent<PlayerHealthClass>();
            player2HC.healthSlider = player2HSlider;
            MovementController player2MC = player.GetComponent<MovementController>();
            player2MC.invertContorls = true;
            player2MC.playerVal = 1;
            player.transform.position = player2SpawnLoc.position;
        }
    }
}
