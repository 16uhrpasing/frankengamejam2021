using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class AbilitySpawnManager : MonoBehaviour
{
    public List<Transform> spawnTransforms;
    public List<Transform> homeTransforms;
    
    public List<GameObject> spawnAbilityPrefabs;
    
    public List<Ability> spawnAbilityEnums;
    
    public AbilityBag playerAbilityBag;
    public TMP_Text abilityInfoText;

    public GameEvent abilityChangedEvent;

    // Start is called before the first frame update
    void Start()
    {
        SpawnAbilities();
    }

    public void SpawnAbilities()
    {
        for (int i = 0; i < spawnTransforms.Count; i++)
        {
            GameObject spawnedAbility = Instantiate(spawnAbilityPrefabs[i]);

            if (playerAbilityBag.abilitesInResetRoom.Contains(spawnAbilityEnums[i]))
            {
                spawnedAbility.gameObject.transform.position = homeTransforms[i].position;
            }
            else
            {
                spawnedAbility.gameObject.transform.position = spawnTransforms[i].position;
            }
            
            AbilityCircle abilityCircle = spawnedAbility.GetComponent<AbilityCircle>();
            abilityCircle.thisAbility = spawnAbilityEnums[i];
            abilityCircle.abilityInfoText = abilityInfoText;
            abilityCircle.playerAbilityBag = playerAbilityBag;
            abilityCircle.AbilityChanged = abilityChangedEvent;
        }
    }

    public void SpawnAbilityAt(Ability ability, Vector3 position)
    {
        int i = spawnAbilityEnums.IndexOf(ability);
        
        GameObject spawnedAbility = Instantiate(spawnAbilityPrefabs[i]);
        spawnedAbility.gameObject.transform.position = position;
        AbilityCircle abilityCircle = spawnedAbility.GetComponent<AbilityCircle>();
        abilityCircle.thisAbility = spawnAbilityEnums[i];
        abilityCircle.abilityInfoText = abilityInfoText;
        abilityCircle.playerAbilityBag = playerAbilityBag;
        abilityCircle.AbilityChanged = abilityChangedEvent;
    }

    public void Respawn()
    {
       DestroyAllAbilities();
       SpawnAbilities();
    }

    public void DestroyAllAbilities()
    {
        //Alle Abilities vernichten
        GameObject[] abilitiesOnMap = GameObject.FindGameObjectsWithTag("AbilityTag");
        for(int i=0; i< abilitiesOnMap. Length; i++)
        {
            Destroy(abilitiesOnMap[i]);
        }
    }
  
}
