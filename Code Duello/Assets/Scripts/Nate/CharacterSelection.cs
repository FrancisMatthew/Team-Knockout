using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Users;

public class CharacterSelection : MonoBehaviour
{

    [SerializeField] private InputDevice playerDevice;
    [SerializeField] private PlayerInput playerInput;

    public SceneLoader sceneLoader;
    public int selectionID = 0;

    [SerializeField] private int currentChoice = 0;
    [SerializeField] private bool isCharacterSelected = false;

    [SerializeField] private GameObject[] characters;

    [SerializeField] private int playerOneChoice, playerTwoChoice;


    private void OnNextCharacter() 
    {
        if (!isCharacterSelected) 
        {
            characters[currentChoice].SetActive(false);
            currentChoice = (currentChoice + 1) % characters.Length;
            characters[currentChoice].SetActive(true);
        }
    }

    private void OnPreviousCharacter() 
    {
        if (!isCharacterSelected)
        {
            characters[currentChoice].SetActive(false);
            currentChoice--;
            if (currentChoice < 0)
            {
                currentChoice += characters.Length;
            }
            characters[currentChoice].SetActive(true);
        }
    }


    private void OnSelectCharacter()
    {
        if (!isCharacterSelected) 
        {
            if (selectionID == 0)
            {
                playerOneChoice = currentChoice;
                PlayerPrefs.SetInt("playerOneChoice", playerOneChoice);
                isCharacterSelected = true;
                sceneLoader.isPlayerOneReady = true;
            }
            else
            {

                playerTwoChoice = currentChoice;
                PlayerPrefs.SetInt("playerTwoChoice", playerTwoChoice);
                isCharacterSelected = true;
                sceneLoader.isPlayerTwoReady = true;
                
            }
        }
    }

}
