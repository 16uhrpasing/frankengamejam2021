using System;
using System.Collections;
using System.Collections.Generic;
using System.Data.Common;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public enum MainMenuState
{
    Idle,
    Adding
}


public class GameManager : MonoBehaviour
{
    public MainMenuState currentState = MainMenuState.Idle;

    public GameEntryState currentGameEntryState;
    
    public GameObject SavedGames;
    
    public GameObject GameEntryTemplate;
    
    public Button NewGameButton;

    public List<String> Games;

    // Start is called before the first frame update
    void Start()
    {
        //DeleteSaved();
        //AddNewGameEntry("One");
        //AddNewGameEntry("Two");
        ReloadGames();
    }

    private void Update()
    {
        if (Input.GetKeyUp(KeyCode.Return) && currentState == MainMenuState.Adding)
        {
            SetAdd();
        }
    }

    public void DeleteByName(string name)
    {
        Debug.Log("Deleting: " + name);
        PlayerPrefs.DeleteKey("Entry_"+name);
        Games.RemoveAt(Games.IndexOf(name));
        SaveGames();
        ReloadGames();
    }

    public void SetAdd()
    {
        Debug.Log("set add called");
        currentGameEntryState.setNameValueChange(currentGameEntryState.TextField.text);    
        AddingDone();
    }

    //empty name = new (adding state)
    public void AddNewGameEntry(string Name)
    {
        if (currentState != MainMenuState.Idle) return;
        currentState = MainMenuState.Adding;
        NewGameButton.enabled = false;
        GameObject newGameEntry = Instantiate(GameEntryTemplate);
        GameEntryState GES = newGameEntry.GetComponent<GameEntryState>();
        GES.SetButtonAsButton.onClick.AddListener(SetAdd);
        GES.DeleteButtonAsButton.onClick.AddListener(delegate{DeleteByName(GES.EntryName);});
        
        newGameEntry.transform.SetParent(SavedGames.transform);
        newGameEntry.SetActive(true);
        newGameEntry.transform.localScale = new Vector3(1.0f,1.0f,1.0f);
        currentGameEntryState = newGameEntry.GetComponent<GameEntryState>();

        currentGameEntryState.SetState(EntryState.Adding);
        if (Name.Equals(""))
        {
            currentGameEntryState.Focus();
        }
        else
        {
            currentGameEntryState.setNameValueChange(Name);       
            AddingDone();
        }
        
    }

    public void AddingDone()
    {
        if (currentState != MainMenuState.Adding) return;
        currentState = MainMenuState.Idle;
        currentGameEntryState.SetState(EntryState.Added);
        string newName = currentGameEntryState.EntryName;
        if(!PlayerPrefs.HasKey("Entry_" + newName))PlayerPrefs.SetString("Entry_" + newName, "");
        Games.Add(newName);
        Debug.Log("Adding finished: EntryName: " + newName);
        NewGameButton.enabled = true;
        SaveGames();
    }

    public void ReloadGames()
    {
        foreach (Transform child in SavedGames.transform) {
            GameObject.Destroy(child.gameObject);
        }
        Games.Clear();
        if (PlayerPrefs.HasKey("GameEntries"))
        {
            String EntryDB = PlayerPrefs.GetString("GameEntries");
            String[] splitted = EntryDB.Split('\n');
            foreach(String entry in splitted)
            {
                Debug.Log("loading: " + entry);
                AddNewGameEntry(entry);
                //Games.Add(entry);
            }
        }
        
        Debug.Log("Loaded Games: ");
        Debug.Log(Games);
    }

    public void SaveGames()
    {
        if (Games.Count == 0) return;
        String toDB = "";
        for(int i = 0; i < Games.Count; i++)
        {
            toDB += Games[i];
            if (i != Games.Count - 1) toDB += "\n";
        }
        PlayerPrefs.SetString("GameEntries", toDB);
        Debug.Log("toDB: " + toDB);
    }

    public void DeleteSaved()
    {
        PlayerPrefs.DeleteKey("GameEntries");
    }

    public void QuitGame()
    {
        Application.Quit();
    }

 
    
}
