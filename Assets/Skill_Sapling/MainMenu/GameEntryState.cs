using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;


public enum EntryState
{
    Added,
    Adding
}

public class GameEntryState : MonoBehaviour
{
    public EntryState currentState = EntryState.Added;

    public GameObject SetButton;
    public Button SetButtonAsButton;
    public GameObject DeleteButton;
    public Button DeleteButtonAsButton;
    public GameObject PlayButton;
    public TMP_Text PlayButtonText;

    public TMP_InputField TextField;

    public String EntryName;
    
    public void SetState(EntryState state)
    {
        if (state == EntryState.Added)
        {
            SetButton.SetActive(false);
            DeleteButton.SetActive(true);
            PlayButtonText.text = "Play: " + EntryName;
            TextField.gameObject.SetActive(false);
            PlayButton.gameObject.SetActive(true);
            PlayButton.GetComponent<Button>().onClick.AddListener(StartGame);
        } else if (state == EntryState.Adding)
        {
            SetButton.SetActive(true);
            DeleteButton.SetActive(false);
            TextField.enabled = true;
        }
    }
    
    
    public void StartGame()
    {
        Debug.Log("Starting Game");
        string loadVal = "Entry_" + EntryName;
        if(PlayerPrefs.HasKey(loadVal)) Debug.Log("Saved State found!!!");
        PassLoadedBag.BagToLoad = PlayerPrefs.GetString(loadVal);
        PassLoadedBag.AccountName = EntryName;
        Debug.Log("Load Entry: (" + loadVal + ") " + PassLoadedBag.BagToLoad);
        Debug.Log("Account Name: " + PassLoadedBag.AccountName);
        SceneManager.LoadScene(1);
    }

    public void setNameValueChange(String input)
    {
        Debug.Log("value change: " + input);
        EntryName = input;
    }

    public void Focus()
    {
        TextField.ActivateInputField();
        TextField.Select();
    }
    
}
