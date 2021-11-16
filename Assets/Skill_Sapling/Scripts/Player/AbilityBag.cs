using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public enum Ability
{
    None,
    Shroom,
    WallJump,
    Dash,
    Roots
}

public class AbilityBag : MonoBehaviour
{

    public AbilitySpawnManager abilitySpawnManager;
    public HashSet<Ability> abilitesInResetRoom = new HashSet<Ability>();
    public SpriteSwapper spriteSwapper;
    public bool firstLoad = true;
    
     [SerializeField] public Ability currentAbility { get; set; } = Ability.None;

     public GameEvent abilityChangedEvent;
     public GameEvent respawnEvent;
     public GameEvent allBack;
     
     public GameObject DashZacken;
     public GameObject WallZacken;
     public GameObject SunZacken;
     public GameObject LampZacken;
     public GameObject RootsZacken;
     
    private void Start()
    {
        if (firstLoad)
        {
            Debug.Log("LOGDBDEBUG: " + "Entry_"+PassLoadedBag.AccountName);
            string loadDB = PassLoadedBag.BagToLoad;
            Debug.Log("loadDB: " + loadDB);
            string[] splitted = loadDB.Split('\n');
            foreach (string abilityString in splitted)
            {
                if (abilityString.Equals("dash"))
                    abilitesInResetRoom.Add(Ability.Dash);
                if (abilityString.Equals("climb"))
                    abilitesInResetRoom.Add(Ability.WallJump);
                if (abilityString.Equals("shroom"))
                    abilitesInResetRoom.Add(Ability.Shroom);
                if (abilityString.Equals("root"))
                    abilitesInResetRoom.Add(Ability.Roots);
            }

            firstLoad = false;
        }
        UpdateGUISprite();
        SyncZackenWithAbilities();
        //respawnEvent.Raise();
        //currentAbility = Ability.None;
    }

    public void SyncZackenWithAbilities()
    {
        DashZacken.SetActive(false);
        WallZacken.SetActive(false);
        SunZacken.SetActive(false);
        LampZacken.SetActive(false);
        RootsZacken.SetActive(false);
        
        
        if(abilitesInResetRoom.Contains(Ability.Dash)) DashZacken.SetActive(true);
        if(abilitesInResetRoom.Contains(Ability.WallJump)) WallZacken.SetActive(true);
        if(abilitesInResetRoom.Contains(Ability.Shroom)) LampZacken.SetActive(true);
        if(abilitesInResetRoom.Contains(Ability.Roots)) RootsZacken.SetActive(true);

        if (abilitesInResetRoom.Count == 4)
        {
            SunZacken.SetActive(true);
            allBack.Raise();
        }

        
    }   
    
    public void SaveGame()
    {
        string db = "";
        foreach (Ability inRoomAbility in abilitesInResetRoom)
        {
            switch (inRoomAbility)
            {
                case Ability.Dash:
                    db += "dash\n";
                    Debug.Log("Dash Saved");
                    break;
                case Ability.WallJump:
                    db += "climb\n";
                    Debug.Log("Climb Saved");
                    break;
                case Ability.Shroom:
                    db += "shroom\n";
                    Debug.Log("Shroom Saved");
                    break;
                case Ability.Roots:
                    db += "root\n";
                    Debug.Log("Root Saved");
                    break;
            }
        }
        
        PlayerPrefs.SetString("Entry_"+PassLoadedBag.AccountName, db);
        Debug.Log("Saving Account: " + PassLoadedBag.AccountName);
        Debug.Log("With DB: " + db);
        Debug.Log("Test Load in play scene:::");
        Debug.Log(PlayerPrefs.GetString("Entry_" + PassLoadedBag.AccountName));
    }

    public void UpdateGUISprite()
    {
        spriteSwapper.SetAbilityGUISprite(currentAbility);
    }

    public void DropAbility()
    {
        if (currentAbility == Ability.None) return;
        Debug.Log("Dropping ability: " + currentAbility.ToString());
        abilitySpawnManager.SpawnAbilityAt(currentAbility, gameObject.transform.position);
        currentAbility = Ability.None;
        UpdateGUISprite();
        abilityChangedEvent.Raise();
    }
    // Update is called once per frame
    void Update()
    {
        //Q zum fallen lassen
        if (Input.GetKeyDown(KeyCode.Q))
        {
           DropAbility();
        }
        //Debug.Log("current: " + currentAbility.ToString());
        //E zum swappen
    }

    public void BringAbilityToResetRoom()
    {
        if (currentAbility == Ability.None) return;
        if(!abilitesInResetRoom.Contains(currentAbility)) abilitesInResetRoom.Add(currentAbility);
        SetAbilityNone();
        abilityChangedEvent.Raise();
        SaveGame();
        SyncZackenWithAbilities();
    }

    public void SetAbilityNone()
    {
        currentAbility = Ability.None;
        UpdateGUISprite();
        abilityChangedEvent.Raise();
    }
    
  
}