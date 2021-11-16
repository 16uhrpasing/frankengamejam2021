using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class AbilityCircle : MonoBehaviour
{
    public Ability thisAbility;
    public AbilityBag playerAbilityBag;
    public TMP_Text abilityInfoText;
    public bool hovering = false;

    public GameEvent AbilityChanged;

    void OnTriggerEnter2D(Collider2D collision)
    {
        hovering = true;
        if (playerAbilityBag.currentAbility == Ability.None)
        {
            abilityInfoText.SetText("Press E to learn " + thisAbility.ToString());
        }
        else
        {
            abilityInfoText.SetText("Press E: Change " + playerAbilityBag.currentAbility.ToString() + " to " +
                                    thisAbility.ToString());
        }
    }

    void OnTriggerExit2D(Collider2D collision)
    {
        hovering = false;
        abilityInfoText.SetText( "");
        Debug.Log("ability exited: " + thisAbility.ToString());
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.E) && hovering)
        {
            if (playerAbilityBag.currentAbility != Ability.None)
            {
                Debug.Log("Dropping ability "+ playerAbilityBag.currentAbility.ToString()+" for new one");
                playerAbilityBag.DropAbility();
            }
            playerAbilityBag.currentAbility = thisAbility;
            playerAbilityBag.UpdateGUISprite();
            AbilityChanged.Raise();
            Destroy(gameObject);

        }
    }
}
