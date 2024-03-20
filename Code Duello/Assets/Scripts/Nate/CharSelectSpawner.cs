using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Users;

public class CharSelectSpawner : MonoBehaviour
{

    [SerializeField] private SceneLoader sceneLoader;
    [SerializeField] private GameObject platformPrfeab;
    [SerializeField] private Transform patform1Trans, platform2Trans;



    // Start is called before the first frame update
    void Start()
    {
        SpawnPlatforms();
    }   


    private void SpawnPlatforms() 
    {
        PlayerInput playerOnePlayerinput = PlayerInput.Instantiate(platformPrfeab, controlScheme: "Gamepad", pairWithDevice: Gamepad.all[0]);
        playerOnePlayerinput.transform.rotation = patform1Trans.rotation;
        playerOnePlayerinput.transform.position = patform1Trans.position;
        CharacterSelection player1CS = playerOnePlayerinput.GetComponent<CharacterSelection>();
        player1CS.sceneLoader = sceneLoader;

        PlayerInput playerTwoPlayerinput = PlayerInput.Instantiate(platformPrfeab, controlScheme: "Gamepad", pairWithDevice: Gamepad.all[1]);
        playerTwoPlayerinput.transform.rotation = platform2Trans.rotation;
        playerTwoPlayerinput.transform.position = platform2Trans.position;
        CharacterSelection player2CS = playerTwoPlayerinput.GetComponent<CharacterSelection>();
        player2CS.sceneLoader = sceneLoader;
        player2CS.selectionID = 1;

    }




}
